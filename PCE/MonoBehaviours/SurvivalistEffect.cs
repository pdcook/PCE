using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;
using PCE.Extensions;

namespace PCE.MonoBehaviours
{
    public class SurvivalistEffect : CounterReversibleEffect
    {
        private Dictionary<SurvivalistType, bool> pacifists = new Dictionary<SurvivalistType, bool>();

        //private readonly float interval = 0.5f;
        //private readonly float numIntervalsScaling = 20f;
        private readonly float max_mult = 3f;
        private readonly float timeToMax = 20f;

        private readonly Color maxChargeColor = Color.blue;
        private ColorFlash colorFlash = null;
        private readonly float colorFlashMin = 0.5f;
        private readonly float colorFlashMax = 3f;
        private readonly float colorFlashThreshMaxFrac = 0.25f;

        // the equation for multipliers >1 is
        /*
                  ⎛            ⎛                1           ⎞            ⎞
            Clamp ⎜1 + maxmult ⎜1 - ────────────────────────⎟, 1, maxmult⎟
                  ⎜            ⎜       ⎛       time        ⎞ ⎟            ⎟
                  ⎜            ⎜       ⎜     ────────      ⎟ ⎟            ⎟
                  ⎜            ⎜   1 + ⎜     interval      ⎟ ⎟            ⎟
                  ⎜            ⎜       ⎜───────────────────⎟ ⎟            ⎟
                  ⎝            ⎝       ⎝numIntervalsScaling⎠ ⎠            ⎠

        */
        // the equation for multipliers < 1 is just the inverse of the above (^-1)

        private float multiplier;

        // time since last damage determines the effect multiplier
        public override CounterStatus UpdateCounter()
        {

            //this.multiplier = UnityEngine.Mathf.Clamp(1f + this.max_mult * (1f - 1f / ((Time.time - (float)Traverse.Create(this.health).Field("lastDamaged").GetValue()) / (this.interval * this.numIntervalsScaling) + 1f)), 1f, this.max_mult);
            float timeSince = Time.time - (float)Traverse.Create(this.health).Field("lastDamage").GetValue();

            this.multiplier = UnityEngine.Mathf.Clamp(((this.max_mult - 1f) / (this.timeToMax)) * timeSince + 1f, 1f, this.max_mult);


            return CounterStatus.Apply;
        }

        public override void UpdateEffects()
        {
            // update which pacifist cards the player has
            this.CheckCards();

            foreach (SurvivalistType pacifistType in Enum.GetValues(typeof(SurvivalistType)))
            {
                if (this.pacifists[pacifistType])
                {
                    switch (pacifistType)
                    {
                        case SurvivalistType.I:
                            this.gunAmmoStatModifier.reloadTimeMultiplier_mult = 1f / this.multiplier;
                            break;
                        case SurvivalistType.II:
                            this.blockModifier.cdMultiplier_mult = 1f / this.multiplier;
                            break;
                        case SurvivalistType.III:
                            base.characterStatModifiersModifier.movementSpeed_mult = this.multiplier;
                            break;
                        case SurvivalistType.IV:
                            base.gunStatModifier.bulletDamageMultiplier_mult = this.multiplier;
                            break;
                        case SurvivalistType.V:
                            throw new NotImplementedException();
                            break;
                        default:
                            break;
                    }
                }
            }
            if (this.multiplier >= this.max_mult * this.colorFlashThreshMaxFrac)
            {
                this.colorFlash = base.player.gameObject.GetOrAddComponent<ColorFlash>();
                this.colorFlash.SetColor(this.maxChargeColor);
                this.colorFlash.SetNumberOfFlashes(int.MaxValue);
                float flashTime = ((this.colorFlashMin - this.colorFlashMax) / (this.max_mult - this.colorFlashThreshMaxFrac * this.max_mult)) * (this.multiplier - this.colorFlashThreshMaxFrac * this.max_mult) + this.colorFlashMax;
                this.colorFlash.SetDuration(flashTime);
                this.colorFlash.SetDelayBetweenFlashes(flashTime);
            }
            else if (this.colorFlash != null)
            {
                this.colorFlash.Destroy();
            }

        }
        public override void OnApply()
        {
        }

        private void CheckCards()
        {
            foreach (SurvivalistType pacifistType in Enum.GetValues(typeof(SurvivalistType)))
            {
                this.pacifists[pacifistType] = (Extensions.Cards.instance.CountPlayerCardsWithCondition(this.player, this.gun, this.gunAmmo, this.data, this.health, this.gravity, this.block, this.characterStatModifiers, (card, p, g, ga, d, h, gr, b, c) => card.name == this.cardNames[pacifistType]) > 0);
            }
        }

        public enum SurvivalistType
        {
            I, // faster reload
            II, // faster block cooldown
            III, // faster movement speed
            IV, // more damage
            V // Not implemented
        }
        private readonly Dictionary<SurvivalistType, string> cardNames = new Dictionary<SurvivalistType, string>()
        {
            {SurvivalistType.I, "Survivalist I" },
            {SurvivalistType.II, "Survivalist II" },
            {SurvivalistType.III, "Survivalist III" },
            {SurvivalistType.IV, "Survivalist IV" },
            {SurvivalistType.V, "Survivalist V" },
        };

    }
}

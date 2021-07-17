using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using UnboundLib.Cards;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;
using PCE.Extensions;
using PCE.Cards;

namespace PCE.MonoBehaviours
{
    public class SurvivalistEffect : CounterReversibleEffect
    {
        private Dictionary<SurvivalistType, bool> survivalists = new Dictionary<SurvivalistType, bool>();

        private readonly float max_mult = 3f;
        private readonly float defaultTimeToMax = 20f;
        private float timeToMax;

        private readonly Color maxChargeColor = Color.blue;
        private ColorFlash colorFlash = null;
        private readonly float colorFlashMin = 0.5f;
        private readonly float colorFlashMax = 3f;
        private readonly float colorFlashThreshMaxFrac = 0.25f;

        private float multiplier;
        private bool Vexists = false;

        // time since last damage determines the effect multiplier
        public override CounterStatus UpdateCounter()
        {

            float timeSince = Time.time - (float)Traverse.Create(base.health).Field("lastDamaged").GetValue();

            this.multiplier = UnityEngine.Mathf.Clamp(((this.max_mult - 1f) / (this.timeToMax)) * timeSince + 1f, 1f, this.max_mult);


            return CounterStatus.Apply;
        }

        public override void UpdateEffects()
        {
            // update which survivalist cards the player has
            this.CheckCards();

            if (this.HasCompleteSet() && !this.survivalists[SurvivalistType.V] && !this.Vexists)
            {
                // build and add survivalist V card
                CustomCard.BuildCard<SurvivalistVCard>(cardInfo => Extensions.Cards.instance.AddCardToPlayer(base.player, cardInfo));
                this.Vexists = true;
            }

            if (!this.survivalists[SurvivalistType.V])
            {
                this.timeToMax = this.defaultTimeToMax;
            }
            else
            {
                this.timeToMax = this.defaultTimeToMax / 2f;
            }

            foreach (SurvivalistType survivalistType in Enum.GetValues(typeof(SurvivalistType)))
            {
                if (this.survivalists[survivalistType])
                {
                    switch (survivalistType)
                    {
                        case SurvivalistType.I:
                            this.gunAmmoStatModifier.reloadTimeMultiplier_mult = 1f / this.multiplier;
                            break;
                        case SurvivalistType.II:
                            this.blockModifier.cdMultiplier_mult = 1f / this.multiplier;
                            break;
                        case SurvivalistType.III:
                            base.characterStatModifiersModifier.movementSpeed_mult = (this.multiplier - 1f) / 2f + 1f; // max movement speed mult is actually 2x
                            break;
                        case SurvivalistType.IV:
                            base.gunStatModifier.bulletDamageMultiplier_mult = this.multiplier;
                            break;
                        case SurvivalistType.V:
                            base.gunStatModifier.projectileColor = this.maxChargeColor;
                            break;
                        default:
                            break;
                    }
                }
            }
            if (this.multiplier == this.max_mult)
            {
                this.colorFlash = base.player.gameObject.GetOrAddComponent<ColorFlash>();
                this.colorFlash.SetColor(this.maxChargeColor);
                this.colorFlash.SetNumberOfFlashes(int.MaxValue);
                this.colorFlash.SetDuration(float.MaxValue);
                this.colorFlash.SetDelayBetweenFlashes(0);
            }
            else if (this.multiplier - 1f >= (this.max_mult - 1f) * this.colorFlashThreshMaxFrac)
            {
                this.colorFlash = base.player.gameObject.GetOrAddComponent<ColorFlash>();
                this.colorFlash.SetColor(Color.Lerp(GetPlayerColor.GetColorMax(base.player), this.maxChargeColor, this.multiplier / this.max_mult));
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
        public override void Reset()
        {
            Traverse.Create(base.health).Field("lastDamaged").SetValue(Time.time);
        }

        private void CheckCards()
        {
            foreach (SurvivalistType survivalistType in Enum.GetValues(typeof(SurvivalistType)))
            {
                this.survivalists[survivalistType] = (Extensions.Cards.instance.CountPlayerCardsWithCondition(this.player, this.gun, this.gunAmmo, this.data, this.health, this.gravity, this.block, this.characterStatModifiers, (card, p, g, ga, d, h, gr, b, c) => card.name == this.cardNames[survivalistType]) > 0);
            }
        }
        private bool HasCompleteSet()
        {
            return (this.survivalists[SurvivalistType.I] && this.survivalists[SurvivalistType.II] && this.survivalists[SurvivalistType.III] && this.survivalists[SurvivalistType.IV]);
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

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;
using PCE.Extensions;
using PCE.Cards;
using UnboundLib.Cards;

namespace PCE.MonoBehaviours
{
    public class PacifistEffect : CounterReversibleEffect
    {
        private Dictionary<PacifistType, bool> pacifists = new Dictionary<PacifistType, bool>();

        private readonly float max_mult = 3f;
        private readonly float defaultTimeToMax = 20f;
        private float timeToMax;
        private bool Vexists = false;

        private readonly Color maxChargeColor = Color.green;
        private ColorFlash colorFlash = null;
        private readonly float colorFlashMin = 0.5f;
        private readonly float colorFlashMax = 3f;
        private readonly float colorFlashThreshMaxFrac = 0.25f;

        private float multiplier;

        // time since last damage determines the effect multiplier
        public override CounterStatus UpdateCounter()
        {

            float timeSince = (float)Traverse.Create(base.characterStatModifiers).Field("sinceDealtDamage").GetValue();

            this.multiplier = UnityEngine.Mathf.Clamp(((this.max_mult - 1f) / (this.timeToMax)) * timeSince + 1f, 1f, this.max_mult);

            return CounterStatus.Apply;
        }

        public override void UpdateEffects()
        {
            // update which pacifist cards the player has
            this.CheckCards();

            if (this.HasCompleteSet() && !this.pacifists[PacifistType.V] && !this.Vexists)
            {
                // build and hide pacifist V card
                CustomCard.BuildCard<PacifistVCard>();
                Unbound.Instance.ExecuteAfterFrames(10, delegate
                {
                    List<CardInfo> activeCards = (List<CardInfo>)typeof(Unbound).GetField("activeCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

                    CardInfo Vcard = activeCards[activeCards.Count - 1];
                    activeCards.RemoveAt(activeCards.Count - 1);
                    Vcard.GetComponent<PacifistVCard>().AddCardToPlayer(base.player, Vcard);
                });
                this.Vexists = true;
            }

            if (!this.pacifists[PacifistType.V])
            {
                this.timeToMax = this.defaultTimeToMax;
            }
            else
            {
                this.timeToMax = this.defaultTimeToMax / 2f;
            }

            foreach (PacifistType pacifistType in Enum.GetValues(typeof(PacifistType)))
            {
                if (this.pacifists[pacifistType])
                {
                    switch (pacifistType)
                    {
                        case PacifistType.I:
                            this.gunAmmoStatModifier.reloadTimeMultiplier_mult = 1f/this.multiplier;
                            break;
                        case PacifistType.II:
                            this.blockModifier.cdMultiplier_mult = 1f / this.multiplier;
                            break;
                        case PacifistType.III:
                            base.characterStatModifiersModifier.movementSpeed_mult = (this.multiplier - 1f) / 2f + 1f; // max movement speed mult is actually 2x
                            break;
                        case PacifistType.IV:
                            base.gunStatModifier.bulletDamageMultiplier_mult = this.multiplier;
                            break;
                        case PacifistType.V:
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
            else if (this.multiplier - 1f >= (this.max_mult - 1f)*this.colorFlashThreshMaxFrac)
            {
                this.colorFlash = base.player.gameObject.GetOrAddComponent<ColorFlash>();
                this.colorFlash.SetColor(Color.Lerp(GetPlayerColor.GetColorMax(base.player),this.maxChargeColor, this.multiplier/this.max_mult));
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
            Traverse.Create(base.characterStatModifiers).Field("sinceDealtDamage").SetValue(0f);
        }

        private void CheckCards()
        {
            foreach (PacifistType pacifistType in Enum.GetValues(typeof(PacifistType)))
            {
                this.pacifists[pacifistType] = (Extensions.Cards.instance.CountPlayerCardsWithCondition(this.player, this.gun, this.gunAmmo, this.data, this.health, this.gravity, this.block, this.characterStatModifiers, (card, p, g, ga, d, h, gr, b, c) => card.name == this.cardNames[pacifistType]) > 0);
            }
        }
        private bool HasCompleteSet()
        {
            return (this.pacifists[PacifistType.I] && this.pacifists[PacifistType.II] && this.pacifists[PacifistType.III] && this.pacifists[PacifistType.IV]);
        }

        public enum PacifistType
        {
            I, // faster reload
            II, // faster block cooldown
            III, // faster movement speed
            IV, // more damage
            V // Not implemented
        }
        private readonly Dictionary<PacifistType, string> cardNames = new Dictionary<PacifistType, string>()
        {
            {PacifistType.I, "Pacifist I" },
            {PacifistType.II, "Pacifist II" },
            {PacifistType.III, "Pacifist III" },
            {PacifistType.IV, "Pacifist IV" },
            {PacifistType.V, "Pacifist V" },
        };

    }
}

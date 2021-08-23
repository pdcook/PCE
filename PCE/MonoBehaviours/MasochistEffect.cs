﻿using System;
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
    public class MasochistEffect : CounterReversibleEffect
    {
        private Dictionary<MasochistType, bool> survivalists = new Dictionary<MasochistType, bool>();

        private readonly float max_mult = 3f;
        private readonly float defaultTimeToMax = 20f;
        private float timeToMax;

        private readonly Color maxChargeColor = Color.red;
        private ColorFlash colorFlash = null;
        private readonly float colorFlashMin = 0.5f;
        private readonly float colorFlashMax = 3f;
        private readonly float colorFlashThreshMaxFrac = 0.25f;

        private float multiplier;

        // time since last successful block determines the effect multiplier
        public override CounterStatus UpdateCounter()
        {

            float timeSince = Time.time - base.block.GetAdditionalData().timeOfLastSuccessfulBlock;

            this.multiplier = UnityEngine.Mathf.Clamp(((this.max_mult - 1f) / (this.timeToMax)) * timeSince + 1f, 1f, this.max_mult);


            return CounterStatus.Apply;
        }

        public override void UpdateEffects()
        {
            // update which survivalist cards the player has
            this.CheckCards();

            if (this.HasCompleteSet() && !this.survivalists[MasochistType.V])
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, MasochistVCard.self);
                Unbound.Instance.StartCoroutine(ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player.teamID, MasochistVCard.self));
                Unbound.Instance.ExecuteAfterSeconds(2f, () => this.gameObject.GetOrAddComponent<MasochistColorEffect>());
            }

            if (!this.survivalists[MasochistType.V])
            {
                this.timeToMax = this.defaultTimeToMax / (float)base.numEnemyPlayers;
            }
            else
            {
                this.timeToMax = this.defaultTimeToMax / (2f* (float)base.numEnemyPlayers);
            }

            foreach (MasochistType survivalistType in Enum.GetValues(typeof(MasochistType)))
            {
                if (this.survivalists[survivalistType])
                {
                    switch (survivalistType)
                    {
                        case MasochistType.I:
                            this.gunAmmoStatModifier.reloadTimeMultiplier_mult = 1f / this.multiplier;
                            break;
                        case MasochistType.II:
                            this.blockModifier.cdMultiplier_mult = 1f / this.multiplier;
                            break;
                        case MasochistType.III:
                            base.characterStatModifiersModifier.movementSpeed_mult = (this.multiplier - 1f) / 2f + 1f; // max movement speed mult is actually 2x
                            break;
                        case MasochistType.IV:
                            base.gunStatModifier.bulletDamageMultiplier_mult = this.multiplier;
                            break;
                        case MasochistType.V:
                            base.gun.projectileColor = this.maxChargeColor;
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
            base.block.GetAdditionalData().timeOfLastSuccessfulBlock = Time.time;
        }

        private void CheckCards()
        {
            foreach (MasochistType survivalistType in Enum.GetValues(typeof(MasochistType)))
            {
                this.survivalists[survivalistType] = (ModdingUtils.Utils.Cards.instance.CountPlayerCardsWithCondition(this.player, this.gun, this.gunAmmo, this.data, this.health, this.gravity, this.block, this.characterStatModifiers, (card, p, g, ga, d, h, gr, b, c) => card.name == this.cardNames[survivalistType]) > 0);
            }
        }
        private bool HasCompleteSet()
        {
            return (this.survivalists[MasochistType.I] && this.survivalists[MasochistType.II] && this.survivalists[MasochistType.III] && this.survivalists[MasochistType.IV]);
        }

        public enum MasochistType
        {
            I, // faster reload
            II, // faster block cooldown
            III, // faster movement speed
            IV, // more damage
            V // Not implemented
        }
        private readonly Dictionary<MasochistType, string> cardNames = new Dictionary<MasochistType, string>()
        {
            {MasochistType.I, "Masochist I" },
            {MasochistType.II, "Masochist II" },
            {MasochistType.III, "Masochist III" },
            {MasochistType.IV, "Masochist IV" },
            {MasochistType.V, "Masochist V" },
        };

    }
    public class MasochistColorEffect : MonoBehaviour
    {
        private Player player;
        internal List<int> indeces = new List<int>() { };
        private Color color = Color.blue;
        void Start()
        {
            Color.RGBToHSV(this.color, out float h, out float s, out float v);

            this.player = this.gameObject.GetComponent<Player>();
            GameObject[] cardSquareObjs = ModdingUtils.Utils.CardBarUtils.instance.GetCardBarSquares(this.player);
            List<UnityEngine.UI.ProceduralImage.ProceduralImage> cardSquares = new List<UnityEngine.UI.ProceduralImage.ProceduralImage>() { };
            foreach (GameObject obj in cardSquareObjs)
            {
                cardSquares.Add(obj.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.ProceduralImage.ProceduralImage>());
            }

            foreach (UnityEngine.UI.ProceduralImage.ProceduralImage cardSquare in cardSquares)
            {
                Color.RGBToHSV(cardSquare.color, out float h_, out float s_, out float v_);
                Color newColor = Color.HSVToRGB(h, s_, v_);
                newColor.a = cardSquare.color.a;

                cardSquare.color = newColor;
            }
        }
    }
}
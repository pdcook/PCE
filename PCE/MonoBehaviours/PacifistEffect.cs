using System;
using System.Collections.Generic;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using ModdingUtils.Extensions;
using ModdingUtils.MonoBehaviours;
using PCE.Cards;

namespace PCE.MonoBehaviours
{
    public class PacifistEffect : CounterReversibleEffect
    {
        private Dictionary<PacifistType, bool> pacifists = new Dictionary<PacifistType, bool>();

        private readonly float max_mult = 3f;
        private readonly float defaultTimeToMax = 20f;
        private float timeToMax;

        private readonly Color maxChargeColor = Color.green;
        private ColorFlash colorFlash = null;
        private readonly float colorFlashMin = 0.5f;
        private readonly float colorFlashMax = 3f;
        private readonly float colorFlashThreshMaxFrac = 0.25f;

        private float multiplier;
        private bool V = false;

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

            if (this.HasCompleteSet() && !this.pacifists[PacifistType.V] && !this.V)
            {
                this.V = true;
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, PacifistVCard.self);
                Unbound.Instance.StartCoroutine(ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player.teamID, PacifistVCard.self));
                Unbound.Instance.ExecuteAfterSeconds(2f, () => this.gameObject.GetOrAddComponent<PacifistColorEffect>());
            }
            else if (!this.HasCompleteSet() && this.pacifists[PacifistType.V] && this.V)
            {
                this.V = false;
                ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(player, PacifistVCard.self);
                Unbound.Instance.ExecuteAfterSeconds(2f, () => UnityEngine.GameObject.Destroy(this.gameObject.GetOrAddComponent<PacifistColorEffect>()));
            }

            if (!this.pacifists[PacifistType.V])
            {
                this.timeToMax = this.defaultTimeToMax / (float)base.numEnemyPlayers;
            }
            else
            {
                this.timeToMax = this.defaultTimeToMax / (2f*(float)base.numEnemyPlayers);
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
                this.pacifists[pacifistType] = (ModdingUtils.Utils.Cards.instance.CountPlayerCardsWithCondition(this.player, this.gun, this.gunAmmo, this.data, this.health, this.gravity, this.block, this.characterStatModifiers, (card, p, g, ga, d, h, gr, b, c) => card.name == this.cardNames[pacifistType]) > 0);
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
    public class PacifistColorEffect : MonoBehaviour
    {
        private Player player;
        internal List<int> indeces = new List<int>() { };
        private Color color = Color.green;
        private Color? originalColor = null;
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
            try
            {
                originalColor = new Color(cardSquares[0].color.r, cardSquares[0].color.g, cardSquares[0].color.b, cardSquares[0].color.a);
            }
            catch
            { }
            foreach (UnityEngine.UI.ProceduralImage.ProceduralImage cardSquare in cardSquares)
            {
                Color.RGBToHSV(cardSquare.color, out float h_, out float s_, out float v_);
                Color newColor = Color.HSVToRGB(h, s_, v_);
                newColor.a = cardSquare.color.a;

                cardSquare.color = newColor;
            }
        }
        void OnDestroy()
        {
            GameObject[] cardSquareObjs = ModdingUtils.Utils.CardBarUtils.instance.GetCardBarSquares(this.player);
            List<UnityEngine.UI.ProceduralImage.ProceduralImage> cardSquares = new List<UnityEngine.UI.ProceduralImage.ProceduralImage>() { };
            foreach (GameObject obj in cardSquareObjs)
            {
                cardSquares.Add(obj.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.ProceduralImage.ProceduralImage>());
            }
            foreach (UnityEngine.UI.ProceduralImage.ProceduralImage cardSquare in cardSquares)
            {
                if (originalColor != null)
                {
                    cardSquare.color = (Color)originalColor;
                }
            }
        }
    }
}

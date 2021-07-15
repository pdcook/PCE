using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;
using PCE.Extensions;
using Photon.Pun;
using UnboundLib.Cards;
using PCE.Cards;

namespace PCE.MonoBehaviours
{
    public class WildcardEffect : CounterReversibleEffect
    {
        private Dictionary<WildcardType, bool> wildcards = new Dictionary<WildcardType, bool>();

        private readonly System.Random rng = new System.Random();

        private readonly float max_mult = 3f;
        private readonly float[] waitMinMax = new float[] {3f, 10f};
        private readonly float[] durationMinMax = new float[] {3f, 10f}; 

        private readonly Color maxChargeColor = Color.magenta;
        private ColorEffect colorEffect = null;

        private float multiplier = 1f;
        private float referenceTime = Time.time;
        private float timeToWait = 0f;
        private bool wait = true;
        private bool active = false;
        private bool Vexists = false;

        // time since last damage determines the effect multiplier
        public override CounterStatus UpdateCounter()
        {

            this.CheckCards();

            if (this.HasCompleteSet() && !this.wildcards[WildcardType.V] && !this.Vexists)
            {
                // build and add wildcard V card
                CustomCard.BuildCard<WildcardVCard>(cardInfo => cardInfo.GetComponent<PCECustomCard>().AddCardToPlayer(base.player, cardInfo));
                this.Vexists = true;
            }

            if (this.wait == this.active)
            {
                // invalid state, reset and wait
                this.Reset();
                return CounterStatus.Wait;
            }

            else if (this.wait && !this.active)
            {
                if (Time.time >= this.timeToWait + this.referenceTime)
                {
                    this.active = true;
                    this.wait = false;
                    this.GetApplyTime();
                    this.GetMultiplier();
                    this.referenceTime = Time.time;
                    return CounterStatus.Apply;
                }
                else
                {
                    return CounterStatus.Wait;
                }
            }

            else if (!this.wait && this.active)
            {
                if (Time.time >= this.timeToWait + this.referenceTime)
                {
                    this.active = false;
                    this.wait = true;
                    this.GetWaitTime();
                    this.multiplier = 1f;
                    this.referenceTime = Time.time;
                    return CounterStatus.Remove;
                }
                else
                {
                    return CounterStatus.Wait;
                }
            }
            else
            {
                // invalid state, reset and wait
                this.Reset();
                return CounterStatus.Wait;
            }
        }

        private void GetApplyTime()
        {
            float newEffectDuration = (this.durationMinMax[1] - this.durationMinMax[0]) * (float)this.rng.NextDouble() + this.durationMinMax[0];

            if (PhotonNetwork.OfflineMode)
            {
                // offline mode
                this.timeToWait = newEffectDuration;
            }
            else if (base.GetComponent<PhotonView>().IsMine)
            {
                base.GetComponent<PhotonView>().RPC("RPCA_SetTimeToWait", RpcTarget.All, new object[] { newEffectDuration });
            }
        }
        private void GetWaitTime()
        {
            float newWaitDuration = (this.waitMinMax[1] - this.waitMinMax[0]) * (float)this.rng.NextDouble() + this.waitMinMax[0];
            
            this.CheckCards();
            if (this.wildcards[WildcardType.V])
            {
                newWaitDuration /= 2f;
            }

            if (PhotonNetwork.OfflineMode)
            {
                // offline mode
                this.timeToWait = newWaitDuration;
            }
            else if (base.GetComponent<PhotonView>().IsMine)
            {
                base.GetComponent<PhotonView>().RPC("RPCA_SetTimeToWait", RpcTarget.All, new object[] { newWaitDuration });
            }
        }
        private void GetMultiplier()
        {
            float new_mult = (this.max_mult - 1f) * (float)this.rng.NextDouble() + 1f;

            if (PhotonNetwork.OfflineMode)
            {
                // offline mode
                this.multiplier = new_mult;
            }
            else if (base.GetComponent<PhotonView>().IsMine)
            {
                base.GetComponent<PhotonView>().RPC("RPCA_SetMultiplier", RpcTarget.All, new object[] { new_mult });
            }
        }

        public override void UpdateEffects()
        {
            // update which wildcard cards the player has
            this.CheckCards();

            if (this.HasCompleteSet() && !this.wildcards[WildcardType.V] && !this.Vexists)
            {
                // build and hide wildcard V card
                CustomCard.BuildCard<WildcardIVCard>(cardInfo => cardInfo.GetComponent<PCECustomCard>().AddCardToPlayer(base.player, cardInfo));
                this.Vexists = true;
            }

            foreach (WildcardType wildcardType in Enum.GetValues(typeof(WildcardType)))
            {
                if (this.wildcards[wildcardType])
                {
                    switch (wildcardType)
                    {
                        case WildcardType.I:
                            this.gunAmmoStatModifier.reloadTimeMultiplier_mult = 1f / this.multiplier;
                            break;
                        case WildcardType.II:
                            this.blockModifier.cdMultiplier_mult = 1f / this.multiplier;
                            break;
                        case WildcardType.III:
                            base.characterStatModifiersModifier.movementSpeed_mult = (this.multiplier - 1f) / 2f + 1f; // max movement speed mult is actually 2x
                            break;
                        case WildcardType.IV:
                            base.gunStatModifier.bulletDamageMultiplier_mult = this.multiplier;
                            break;
                        case WildcardType.V:
                            base.gunStatModifier.projectileColor = this.maxChargeColor;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public override void OnApply()
        {
            if (this.colorEffect == null)
            {
                this.colorEffect = base.player.gameObject.AddComponent<ColorEffect>();
                this.colorEffect.SetColor(Color.Lerp(GetPlayerColor.GetColorMax(base.player), this.maxChargeColor, this.multiplier / this.max_mult));
            }
        }
        public override void OnRemove()
        {
            if (this.colorEffect != null)
            {
                this.colorEffect.Destroy();
            }
        }
        public override void Reset()
        {
            this.multiplier = 1f;
            this.referenceTime = Time.time;
            this.wait = true;
            this.active = false;
            this.GetWaitTime();
        }

        private void CheckCards()
        {
            foreach (WildcardType wildcardType in Enum.GetValues(typeof(WildcardType)))
            {
                this.wildcards[wildcardType] = (Extensions.Cards.instance.CountPlayerCardsWithCondition(this.player, this.gun, this.gunAmmo, this.data, this.health, this.gravity, this.block, this.characterStatModifiers, (card, p, g, ga, d, h, gr, b, c) => card.name == this.cardNames[wildcardType]) > 0);
            }
        }

        private bool HasCompleteSet()
        {
            return (this.wildcards[WildcardType.I] && this.wildcards[WildcardType.II] && this.wildcards[WildcardType.III] && this.wildcards[WildcardType.IV]);
        }

        public enum WildcardType
        {
            I, // faster reload
            II, // faster block cooldown
            III, // faster movement speed
            IV, // more damage
            V // Not implemented
        }
        private readonly Dictionary<WildcardType, string> cardNames = new Dictionary<WildcardType, string>()
        {
            {WildcardType.I, "Wildcard I" },
            {WildcardType.II, "Wildcard II" },
            {WildcardType.III, "Wildcard III" },
            {WildcardType.IV, "Wildcard IV" },
            {WildcardType.V, "Wildcard V" },
        };

        [PunRPC]
        public void RPCA_SetTimeToWait(float timeToWait)
        {
            this.timeToWait = timeToWait;
        }
        [PunRPC]
        public void RPCA_SetMultiplier(float mult)
        {
            this.multiplier = mult;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnboundLib;
using PCE.Extensions;
using ModdingUtils.Extensions;
using UnityEngine;
using System.Reflection;

namespace PCE.Cards
{
    public class MurderCard : CustomCard
    {
        public static System.Random rng = new System.Random();
        /*
        *  A rare card which kills the opposing player immediately at the start of the next round
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            ModdingUtils.Extensions.CardInfoExtension.GetAdditionalData(cardInfo).canBeReassigned = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

            if (GM_Test.instance != null && GM_Test.instance.gameObject.activeInHierarchy)
            {
                // are we in sandbox mode? if so, just kill the other player
                Player oppPlayer = PlayerManager.instance.GetOtherPlayer(player);
                Unbound.Instance.ExecuteAfterSeconds(2f, delegate
                {
                    typeof(HealthHandler).InvokeMember("RPCA_Die",
                                BindingFlags.Instance | BindingFlags.InvokeMethod |
                                BindingFlags.NonPublic, null, oppPlayer.data.healthHandler,
                                new object[] { new Vector2(0, 1) });

                });
            }
            else
            {
                // otherwise, let the onbattlestart hook handle it
                characterStats.GetAdditionalData().murder += 1;
            }

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Murder";
        }
        protected override string GetDescription()
        {
            return "Kill your opponent";
        }

        protected override GameObject GetCardArt()
        {
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Murder");
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override CardInfoStat[] GetStats()
        {
            return null;
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.EvilPurple;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

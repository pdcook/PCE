using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;
using PCE;
using UnboundLib;
using PCE.Extensions;
using PCE.RoundsEffects;
using PCE.MonoBehaviours;


namespace PCE.Cards
{
    public class MulliganCard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth *= 0.85f;

            player.gameObject.GetOrAddComponent<MulliganEffect>();

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Mulligan";
        }
        protected override string GetDescription()
        {
            return "Always survive your first fatal blow.";
        }

        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                positive = false,
                stat = "HP",
                amount = "-15%",
                simepleAmount = CardInfoStat.SimpleAmount.slightlyLower
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

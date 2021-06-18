using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;

namespace PCE.Cards
{
    public class OldJetpackCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.jumps += 10;
            characterStats.jump = 0.5f;
            characterStats.movementSpeed *= 0.75f;
            data.maxHealth *= 0.75f;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Old Jetpack";
        }
        protected override string GetDescription()
        {
            return "Sputter around for a bit by continually firing this old jetpack";
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
                positive = true,
                stat = "Old Jetpack",
                amount = "An",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Jetpack Fuel",
                amount = "+10",
                simepleAmount = CardInfoStat.SimpleAmount.Some
                },
                new CardInfoStat
                {
                positive = false,
                stat = "HP",
                amount = "-25%",
                simepleAmount = CardInfoStat.SimpleAmount.lower
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Movement Speed",
                amount = "-25%",
                simepleAmount = CardInfoStat.SimpleAmount.lower
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnboundLib;
using UnityEngine;

namespace PCE.Cards
{
    public class StraightShotCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.gravity = 0f;
            gun.projectileSpeed *= 0.85f;
            gunAmmo.reloadTimeMultiplier = 0.7f;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Straight Shot";
        }
        protected override string GetDescription()
        {
            return "Bullets follow a straight line";
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
                stat = "Bullet Speed",
                amount = "-15%",
                simepleAmount = CardInfoStat.SimpleAmount.slightlyLower
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Reload Speed",
                amount = "+30%",
                simepleAmount = CardInfoStat.SimpleAmount.Some
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.FirepowerYellow;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using PCE.MonoBehaviours;

namespace PCE.Cards
{
    public class SuperJumpCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth *= 1.35f;

            SuperJumpEffect effect = player.gameObject.GetOrAddComponent<SuperJumpEffect>();

            effect.AddToTimeMultiplier(1f);

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Super Jump";
        }
        protected override string GetDescription()
        {
            return "Crouch to charge a super jump";
        }

        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                positive = true,
                stat = "HP",
                amount = "+35%",
                simepleAmount = CardInfoStat.SimpleAmount.Some
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

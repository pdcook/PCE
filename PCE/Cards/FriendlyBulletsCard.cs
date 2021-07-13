using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using PCE.MonoBehaviours;
using PCE.RoundsEffects;

namespace PCE.Cards
{
    public class FriendlyBulletsCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            FriendlyBulletsDealtDamageEffect effect = player.gameObject.GetOrAddComponent<FriendlyBulletsDealtDamageEffect>();

            effect.multiplier /= 2f;

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Friendly Bullets";
        }
        protected override string GetDescription()
        {
            return "Reduced friendly fire damage. Be careful: effects still apply.";
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
                stat = "Friendly Fire",
                amount = "-50%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.NatureBrown;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

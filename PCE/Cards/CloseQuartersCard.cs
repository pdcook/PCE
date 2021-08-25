using UnboundLib.Cards;
using PCE.Extensions;
using UnityEngine;

namespace PCE.Cards
{
    public class CloseQuartersCard : CustomCard
    {
        /*
        *  More damage, but damage falloff is significantly increased
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damageAfterDistanceMultiplier = 0.1f;
            gun.GetAdditionalData().minDistanceMultiplier = 0f;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Close Quarters";
        }
        protected override string GetDescription()
        {
            return "Do significantly more damage up close";
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
                    stat = "Damage",
                    amount = "+100%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                    },
                    new CardInfoStat
                    {
                    positive = false,
                    stat = "Damage Falloff",
                    amount = "+100%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLotOf
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

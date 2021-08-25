using System;
using ModdingUtils.Extensions;
using ModdingUtils.RoundsEffects;
using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;


namespace PCE.Cards
{
    public class FlipCard : CustomCard
    {
        /*
        *  Bullets temporarily invert victim's gravity
        */

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

            if (characterStats.GetAdditionalData().gravityMultiplierOnDoDamage > 0)
            {
                characterStats.GetAdditionalData().gravityMultiplierOnDoDamage = -0.25f * Math.Abs(characterStats.GetAdditionalData().gravityMultiplierOnDoDamage);
            }
            characterStats.GetAdditionalData().gravityDurationOnDoDamage = UnityEngine.Mathf.Clamp(characterStats.GetAdditionalData().gravityDurationOnDoDamage + 1.5f, 0f, 4f);

            player.gameObject.GetOrAddComponent<GravityDealtDamageEffect>();
            

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Flip";
        }
        protected override string GetDescription()
        {
            return "Bullets temporarily flip victim's direction of gravity";
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
                stat = "Bullets",
                amount = "Antigravity",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

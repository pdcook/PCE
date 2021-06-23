using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using UnboundLib.Cards;
using PCE.RoundsEffects;
using PCE.Extensions;

namespace PCE.Cards
{
    public class GroundedCard : CustomCard
    {
        /*
         *  Bullets temporarily increase victim's gravity
         */

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.GetAdditionalData().gravityMultiplierOnDoDamage *= 3f;
            if (characterStats.GetAdditionalData().gravityDurationOnDoDamage < 4f)
            {
                characterStats.GetAdditionalData().gravityDurationOnDoDamage += 2f;
            }

            player.gameObject.AddComponent<GravityDealtDamageEffect>();


        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Grounded";
        }
        protected override string GetDescription()
        {
            return "Bullets temporarily increase victim's gravity";
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
                amount = "Grounding",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
    }
}

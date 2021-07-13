using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnboundLib;
using UnityEngine;
using PCE.MonoBehaviours;

namespace PCE.Cards
{
    public class RetreatCard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            HealthBasedEffect effect = player.gameObject.AddComponent<HealthBasedEffect>();
            effect.blockModifier.additionalBlocks_add = 1;
            effect.blockModifier.cdMultiplier_mult = 0.5f;
            effect.characterStatModifiersModifier.movementSpeed_mult = 1.5f;
            effect.SetPercThresholdMax(0.2f);
            effect.SetColor(Color.blue);
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Retreat";
        }
        protected override string GetDescription()
        {
            return "Get boosted defense stats when below 20% of your max HP.\nWhen active:";
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
                stat = "Additional Blocks",
                amount = "+1",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Block Cooldown",
                amount = "-50%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Movement Speed",
                amount = "+50%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                },

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

using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnboundLib;
using UnityEngine;
using PCE.MonoBehaviours;

namespace PCE.Cards
{
    public class LastStandCard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            HealthBasedEffect effect = player.gameObject.AddComponent<HealthBasedEffect>();
            effect.gunStatModifier.attackSpeedMultiplier_mult = 0.75f;
            effect.gunAmmoStatModifier.reloadTimeMultiplier_mult = 0.5f;
            effect.gunStatModifier.projectileSpeed_mult = 1.5f;
            effect.SetPercThresholdMax(0.5f);
            effect.SetColor(Color.red);
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Last Stand";
        }
        protected override string GetDescription()
        {
            return "Get boosted attack stats when below 50% of your max HP.\nWhen active:";
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
                stat = "Attack Speed",
                amount = "+50%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Reload Speed",
                amount = "+100%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Bullet Speed",
                amount = "+50%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using PCE.MonoBehaviours;
using PCE.Extensions;
using System.Linq;
using UnboundLib;

namespace PCE.Cards
{
    public class PacPlayerCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.GetAdditionalData().wraps += 2;
            data.maxHealth *= 1.35f;

            characterStats.GetAdditionalData().remainingWraps = characterStats.GetAdditionalData().wraps;

            player.gameObject.GetOrAddComponent<PacPlayerEffect>();

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Pac-Player";
        }
        protected override string GetDescription()
        {
            return "<b>You</b> wrap around from the edge of the screen.";
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
                    stat = "Warp",
                    amount = "+2",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "HP",
                    amount = "+35%",
                    simepleAmount = CardInfoStat.SimpleAmount.Some
                }

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

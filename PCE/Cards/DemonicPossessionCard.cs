using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnboundLib;
using UnityEngine;
using PCE.MonoBehaviours;

namespace PCE.Cards
{
    public class DemonicPossessionCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<DemonicPossessionEffect>();
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Demonic Possession";
        }
        protected override string GetDescription()
        {
            return "Become a being of pure chaos.";
        }

        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override CardInfoStat[] GetStats()
        {
            return null;
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.EvilPurple;
        }
    }
}

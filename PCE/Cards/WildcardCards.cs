using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;
using PCE;
using UnboundLib;
using PCE.Extensions;
using PCE.RoundsEffects;
using PCE.MonoBehaviours;
using System.Reflection;
using HarmonyLib;
using System.Linq;


namespace PCE.Cards
{
    public class WildcardCategories
    {
        // singleton design, so that the categories are only created once
        public static readonly WildcardCategories instance = new WildcardCategories();

        public CardCategory[] categories;
        public CardCategory[] blacklistedCategories
        {
            get { return (SurvivalistCategories.instance.categories.Concat(PacifistCategories.instance.categories)).ToArray(); }
            set { }
        }
        private WildcardCategories()
        {
            WildcardCategories instance = this;

            CardCategory category = ScriptableObject.CreateInstance<CardCategory>();
            category.name = "Wildcard";
            this.categories = new CardCategory[] { category };
        }
    }
    public class WildcardICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = WildcardCategories.instance.categories;
            cardInfo.blacklistedCategories = WildcardCategories.instance.blacklistedCategories;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<WildcardEffect>();

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Wildcard I";
        }
        protected override string GetDescription()
        {
            return "Randomly increased reload speed at random intervals.";
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
                stat = "Reload Speed",
                amount = "Up to 3×",
                simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
    }
    public class WildcardIICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = WildcardCategories.instance.categories;
            cardInfo.blacklistedCategories = WildcardCategories.instance.blacklistedCategories;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<WildcardEffect>();

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Wildcard II";
        }
        protected override string GetDescription()
        {
            return "Randomly decreased block cooldown at random intervals.";
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
                stat = "Block Cooldown",
                amount = "Up to 1/3×",
                simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
    }
    public class WildcardIIICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = WildcardCategories.instance.categories;
            cardInfo.blacklistedCategories = WildcardCategories.instance.blacklistedCategories;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<WildcardEffect>();

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Wildcard III";
        }
        protected override string GetDescription()
        {
            return "Randomly increased movement speed at random intervals.";
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
                stat = "Movement Speed",
                amount = "Up to 2×",
                simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
    }
    public class WildcardIVCard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = WildcardCategories.instance.categories;
            cardInfo.blacklistedCategories = WildcardCategories.instance.blacklistedCategories;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<WildcardEffect>();

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Wildcard IV";
        }
        protected override string GetDescription()
        {
            return "Randomly increased damage at random intervals.";
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
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                positive = true,
                stat = "Damage",
                amount = "Up to 3×",
                simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
    }
    public class WildcardVCard : PCECustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = WildcardCategories.instance.categories;
            cardInfo.blacklistedCategories = WildcardCategories.instance.blacklistedCategories;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<WildcardEffect>();
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Wildcard V";
        }
        protected override string GetDescription()
        {
            return "Double the frequency of all Wildcard effects.";
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
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
        public override bool GetEnabled()
        {
            return false;
        }
    }
}

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
    public class PacifistCategories
    {
        // singleton design, so that the categories are only created once
        public static readonly PacifistCategories instance = new PacifistCategories();

        public CardCategory[] categories;
        public CardCategory[] blacklistedCategories
        {
            get { return (SurvivalistCategories.instance.categories.Concat(WildcardCategories.instance.categories)).ToArray(); }
            set { }
        }
        private PacifistCategories()
        {
            PacifistCategories instance = this;

            CardCategory category = ScriptableObject.CreateInstance<CardCategory>();
            category.name = "Pacifist";
            this.categories = new CardCategory[] { category };
        }
    }
    public class PacifistICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = PacifistCategories.instance.categories;
            cardInfo.blacklistedCategories = PacifistCategories.instance.blacklistedCategories;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(characterStats).Field("sinceDealtDamage").SetValue(0f);
            player.gameObject.GetOrAddComponent<PacifistEffect>();

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Pacifist I";
        }
        protected override string GetDescription()
        {
            return "Increased reload speed the longer you go without dealing damage.";
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
            return CardThemeColor.CardThemeColorType.NatureBrown;
        }
    }
    public class PacifistIICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = PacifistCategories.instance.categories;
            cardInfo.blacklistedCategories = PacifistCategories.instance.blacklistedCategories;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(characterStats).Field("sinceDealtDamage").SetValue(0f);
            player.gameObject.GetOrAddComponent<PacifistEffect>();

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Pacifist II";
        }
        protected override string GetDescription()
        {
            return "Decreased block cooldown the longer you go without dealing damage.";
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
            return CardThemeColor.CardThemeColorType.NatureBrown;
        }
    }
    public class PacifistIIICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = PacifistCategories.instance.categories;
            cardInfo.blacklistedCategories = PacifistCategories.instance.blacklistedCategories;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(characterStats).Field("sinceDealtDamage").SetValue(0f);
            player.gameObject.GetOrAddComponent<PacifistEffect>();

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Pacifist III";
        }
        protected override string GetDescription()
        {
            return "Increased movement speed the longer you go without dealing damage.";
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
            return CardThemeColor.CardThemeColorType.NatureBrown;
        }
    }
    public class PacifistIVCard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = PacifistCategories.instance.categories;
            cardInfo.blacklistedCategories = PacifistCategories.instance.blacklistedCategories;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(characterStats).Field("sinceDealtDamage").SetValue(0f);
            player.gameObject.GetOrAddComponent<PacifistEffect>();

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Pacifist IV";
        }
        protected override string GetDescription()
        {
            return "Increased damage the longer you go without dealing damage.";
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
            return CardThemeColor.CardThemeColorType.NatureBrown;
        }
    }
    public class PacifistVCard : PCECustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = PacifistCategories.instance.categories;
            cardInfo.blacklistedCategories = PacifistCategories.instance.blacklistedCategories;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(characterStats).Field("sinceDealtDamage").SetValue(0f);
            player.gameObject.GetOrAddComponent<PacifistEffect>();
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Pacifist V";
        }
        protected override string GetDescription()
        {
            return "Double the charge speed of all Pacifist effects.";
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
            return CardThemeColor.CardThemeColorType.NatureBrown;
        }
        public override bool GetEnabled()
        {
            return false;
        }
    }
}

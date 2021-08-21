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
using CardChoiceSpawnUniqueCardPatch.CustomCategories;


namespace PCE.Cards
{
    public class SurvivalistICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Survivalist") };
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Masochist"), CustomCardCategories.instance.CardCategory("Pacifist"), CustomCardCategories.instance.CardCategory("Wildcard") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(health).Field("lastDamaged").SetValue(Time.time);
            player.gameObject.GetOrAddComponent<SurvivalistEffect>();

            SurvivalistBlacklistCard.DenyOthers(player);
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Survivalist I";
        }
        protected override string GetDescription()
        {
            return "Increased reload speed the longer you go without taking damage.";
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
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
    public class SurvivalistIICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Survivalist") };
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Masochist"), CustomCardCategories.instance.CardCategory("Pacifist"), CustomCardCategories.instance.CardCategory("Wildcard") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(health).Field("lastDamaged").SetValue(Time.time);
            player.gameObject.GetOrAddComponent<SurvivalistEffect>();
            SurvivalistBlacklistCard.DenyOthers(player);
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Survivalist II";
        }
        protected override string GetDescription()
        {
            return "Decreased block cooldown the longer you go without taking damage.";
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
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
    public class SurvivalistIIICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Survivalist") };
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Masochist"), CustomCardCategories.instance.CardCategory("Pacifist"), CustomCardCategories.instance.CardCategory("Wildcard") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(health).Field("lastDamaged").SetValue(Time.time);
            player.gameObject.GetOrAddComponent<SurvivalistEffect>();
            SurvivalistBlacklistCard.DenyOthers(player);

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Survivalist III";
        }
        protected override string GetDescription()
        {
            return "Increased movement speed the longer you go without taking damage.";
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
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
    public class SurvivalistIVCard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Survivalist") };
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Masochist"), CustomCardCategories.instance.CardCategory("Pacifist"), CustomCardCategories.instance.CardCategory("Wildcard") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(health).Field("lastDamaged").SetValue(Time.time);
            player.gameObject.GetOrAddComponent<SurvivalistEffect>();
            SurvivalistBlacklistCard.DenyOthers(player);

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Survivalist IV";
        }
        protected override string GetDescription()
        {
            return "Increased damage the longer you go without taking damage.";
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
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
    public class SurvivalistVCard : CustomCard
    {
        internal static CardInfo self = null;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Survivalist") };
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Masochist"), CustomCardCategories.instance.CardCategory("Pacifist"), CustomCardCategories.instance.CardCategory("Wildcard") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(health).Field("lastDamaged").SetValue(Time.time);
            player.gameObject.GetOrAddComponent<SurvivalistEffect>();
            SurvivalistBlacklistCard.DenyOthers(player);

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Survivalist V";
        }
        protected override string GetDescription()
        {
            return "Double the charge speed of all Survivalist effects.";
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
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }
        public override bool GetEnabled()
        {
            return false;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
    public class SurvivalistBlacklistCard : CustomCard
    {
        internal static CardInfo self = null;

        internal static void DenyOthers(Player player)
        {
            foreach (Player otherPlayer in PlayerStatus.GetOtherPlayers(player).Where(other_player => ModdingUtils.Utils.Cards.instance.PlayerIsAllowedCard(other_player, SurvivalistBlacklistCard.self)))
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(otherPlayer, SurvivalistBlacklistCard.self);
            }
        }
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Survivalist") };
            ModdingUtils.Extensions.CardInfoExtension.GetAdditionalData(cardInfo).isVisible = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "NoSurvivalist";
        }
        protected override string GetDescription()
        {
            return "";
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
            return null;
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }
        public override bool GetEnabled()
        {
            return false;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

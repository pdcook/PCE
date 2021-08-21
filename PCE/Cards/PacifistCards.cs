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
    public class PacifistICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Pacifist") };
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Survivalist"), CustomCardCategories.instance.CardCategory("Wildcard") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(characterStats).Field("sinceDealtDamage").SetValue(0f);
            player.gameObject.GetOrAddComponent<PacifistEffect>();
            PacifistBlacklistCard.DenyOthers(player);

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
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Pacifist_I");
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
        public override string GetModName()
        {
            return "PCE";
        }
    }
    public class PacifistIICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Pacifist") };
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Survivalist"), CustomCardCategories.instance.CardCategory("Wildcard") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(characterStats).Field("sinceDealtDamage").SetValue(0f);
            player.gameObject.GetOrAddComponent<PacifistEffect>();
            PacifistBlacklistCard.DenyOthers(player);

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
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Pacifist_II");
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
        public override string GetModName()
        {
            return "PCE";
        }
    }
    public class PacifistIIICard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Pacifist") };
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Survivalist"), CustomCardCategories.instance.CardCategory("Wildcard") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(characterStats).Field("sinceDealtDamage").SetValue(0f);
            player.gameObject.GetOrAddComponent<PacifistEffect>();
            PacifistBlacklistCard.DenyOthers(player);

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
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Pacifist_III");
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
        public override string GetModName()
        {
            return "PCE";
        }
    }
    public class PacifistIVCard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Pacifist") };
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Survivalist"), CustomCardCategories.instance.CardCategory("Wildcard") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(characterStats).Field("sinceDealtDamage").SetValue(0f);
            player.gameObject.GetOrAddComponent<PacifistEffect>();
            PacifistBlacklistCard.DenyOthers(player);

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
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Pacifist_IV");
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
        public override string GetModName()
        {
            return "PCE";
        }
    }
    public class PacifistVCard : CustomCard
    {

        internal static CardInfo self = null;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Pacifist") };
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Survivalist"), CustomCardCategories.instance.CardCategory("Wildcard") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(characterStats).Field("sinceDealtDamage").SetValue(0f);
            player.gameObject.GetOrAddComponent<PacifistEffect>();
            PacifistBlacklistCard.DenyOthers(player);
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
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Pacifist_V");
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
        public override string GetModName()
        {
            return "PCE";
        }
    }
    public class PacifistBlacklistCard : CustomCard
    {
        internal static CardInfo self = null;

        internal static void DenyOthers(Player player)
        {
            foreach (Player otherPlayer in PlayerStatus.GetOtherPlayers(player).Where(other_player => ModdingUtils.Utils.Cards.instance.PlayerIsAllowedCard(other_player, PacifistBlacklistCard.self)))
            {
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(otherPlayer, PacifistBlacklistCard.self);
            }
        }
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("Pacifist") };
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
            return "NoPacifist";
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
            return CardThemeColor.CardThemeColorType.NatureBrown;
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

using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using PCE.Extensions;
using PCE.MonoBehaviours;
using HarmonyLib;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;


namespace PCE.Cards
{
    public abstract class SurvivalistCardBase : CustomCard
    {
        internal static CardCategory category = CustomCardCategories.instance.CardCategory("Survivalist");

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { SurvivalistCardBase.category };
            cardInfo.blacklistedCategories = new CardCategory[] { MasochistCardBase.category, PacifistCardBase.category, WildcardCardBase.category };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Traverse.Create(health).Field("lastDamaged").SetValue(Time.time);
            player.gameObject.GetOrAddComponent<SurvivalistEffect>();

            foreach (Player otherPlayer in PlayerStatus.GetOtherPlayers(player))
            {
                if (!ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(otherPlayer.data.stats).blacklistedCategories.Contains(SurvivalistCardBase.category))
                {
                    ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(otherPlayer.data.stats).blacklistedCategories.Add(SurvivalistCardBase.category);
                }
            }    
        }
        public override void OnRemoveCard()
        {
            foreach (Player player in PlayerManager.instance.players)
            {
                ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).blacklistedCategories.RemoveAll(cardcat => cardcat == SurvivalistCardBase.category);
            }
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

    public class SurvivalistICard : SurvivalistCardBase
    {
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
    }
    public class SurvivalistIICard : SurvivalistCardBase
    {
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
    }
    public class SurvivalistIIICard : SurvivalistCardBase
    {
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
    }
    public class SurvivalistIVCard : SurvivalistCardBase
    {
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
    }
    public class SurvivalistVCard : SurvivalistCardBase
    {
        internal static CardInfo self = null;

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
        public override bool GetEnabled()
        {
            return false;
        }
    }
}

using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using PCE.Extensions;
using PCE.MonoBehaviours;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using PlayerStatus = PCE.Extensions.PlayerStatus;


namespace PCE.Cards
{
    public abstract class MasochistCardBase : CustomCard
    {
        internal static CardCategory category = CustomCardCategories.instance.CardCategory("Masochist");

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { MasochistCardBase.category };
            cardInfo.blacklistedCategories = new CardCategory[] { PacifistCardBase.category, SurvivalistCardBase.category, WildcardCardBase.category };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.GetAdditionalData().timeOfLastSuccessfulBlock = Time.time;
            player.gameObject.GetOrAddComponent<MasochistEffect>();
            foreach (Player otherPlayer in PlayerStatus.GetOtherPlayers(player))
            {
                if (!ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(otherPlayer.data.stats).blacklistedCategories.Contains(MasochistCardBase.category))
                {
                    ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(otherPlayer.data.stats).blacklistedCategories.Add(MasochistCardBase.category);
                }
            }
        }
        public override void OnRemoveCard()
        {
            foreach (Player player in PlayerManager.instance.players)
            {
                ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).blacklistedCategories.RemoveAll(cardcat => cardcat == MasochistCardBase.category);
            }
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
    public class MasochistICard : MasochistCardBase
    {
        protected override string GetTitle()
        {
            return "Masochist I";
        }
        protected override string GetDescription()
        {
            return "Increased reload speed the longer you go without blocking damage.";
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
    public class MasochistIICard : MasochistCardBase
    {

        protected override string GetTitle()
        {
            return "Masochist II";
        }
        protected override string GetDescription()
        {
            return "Decreased block cooldown the longer you go without blocking damage.";
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
    public class MasochistIIICard : MasochistCardBase
    {

        protected override string GetTitle()
        {
            return "Masochist III";
        }
        protected override string GetDescription()
        {
            return "Increased movement speed the longer you go without blocking damage.";
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
    public class MasochistIVCard : MasochistCardBase
    {
        protected override string GetTitle()
        {
            return "Masochist IV";
        }
        protected override string GetDescription()
        {
            return "Increased damage the longer you go without blocking damage.";
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
    public class MasochistVCard : MasochistCardBase
    {
        internal static CardInfo self = null;

        protected override string GetTitle()
        {
            return "Masochist V";
        }
        protected override string GetDescription()
        {
            return "Double the charge speed of all Masochist effects.";
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

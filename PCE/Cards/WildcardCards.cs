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
using UnboundLib.Networking;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using Photon.Pun;
using PCE.Utils;


namespace PCE.Cards
{
    public abstract class WildcardCardBase : CustomCard
    {
        internal static CardCategory category = CustomCardCategories.instance.CardCategory("Wildcard");

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            cardInfo.categories = new CardCategory[] { WildcardCardBase.category };
            cardInfo.blacklistedCategories = new CardCategory[] { PacifistCardBase.category, SurvivalistCardBase.category, MasochistCardBase.category };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<WildcardEffect>();
            foreach (Player otherPlayer in PlayerStatus.GetOtherPlayers(player))
            {
                if (!ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(otherPlayer.data.stats).blacklistedCategories.Contains(WildcardCardBase.category))
                {
                    ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(otherPlayer.data.stats).blacklistedCategories.Add(WildcardCardBase.category);
                }
            }
        }
        public override void OnRemoveCard()
        {
            foreach (Player player in PlayerManager.instance.players)
            {
                ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).blacklistedCategories.RemoveAll(cardcat => cardcat == WildcardCardBase.category);
            }
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
    public class WildcardICard : WildcardCardBase
    {

        
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
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Wildcard_I");
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
    public class WildcardIICard : WildcardCardBase
    {

        

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
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Wildcard_II");
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
    public class WildcardIIICard : WildcardCardBase
    {

        
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
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Wildcard_III");
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
    public class WildcardIVCard : WildcardCardBase
    {

        

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
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Wildcard_IV");
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
    public class WildcardVCard : WildcardCardBase
    {
        internal static CardInfo self = null;

        
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
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Wildcard_V");
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

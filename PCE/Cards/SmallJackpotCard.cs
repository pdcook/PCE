using UnityEngine;
using UnboundLib.Cards;
using ModdingUtils.Extensions;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnboundLib.Utils;
using System.Reflection;

namespace PCE.Cards
{
    public class SmallJackpotCard : CustomCard
    {
        /*
         *  A Common card which gives the player a random Uncommon card
         */

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.GetAdditionalData().canBeReassigned = false;
            cardInfo.categories = new CardCategory[] { CardChoiceSpawnUniqueCardPatch.CustomCategories.CustomCardCategories.instance.CardCategory("CardManipulation") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

            CardInfo randomCard = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, this.condition);
            if (randomCard == null)
            {
                // if there is no valid card, then try drawing from the list of all cards (inactive + active) but still make sure it is compatible
                CardInfo[] allCards = ((ObservableCollection<CardInfo>)typeof(CardManager).GetField("activeCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).ToList().Concat((List<CardInfo>)typeof(CardManager).GetField("inactiveCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).ToArray();
                randomCard = ModdingUtils.Utils.Cards.instance.DrawRandomCardWithCondition(allCards, player, null, null, null, null, null, null, null, this.condition);
            }
            //ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, false, "", 2f);
            ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, addToCardBar: true);
            ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Small Jackpot";
        }
        protected override string GetDescription()
        {
            return "Get a random <color=#00ffffff>Uncommon</color> card";
        }

        protected override GameObject GetCardArt()
        {
            return PCE.ArtAssets.LoadAsset<GameObject>("C_SmallJackpot");
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
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
        public bool condition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // do not allow duplicates of cards with allowMultiple == false (handled by moddingutils)
            // card rarity must be as desired
            // card cannot be another cardmanipulation card
            // card cannot be from a blacklisted catagory of any other card (handled by moddingutils)

            return (card.rarity == CardInfo.Rarity.Uncommon) && !card.categories.Contains(CardChoiceSpawnUniqueCardPatch.CustomCategories.CustomCardCategories.instance.CardCategory("CardManipulation"));

        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

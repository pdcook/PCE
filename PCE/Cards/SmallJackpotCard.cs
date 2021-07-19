using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using UnboundLib.Cards;
using Photon.Pun;
using HarmonyLib;
using System.Reflection;
using System.Linq;
using PCE.Extensions;

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

            CardInfo randomCard = Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, this.condition);

            //Utils.Cards.instance.AddCardToPlayer(player, randomCard, false, "", 2f);
            Utils.Cards.instance.AddCardToPlayer(player, randomCard);
            Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
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
            // do not allow duplicates of cards with allowMultiple == false
            // card rarity must be as desired
            // card cannot be another Gamble / Jackpot card
            // card cannot be from a blacklisted catagory of any other card

            return (card.rarity == CardInfo.Rarity.Uncommon) && !card.cardName.Contains("Jackpot") && !card.cardName.Contains("Gamble");

        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

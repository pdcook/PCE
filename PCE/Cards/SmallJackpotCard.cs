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
    public class SmallJackpotCard : PCECustomCard
    {
        /*
         *  A Common card which gives the player a random Uncommon card
         */

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

            CardInfo randomCard = Extensions.Cards.instance.GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, this.condition);

            base.AddCardToPlayer(player, randomCard);
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

            bool blacklisted = false;

            foreach (CardInfo currentCard in player.data.currentCards)
            {
                if (card.categories.Intersect(currentCard.blacklistedCategories).Any())
                {
                    blacklisted = true;
                }
            }

            return !blacklisted && (card.allowMultiple || !player.data.currentCards.Where(cardinfo => cardinfo.name == card.name).Any()) && (card.rarity == CardInfo.Rarity.Uncommon) && !card.cardName.Contains("Jackpot") && !card.cardName.Contains("Gamble");

        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

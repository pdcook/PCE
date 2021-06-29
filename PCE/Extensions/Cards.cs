using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnboundLib;
using Photon.Pun;
using UnboundLib.Cards;
using System.Reflection;
using HarmonyLib;

namespace PCE.Extensions
{
    public sealed class Cards
    {
        // singleton design, so that the RNG isn't reset each call
        public static readonly Cards instance = new Cards();
        private static readonly System.Random rng = new System.Random();
        private Cards()
        {
            Cards instance = this;
        }
        public bool CardIsUniqueFromCards(CardInfo card, CardInfo[] cards)
        {
            bool unique = true;

            foreach (CardInfo otherCard in cards)
            {
                if (card.cardName == otherCard.cardName)
                {
                    unique = false;
                }
            }

            return unique;
        }

        public bool CardDoesNotConflictWithCards(CardInfo card, CardInfo[] cards)
        {
            bool conflicts = false;

            foreach (CardInfo otherCard in cards)
            {
                if (card.categories.Intersect(otherCard.blacklistedCategories).Any())
                {
                    conflicts = true;
                }
            }

            return conflicts;
        }

        public bool PlayerIsAllowedCard(Player player, CardInfo card)
        {
            bool blacklisted = false;

            foreach (CardInfo currentCard in player.data.currentCards)
            {
                if (card.categories.Intersect(currentCard.blacklistedCategories).Any())
                {
                    blacklisted = true;
                }
            }

            return !blacklisted && (card.allowMultiple || !player.data.currentCards.Where(cardinfo => cardinfo.name == card.name).Any());

        }

        public int CountPlayerCardsWithCondition(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, Func<CardInfo, Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, bool> condition)
        {
            return this.GetPlayerCardsWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, condition).Length;
        }
        public CardInfo[] GetPlayerCardsWithCondition(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, Func<CardInfo, Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, bool> condition)
        {
            return player.data.currentCards.Where(cardinfo => condition(cardinfo, player, gun, gunAmmo, data, health, gravity, block, characterStats)).ToArray();
        }
        public int GetRandomCardIDWithCondition(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, Func<CardInfo, Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, bool> condition, int maxattempts = 1000)
        {
            CardInfo card = this.GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, condition, maxattempts);
            if (card != null)
            {
                return this.GetCardID(card);
            }
            else
            {
                return -1;
            }
            
        }
        public CardInfo GetRandomCardWithCondition(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, Func<CardInfo, Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, bool> condition, int maxattempts = 1000)
        {
            // get array of all cards
            CardInfo[] cards = global::CardChoice.instance.cards;

            // pseudorandom number generator
            int rID = rng.Next(0, cards.Length); // random card index

            int i = 0;

            // draw a random card until it's an uncommon or the maximum number of attempts was reached
            while (!condition(cards[rID], player, gun, gunAmmo, data, health, gravity, block, characterStats) && i < maxattempts)
            {
                rID = rng.Next(0, cards.Length);
                i++;
            }

            if (!condition(cards[rID], player, gun, gunAmmo, data, health, gravity, block, characterStats))
            {
                return null;
            }
            else
            {
                return cards[rID];
            }

        }

        public int GetCardID(CardInfo card)
        {
            return Array.IndexOf(global::CardChoice.instance.cards, card);
        }
    }
}

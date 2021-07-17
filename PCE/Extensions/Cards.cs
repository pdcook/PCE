using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnboundLib;
using Photon.Pun;
using UnboundLib.Cards;
using System.Reflection;
using HarmonyLib;
using UnboundLib.Networking;

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
        public void AddCardToPlayer(Player player, CardInfo card)
        {
            // adds the card "card" to the player "player"
            if (card == null) { return; }
            else if (PhotonNetwork.OfflineMode)
            {
                // assign card locally
                ApplyCardStats cardStats = card.gameObject.GetComponentInChildren<ApplyCardStats>();
                cardStats.GetComponent<CardInfo>().sourceCard = card;
                if (card.GetAdditionalData().canBeAssigned)
                {
                    cardStats.Pick(player.playerID, true, PickerType.Player);
                }
                else
                {
                    CardBarHandler.instance.AddCard(player.playerID, cardStats.GetComponent<CardInfo>().sourceCard);
                }
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                // assign card with RPC

                Player[] array = new Player[] { player };
                int[] array2 = new int[array.Length];

                for (int j = 0; j < array.Length; j++)
                {
                    array2[j] = array[j].data.view.ControllerActorNr;
                }

                NetworkingManager.RPC(typeof(Cards), "RPCA_AssignCard", new object[] { Cards.instance.GetCardID(card), array2 });

            }
        }
        public void AddCardsToPlayer(Player player, CardInfo[] cards)
        {
            foreach (CardInfo card in cards)
            {
                this.AddCardToPlayer(player, card);
            }
        }

        public CardInfo[] RemoveCardsFromPlayer(Player player, int[] indeces)
        {
            List<CardInfo> removed = new List<CardInfo>() { };

            foreach (int idx in indeces)
            {
                removed.Add(this.RemoveCardFromPlayer(player, idx));
            }

            return removed.ToArray();
        }
        public int RemoveCardsFromPlayer(Player player, CardInfo[] cards, SelectionType selectType = SelectionType.All)
        {
            int removed = 0;

            foreach (CardInfo card in cards)
            {
                removed += this.RemoveCardFromPlayer(player, card, selectType);
            }

            return removed;
        }

        public CardInfo RemoveCardFromPlayer(Player player, int idx)
        {
            // copy player's currentCards list
            List<CardInfo> originalCards = new List<CardInfo>() { };
            foreach (CardInfo origCard in player.data.currentCards)
            {
                originalCards.Add(origCard);
            }

            List<CardInfo> newCards = new List<CardInfo>() { };

            for (int i = 0; i < originalCards.Count; i++)
            {
                if (i != idx) { newCards.Add(originalCards[i]); }
            }

            // now we remove all of the cards from the player
            this.RemoveAllCardsFromPlayer(player);

            // then add back only the ones we didn't remove
            this.AddCardsToPlayer(player, newCards.ToArray());

            // return the card that was removed
            return originalCards[idx];
        }
        public int RemoveCardFromPlayer(Player player, CardInfo card, SelectionType selectType = SelectionType.All)
        {
            // copy player's currentCards list
            List<CardInfo> originalCards = new List<CardInfo>() { };
            foreach (CardInfo origCard in player.data.currentCards)
            {
                originalCards.Add(origCard);
            }

            // get list of all indeces that the card appears
            List<int> indeces = Enumerable.Range(0, player.data.currentCards.Count).Where(idx => player.data.currentCards[idx].name == card.name).ToList();

            int start = 0;
            int end = indeces.Count;

            switch (selectType)
            {
                case SelectionType.All:
                    start = 0;
                    end = indeces.Count;
                    break;
                case SelectionType.Newest:
                    start = indeces.Count - 1;
                    end = start + 1;
                    break;
                case SelectionType.Oldest:
                    start = 0;
                    end = start + 1;
                    break;
                case SelectionType.Random:
                    start = rng.Next(0, indeces.Count);
                    end = start + 1;
                    break;
            }

            List<int> indecesToRemove = new List<int>() { };
            for (int i = start; i < end; i++)
            {
                indecesToRemove.Add(indeces[i]);
            }

            List<CardInfo> newCards = new List<CardInfo>() { };

            for (int i = 0; i < originalCards.Count; i++)
            {
                if (!indecesToRemove.Contains(i))
                {
                    newCards.Add(originalCards[i]);
                }
            }

            // now we remove all of the cards from the player
            this.RemoveAllCardsFromPlayer(player);

            // then add back only the ones we didn't remove
            this.AddCardsToPlayer(player, newCards.ToArray());

            // return the number of cards removed
            return indecesToRemove.Count;
        }
        public CardInfo[] RemoveAllCardsFromPlayer(Player player)
        {
            // copy currentCards
            List<CardInfo> cards = new List<CardInfo>();
            foreach (CardInfo origCard in player.data.currentCards)
            {
                cards.Add(origCard);
            }

            // for custom cards, call OnRemoveCard
            foreach (CardInfo card in player.data.currentCards)
            {
                if (card.GetComponent<CustomCard>() != null)
                {
                    card.GetComponent<CustomCard>().OnRemoveCard();
                }
            }

            if (PhotonNetwork.OfflineMode)
            {
                // remove all the cards from the player by calling the PATCHED FullReset
                typeof(Player).InvokeMember("FullReset",
                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                        BindingFlags.NonPublic, null, player, new object[] { });
                CardBar[] cardBars = (CardBar[])Traverse.Create(CardBarHandler.instance).Field("cardBars").GetValue();
                cardBars[player.teamID].ClearBar();
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(Cards), "RPCA_FullReset", new object[] { player.data.view.ControllerActorNr });
                NetworkingManager.RPC(typeof(Cards), "RPCA_ClearCardBar", new object[] { player.data.view.ControllerActorNr });
            }

            return cards.ToArray(); // return the removed cards

        }
        public CardInfo ReplaceCard(Player player, int idx, CardInfo newCard)
        {
            // copy player's currentCards list
            List<CardInfo> originalCards = new List<CardInfo>() { };
            foreach (CardInfo origCard in player.data.currentCards)
            {
                originalCards.Add(origCard);
            }

            List<CardInfo> newCards = new List<CardInfo>() { };

            for (int i = 0; i < originalCards.Count; i++)
            {
                if (i != idx) { newCards.Add(originalCards[i]); }
                else { newCards.Add(newCard); }
            }

            // now we remove all of the cards from the player
            this.RemoveAllCardsFromPlayer(player);

            // then add back the new card
            this.AddCardsToPlayer(player, newCards.ToArray());

            // return the card that was removed
            return originalCards[idx];
        }
        public int ReplaceCard(Player player, CardInfo cardToReplace, CardInfo newCard, SelectionType selectType = SelectionType.All)
        {
            // copy player's currentCards list
            List<CardInfo> originalCards = new List<CardInfo>() { };
            foreach (CardInfo origCard in player.data.currentCards)
            {
                originalCards.Add(origCard);
            }

            // get list of all indeces that the card appears
            List<int> indeces = Enumerable.Range(0, player.data.currentCards.Count).Where(idx => player.data.currentCards[idx].name == cardToReplace.name).ToList();

            int start = 0;
            int end = indeces.Count;

            switch (selectType)
            {
                case SelectionType.All:
                    start = 0;
                    end = indeces.Count;
                    break;
                case SelectionType.Newest:
                    start = indeces.Count - 1;
                    end = start + 1;
                    break;
                case SelectionType.Oldest:
                    start = 0;
                    end = start + 1;
                    break;
                case SelectionType.Random:
                    start = rng.Next(0, indeces.Count);
                    end = start + 1;
                    break;
            }

            List<int> indecesToReplace = new List<int>() { };
            for (int i = start; i < end; i++)
            {
                indecesToReplace.Add(indeces[i]);
            }

            List<CardInfo> newCards = new List<CardInfo>() { };

            for (int i = 0; i < originalCards.Count; i++)
            {
                if (!indecesToReplace.Contains(i))
                {
                    newCards.Add(originalCards[i]);
                }
                else
                {
                    newCards.Add(newCard);
                }
            }

            // now we remove all of the cards from the player
            this.RemoveAllCardsFromPlayer(player);

            // then add back the new cards
            this.AddCardsToPlayer(player, newCards.ToArray());

            // return the number of cards replaced
            return indecesToReplace.Count;
        }
        [UnboundRPC]
        public static void RPCA_AssignCard(int cardID, int[] actorIDs)
        {
            Player playerToUpgrade;

            for (int i = 0; i < actorIDs.Length; i++)
            {
                CardInfo[] cards = global::CardChoice.instance.cards;
                ApplyCardStats cardStats = cards[cardID].gameObject.GetComponentInChildren<ApplyCardStats>();

                // call Start to initialize card stat components for base-game cards
                typeof(ApplyCardStats).InvokeMember("Start",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, cardStats, new object[] { });
                cardStats.GetComponent<CardInfo>().sourceCard = cards[cardID];

                playerToUpgrade = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithActorID",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { actorIDs[i] });

                Traverse.Create(cardStats).Field("playerToUpgrade").SetValue(playerToUpgrade);

                if (cards[cardID].GetAdditionalData().canBeAssigned)
                {
                    typeof(ApplyCardStats).InvokeMember("ApplyStats",
                                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                                        BindingFlags.NonPublic, null, cardStats, new object[] { });
                }
                CardBarHandler.instance.AddCard(playerToUpgrade.playerID, cardStats.GetComponent<CardInfo>().sourceCard);
            }
        }
        [UnboundRPC]
        public static void RPCA_FullReset(int actorID)
        {
            Player playerToReset = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithActorID",
                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                    BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { actorID });

            // remove all the cards from the player by calling the PATCHED FullReset
            typeof(Player).InvokeMember("FullReset",
                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                    BindingFlags.NonPublic, null, playerToReset, new object[] { });
        }
        [UnboundRPC]
        public static void RPCA_ClearCardBar(int actorID)
        {
            CardBar[] cardBars = (CardBar[])Traverse.Create(CardBarHandler.instance).Field("cardBars").GetValue();
            Player playerToReset = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithActorID",
            BindingFlags.Instance | BindingFlags.InvokeMethod |
            BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { actorID });

            cardBars[playerToReset.teamID].ClearBar();

        }
        public enum SelectionType
        {
            All,
            Oldest,
            Newest,
            Random
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

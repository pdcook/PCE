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
using UnityEngine;
using TMPro;
using PCE.Extensions;

namespace PCE.Utils
{
    public sealed class Cards
    {
        // singleton design, so that the RNG isn't reset each call
        public static readonly Cards instance = new Cards();
        private static readonly System.Random rng = new System.Random();
        private List<CardInfo> hiddenCards = new List<CardInfo>() { };

        private List<CardInfo> ACTIVEANDHIDDENCARDS
        {
            get 
            {
                return this.activeCards.ToList().Concat(this.hiddenCards).ToList();
            }
            set { }
        }
        private CardInfo[] activeCards
        {
            get
            {
                return global::CardChoice.instance.cards;
            }
            set { }
        }

        private Cards()
        {
            Cards instance = this;
        }
        public void AddCardToPlayer(Player player, CardInfo card, bool reassign = false, string twoLetterCode = "", float forceDisplay = 0f, float forceDisplayDelay = 0f)
        {
            // adds the card "card" to the player "player"
            if (card == null) { return; }
            else if (PhotonNetwork.OfflineMode)
            {
                // assign card locally
                ApplyCardStats cardStats = card.gameObject.GetComponentInChildren<ApplyCardStats>();

                // call Start to initialize card stat components for base-game cards
                typeof(ApplyCardStats).InvokeMember("Start",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, cardStats, new object[] { });
                cardStats.GetComponent<CardInfo>().sourceCard = card;

                Traverse.Create(cardStats).Field("playerToUpgrade").SetValue(player);

                if (!reassign || card.GetAdditionalData().canBeReassigned)
                {
                    typeof(ApplyCardStats).InvokeMember("ApplyStats",
                                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                                        BindingFlags.NonPublic, null, cardStats, new object[] { });
                }
                else
                {
                    player.data.currentCards.Add(card);
                }
                Cards.SilentAddToCardBar(player.playerID, cardStats.GetComponent<CardInfo>().sourceCard, twoLetterCode, forceDisplay, forceDisplayDelay);
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

                NetworkingManager.RPC(typeof(Cards), "RPCA_AssignCard", new object[] { Cards.instance.GetCardID(card), array2, reassign, twoLetterCode, forceDisplay, forceDisplayDelay});

            }
        }
        public void AddCardsToPlayer(Player player, CardInfo[] cards, bool reassign = false, string[] twoLetterCodes = null, float[] forceDisplays = null, float[] forceDisplayDelays = null)
        {
            bool[] reassigns = new bool[cards.Length];
            for (int i = 0; i < cards.Length; i++)
            {
                reassigns[i] = reassign;
            }

            this.AddCardsToPlayer(player, cards, reassigns, twoLetterCodes, forceDisplays, forceDisplayDelays);
        }
        public void AddCardsToPlayer(Player player, CardInfo[] cards, bool[] reassigns = null, string[] twoLetterCodes = null, float[] forceDisplays = null, float[] forceDisplayDelays = null)
        {
            if (reassigns == null)
            {
                reassigns = new bool[cards.Length];
                for(int i = 0; i < reassigns.Length; i++)
                {
                    reassigns[i] = false;
                }
            }
            if (twoLetterCodes == null)
            {
                twoLetterCodes = new string[cards.Length];
                for (int i = 0; i < twoLetterCodes.Length; i++)
                {
                    twoLetterCodes[i] = "";
                }
            }
            if (forceDisplays == null)
            {
                forceDisplays = new float[cards.Length];
                for (int i = 0; i < forceDisplays.Length; i++)
                {
                    forceDisplays[i] = 0f;
                }
            }
            if (forceDisplayDelays == null)
            {
                forceDisplayDelays = new float[cards.Length];
                for (int i = 0; i < forceDisplayDelays.Length; i++)
                {
                    forceDisplayDelays[i] = 0f;
                }
            }

            for (int i = 0; i<cards.Length; i++)
            {
                this.AddCardToPlayer(player, cards[i], reassigns[i], twoLetterCodes[i], forceDisplays[i], forceDisplayDelays[i]);
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
            Unbound.Instance.ExecuteAfterSeconds(0.1f, () =>
            {
                Utils.CardBarUtils.instance.ClearCardBar(player);
                // then add back only the ones we didn't remove, marking them as reassignments
                this.AddCardsToPlayer(player, newCards.ToArray(), true);
            });
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
            this.RemoveAllCardsFromPlayer(player, false);
            Unbound.Instance.ExecuteAfterSeconds(0.1f, () =>
            {
                Utils.CardBarUtils.instance.ClearCardBar(player);
                // then add back only the ones we didn't remove
                this.AddCardsToPlayer(player, newCards.ToArray(), true);
            });
            // return the number of cards removed
            return indecesToRemove.Count;
        }
        public CardInfo[] RemoveAllCardsFromPlayer(Player player, bool clearBar = true)
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
                    try
                    {
                        card.GetComponent<CustomCard>().OnRemoveCard();
                    }
                    catch (NotImplementedException)
                    { }
                }
            }

            if (PhotonNetwork.OfflineMode)
            {
                // remove all the cards from the player by calling the PATCHED FullReset
                typeof(Player).InvokeMember("FullReset",
                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                        BindingFlags.NonPublic, null, player, new object[] { });
                if (clearBar)
                {
                    Utils.CardBarUtils.instance.ClearCardBar(player);
                }
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(Cards), "RPCA_FullReset", new object[] { player.data.view.ControllerActorNr });
                if (clearBar)
                {
                    Utils.CardBarUtils.instance.ClearCardBar(player);
                }
            }

            return cards.ToArray(); // return the removed cards

        }
        public System.Collections.IEnumerator ReplaceCard(Player player, int idx, CardInfo newCard, string twoLetterCode = "", float forceDisplay = 0f, float forceDisplayDelay = 0f)
        {
            if (newCard == null)
            {
                yield break;
            }
            List<string> twoLetterCodes = new List<string>() { };
            List<float> forceDisplays = new List<float>() { };
            List<float> forceDisplayDelays = new List<float>() { };

            // copy player's currentCards list
            List<CardInfo> originalCards = new List<CardInfo>() { };
            foreach (CardInfo origCard in player.data.currentCards)
            {
                originalCards.Add(origCard);
            }

            List<CardInfo> newCards = new List<CardInfo>() { };

            for (int i = 0; i < originalCards.Count; i++)
            {
                if (i != idx)
                {
                    newCards.Add(originalCards[i]);
                    twoLetterCodes.Add("");
                    forceDisplays.Add(0f);
                    forceDisplayDelays.Add(0f);
                }
                else
                {
                    newCards.Add(newCard);
                    twoLetterCodes.Add(twoLetterCode);
                    forceDisplays.Add(forceDisplay);
                    forceDisplayDelays.Add(forceDisplayDelay);
                }
            }

            // now we remove all of the cards from the player
            this.RemoveAllCardsFromPlayer(player);

            yield return new WaitForSecondsRealtime(0.1f);

            Utils.CardBarUtils.instance.ClearCardBar(player);
            // then add back the new card
            this.AddCardsToPlayer(player, newCards.ToArray(), true, twoLetterCodes.ToArray(), forceDisplays.ToArray(), forceDisplayDelays.ToArray());

            yield break;
            // return the card that was removed
            //return originalCards[idx];
        }
        public System.Collections.IEnumerator ReplaceCards(Player player, int[] indeces, CardInfo[] newCards, string[] twoLetterCodes = null)
        {
            if (twoLetterCodes == null)
            {
                twoLetterCodes = new string[indeces.Length];
                for (int i = 0; i < twoLetterCodes.Length; i++)
                {
                    twoLetterCodes[i] = "";
                }
            }
            // copy player's currentCards list
            List<CardInfo> originalCards = new List<CardInfo>() { };
            foreach (CardInfo origCard in player.data.currentCards)
            {
                originalCards.Add(origCard);
            }

            List<CardInfo> newCardsToAssign = new List<CardInfo>() { };
            List<string> twoLetterCodesToAssign = new List<string>() { };

            int j = 0;
            for (int i = 0; i < originalCards.Count; i++)
            {
                if (!indeces.Contains(i))
                {
                    newCardsToAssign.Add(originalCards[i]);
                    twoLetterCodesToAssign.Add("");
                }
                else if (newCards[j] == null)
                {
                    newCardsToAssign.Add(originalCards[i]);
                    twoLetterCodesToAssign.Add("");
                    j++;
                }
                else
                {
                    newCardsToAssign.Add(newCards[j]);
                    twoLetterCodesToAssign.Add(twoLetterCodes[j]);
                    j++;
                }
            }

            // now we remove all of the cards from the player
            this.RemoveAllCardsFromPlayer(player);

            yield return new WaitForSecondsRealtime(0.1f);

            Utils.CardBarUtils.instance.ClearCardBar(player);
            // then add back the new cards
            this.AddCardsToPlayer(player, newCardsToAssign.ToArray(), true, twoLetterCodesToAssign.ToArray());

            yield break;
            // return the card that was removed
            //return originalCards[idx];
        }
        public System.Collections.IEnumerator ReplaceCard(Player player, CardInfo cardToReplace, CardInfo newCard, string twoLetterCode = "", float forceDisplay = 0f, float forceDisplayDelay = 0f, SelectionType selectType = SelectionType.All)
        {
            if (newCard == null)
            {
                yield break;
            }
            List<string> twoLetterCodes = new List<string>() { };
            List<float> forceDisplays = new List<float>() { };
            List<float> forceDisplayDelays = new List<float>() { };

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
                    twoLetterCodes.Add("");
                    forceDisplays.Add(0f);
                    forceDisplayDelays.Add(0f);
                }
                else
                {
                    newCards.Add(newCard);
                    twoLetterCodes.Add(twoLetterCode);
                    forceDisplays.Add(forceDisplay);
                    forceDisplayDelays.Add(forceDisplayDelay);
                }
            }

            // now we remove all of the cards from the player
            this.RemoveAllCardsFromPlayer(player);

            //Unbound.Instance.ExecuteAfterSeconds(0.1f, () =>
            //{
            yield return new WaitForSecondsRealtime(0.1f);

            Utils.CardBarUtils.instance.ClearCardBar(player);
                // then add back the new card
                this.AddCardsToPlayer(player, newCards.ToArray(), true, twoLetterCodes.ToArray(), forceDisplays.ToArray(), forceDisplayDelays.ToArray());
            //});

            yield break;

            // return the number of cards replaced
            //return indecesToReplace.Count;
        }
        [UnboundRPC]
        public static void RPCA_AssignCard(int cardID, int[] actorIDs, bool reassign, string twoLetterCode, float forceDisplay, float forceDisplayDelay)
        {
            Player playerToUpgrade;

            for (int i = 0; i < actorIDs.Length; i++)
            {
                CardInfo[] cards = Cards.instance.ACTIVEANDHIDDENCARDS.ToArray();
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

                if (!reassign || cards[cardID].GetAdditionalData().canBeReassigned)
                {
                    typeof(ApplyCardStats).InvokeMember("ApplyStats",
                                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                                        BindingFlags.NonPublic, null, cardStats, new object[] { });
                }
                else
                {
                    playerToUpgrade.data.currentCards.Add(cards[cardID]);
                }
                Cards.SilentAddToCardBar(playerToUpgrade.playerID, cardStats.GetComponent<CardInfo>().sourceCard, twoLetterCode, forceDisplay, forceDisplayDelay);
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

        public bool CardIsNotBlacklisted(CardInfo card, CardCategory[] blacklistedCategories)
        {
            bool blacklisted = false;

            if (card.categories.Intersect(blacklistedCategories).Any())
            {
                blacklisted = true;
            }

            return !blacklisted;
        }

        public bool CardDoesNotConflictWithCardsCategories(CardInfo card, CardInfo[] cards)
        {
            bool conflicts = false;

            if (cards.Length == 0) { return !conflicts; }

            foreach (CardInfo otherCard in cards)
            {
                if (card.categories != null && otherCard.blacklistedCategories != null && card.categories.Intersect(otherCard.blacklistedCategories).Any())
                {
                    conflicts = true;
                }
            }

            return !conflicts;
        }
        public bool CardDoesNotConflictWithCards(CardInfo card, CardInfo[] cards)
        {
            bool conflicts = false;

            if (cards.Length == 0) { return !conflicts; }

            foreach (CardInfo otherCard in cards)
            {
                if (card.categories != null && otherCard.blacklistedCategories != null && card.categories.Intersect(otherCard.blacklistedCategories).Any())
                {
                    conflicts = true;
                }
            }

            return !conflicts && (card.allowMultiple || !cards.Where(cardinfo => cardinfo.name == card.name).Any());
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
        public int NORARITY_GetRandomCardIDWithCondition(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, Func<CardInfo, Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, bool> condition, int maxattempts = 1000)
        {
            CardInfo card = this.NORARITY_GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, condition, maxattempts);
            if (card != null)
            {
                return this.GetCardID(card);
            }
            else
            {
                return -1;
            }
            
        }
        // get random card without respecting rarity, but always respecting PlayerIsAllowedCard
        public CardInfo NORARITY_GetRandomCardWithCondition(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, Func<CardInfo, Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, bool> condition, int maxattempts = 1000)
        {
            // get array of all ACTIVE cards
            CardInfo[] cards = this.activeCards;

            // pseudorandom number generator
            int rID = rng.Next(0, cards.Length); // random card index

            int i = 0;

            // draw a random card until it's an uncommon or the maximum number of attempts was reached
            while (!(condition(cards[rID], player, gun, gunAmmo, data, health, gravity, block, characterStats) && this.PlayerIsAllowedCard(player,cards[rID])) && i < maxattempts)
            {
                rID = rng.Next(0, cards.Length);
                i++;
            }

            if (!(condition(cards[rID], player, gun, gunAmmo, data, health, gravity, block, characterStats) && this.PlayerIsAllowedCard(player, cards[rID])))
            {
                return null;
            }
            else
            {
                return cards[rID];
            }

        }
        // get random card using the base-game's spawn method (which respects rarities), also satisfying some conditions - always including PlayerIsAllowedCard
        public CardInfo GetRandomCardWithCondition(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, Func<CardInfo, Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, bool> condition, int maxattempts = 1000)
        {

            CardInfo card = ((GameObject)typeof(CardChoice).InvokeMember("GetRanomCard",
                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                        BindingFlags.NonPublic, null, global::CardChoice.instance, new object[] { })).GetComponent<CardInfo>();

            int i = 0;

            // draw a random card until it's an uncommon or the maximum number of attempts was reached
            while (!(condition(card, player, gun, gunAmmo, data, health, gravity, block, characterStats) && this.PlayerIsAllowedCard(player, card)) && i < maxattempts)
            {
                card = ((GameObject)typeof(CardChoice).InvokeMember("GetRanomCard",
                           BindingFlags.Instance | BindingFlags.InvokeMethod |
                           BindingFlags.NonPublic, null, global::CardChoice.instance, new object[] { })).GetComponent<CardInfo>();
                i++;
            }

            if (!(condition(card, player, gun, gunAmmo, data, health, gravity, block, characterStats) && this.PlayerIsAllowedCard(player, card)))
            {
                return null;
            }
            else
            {
                return card;
            }

        }
        public int GetCardID(CardInfo card)
        {
            return Array.IndexOf(this.ACTIVEANDHIDDENCARDS.ToArray(), card);
        }
        public CardInfo GetCardWithID(int cardID)
        {
            return this.ACTIVEANDHIDDENCARDS[cardID];
        }

        internal static void SilentAddToCardBar(int teamID, CardInfo card, string twoLetterCode = "", float forceDisplay = 0f, float forceDisplayDelay = 0f)
        {
            CardBar[] cardBars = (CardBar[])Traverse.Create(CardBarHandler.instance).Field("cardBars").GetValue();

            Traverse.Create(cardBars[teamID]).Field("ci").SetValue(card);
            GameObject source = (GameObject)Traverse.Create(cardBars[teamID]).Field("source").GetValue();
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(source, source.transform.position, source.transform.rotation, source.transform.parent);
            gameObject.transform.localScale = Vector3.one;
            string text = card.cardName;
            if (twoLetterCode != "") { text = twoLetterCode; }
            text = text.Substring(0, 2);
            string text2 = text[0].ToString().ToUpper();
            if (text.Length > 1)
            {
                string str = text[1].ToString().ToLower();
                text = text2 + str;
            }
            else
            {
                text = text2;
            }
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
            Traverse.Create(gameObject.GetComponent<CardBarButton>()).Field("card").SetValue(card);
            gameObject.gameObject.SetActive(true);
        }

        internal void AddHiddenCard(CardInfo card)
        {
            this.hiddenCards.Add(card);
        }

    }
}

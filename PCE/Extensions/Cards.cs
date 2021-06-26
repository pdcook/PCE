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

        public void AddCardToPlayer(Player player, CardInfo card)
        {
            if (card == null) { return; }
            else if (PhotonNetwork.OfflineMode)
            {
                // assign card locally
                ApplyCardStats cardStats = card.gameObject.GetComponentInChildren<ApplyCardStats>();
                cardStats.GetComponent<CardInfo>().sourceCard = card;
                cardStats.Pick(player.playerID, true, PickerType.Player);
            }
            else
            {
                // assign card with RPC

                Player[] array = new Player[] { player };
                int[] array2 = new int[array.Length];

                for (int j = 0; j < array.Length; j++)
                {
                    array2[j] = array[j].data.view.ControllerActorNr;
                }
                if (player.GetComponent<PhotonView>().IsMine)
                {

                    player.GetComponent<PhotonView>().RPC("RPCA_AssignCard", RpcTarget.All, new object[] { Cards.instance.GetCardID(card), array2 });

                }
            }
        }
        [PunRPC]
        public void RPCA_AssignCard(int cardID, int[] actorIDs)
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

                typeof(ApplyCardStats).InvokeMember("ApplyStats",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, cardStats, new object[] { });
                CardBarHandler.instance.AddCard(playerToUpgrade.playerID, cardStats.GetComponent<CardInfo>().sourceCard);
            }
        }
    }
}

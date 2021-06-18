using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using Photon.Pun;

namespace PCE.Cards
{
    public class GambleCard : CustomCard
    {
        /*
        *  A Rare card which gives the player two random Uncommon cards
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // get array of all cards
            CardInfo[] cards = global::CardChoice.instance.cards;

            // pseudorandom number generator
            System.Random r = new System.Random();

            int rID1 = r.Next(0, cards.Length); // random card index
            int rID2 = r.Next(0, cards.Length); // random card index


            int i = 0;
            int maxAttempts = 1000;

            // draw a random card until it's an uncommon or the maximum number of attempts was reached
            while ((cards[rID1].rarity != CardInfo.Rarity.Uncommon && i < maxAttempts) || (cards[rID1].cardName.Contains("Jackpot") || cards[rID1].cardName.Contains("Gamble")))
            {
                rID1 = r.Next(0, cards.Length);
                i++;
            }
            // draw a random card until it's an uncommon or the maximum number of attempts was reached
            while ((cards[rID2].rarity != CardInfo.Rarity.Uncommon && i < maxAttempts) || (cards[rID2].cardName.Contains("Jackpot") || cards[rID2].cardName.Contains("Gamble")))
            {
                rID2 = r.Next(0, cards.Length);
                i++;
            }


            // add the cards to the player's deck


            if (PhotonNetwork.OfflineMode)
            {
                // assign card locally
                ApplyCardStats randomCardStats1 = cards[rID1].gameObject.GetComponentInChildren<ApplyCardStats>();
                randomCardStats1.GetComponent<CardInfo>().sourceCard = cards[rID1];
                randomCardStats1.Pick(player.playerID, true, PickerType.Player);
                ApplyCardStats randomCardStats2 = cards[rID2].gameObject.GetComponentInChildren<ApplyCardStats>();
                randomCardStats2.GetComponent<CardInfo>().sourceCard = cards[rID2];
                randomCardStats2.Pick(player.playerID, true, PickerType.Player);
            }
            else
            {
                // assign cards with RPC
                Player[] array = new Player[] { player };
                int[] array2 = new int[array.Length];

                for (int j = 0; j < array.Length; j++)
                {
                    array2[j] = array[j].data.view.ControllerActorNr;
                }
                if (base.GetComponent<PhotonView>().IsMine)
                {

                    base.GetComponent<PhotonView>().RPC("RPCA_AssignCard", RpcTarget.All, new object[] { rID1, array2 });
                    base.GetComponent<PhotonView>().RPC("RPCA_AssignCard", RpcTarget.All, new object[] { rID2, array2 });


                }
            }
            return;

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Gamble";
        }
        protected override string GetDescription()
        {
            return "Get <b>two</b> random <color=#00ffffff>Uncommon</color> cards";
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
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
        [PunRPC]
        public void RPCA_AssignCard(int cardID, int[] actorIDs)
        {
            Player playerToUpgrade;

            for (int i = 0; i < actorIDs.Length; i++)
            {
                CardInfo[] cards = global::CardChoice.instance.cards;
                ApplyCardStats randomCardStats = cards[cardID].gameObject.GetComponentInChildren<ApplyCardStats>();

                // call Start to initialize card stat components for base-game cards
                typeof(ApplyCardStats).InvokeMember("Start",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, randomCardStats, new object[] { });
                randomCardStats.GetComponent<CardInfo>().sourceCard = cards[cardID];

                playerToUpgrade = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithActorID",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { actorIDs[i] });

                Traverse.Create(randomCardStats).Field("playerToUpgrade").SetValue(playerToUpgrade);

                typeof(ApplyCardStats).InvokeMember("ApplyStats",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, randomCardStats, new object[] { });
                CardBarHandler.instance.AddCard(((Player)Traverse.Create(cardStats).Field("playerToUpgrade").GetValue()).playerID, randomCardStats.GetComponent<CardInfo>().sourceCard);
            }
        }
    }
}

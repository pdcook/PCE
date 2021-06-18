using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using Photon.Pun;
using System.Reflection;
using HarmonyLib;

namespace PCE.Cards
{
    public class JackpotCard : CustomCard
    {
        /*
        *  An Uncommon card which gives the player a random Rare card
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

            int rID = r.Next(0, cards.Length); // random card index

            int i = 0;
            int maxAttempts = 1000;

            // draw a random card until it's a rare or the maximum number of attempts was reached
            while ((cards[rID].rarity != CardInfo.Rarity.Rare && i < maxAttempts) || (cards[rID].cardName.Contains("Jackpot") || cards[rID].cardName.Contains("Gamble")))
            {
                rID = r.Next(0, cards.Length);
                i++;
            }

            // add the card to the player's deck


            if (PhotonNetwork.OfflineMode)
            {
                // assign card locally
                ApplyCardStats randomCardStats = cards[rID].gameObject.GetComponentInChildren<ApplyCardStats>();
                randomCardStats.GetComponent<CardInfo>().sourceCard = cards[rID];
                randomCardStats.Pick(player.playerID, true, PickerType.Player);
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
                if (base.GetComponent<PhotonView>().IsMine)
                {

                    base.GetComponent<PhotonView>().RPC("RPCA_AssignCard", RpcTarget.All, new object[] { rID, array2 });

                }
            }
            return;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Jackpot";
        }
        protected override string GetDescription()
        {
            return "Get a random <color=#ff00ffff>Rare</color> card";
        }

        protected override GameObject GetCardArt()
        {
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Jackpot");
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
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

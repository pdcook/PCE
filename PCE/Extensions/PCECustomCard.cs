using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using UnboundLib.Cards;
using System.Reflection;
using HarmonyLib;
using Photon.Pun;

namespace PCE.Extensions
{
    public abstract class PCECustomCard : CustomCard
    {

        // extension of CustomCard that gives the card object the ability to assign other cards to the player

        public void AddCardToPlayer(Player player, CardInfo card)
        {
            // adds the card "card" to the player "player"
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
                if (base.GetComponent<PhotonView>().IsMine)
                {

                    base.GetComponent<PhotonView>().RPC("RPCA_AssignCard", RpcTarget.All, new object[] { Cards.instance.GetCardID(card), array2 });

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

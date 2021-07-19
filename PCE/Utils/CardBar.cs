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
using UnityEngine.UI;

namespace PCE.Utils
{
    public sealed class CardBarUtils
    {
        // singleton design
        public static readonly CardBarUtils instance = new CardBarUtils();

        private static readonly float displayDuration = 1f;
        private static readonly Vector3 localShift = new Vector3(-50f, 0f, 0f);
        private static readonly float barlocalScaleMult = 1.1f;
        private static readonly float cardLocalScaleMult = 1f;

        private DictionaryOfLists<Player, CardInfo> cardsToShow = new DictionaryOfLists<Player, CardInfo>() { };

        private CardBar[] CardBars
        {
            get
            {
                return (CardBar[])Traverse.Create(CardBarHandler.instance).Field("cardBars").GetValue();
            }
        }

        private CardBar PlayersCardBar(int teamID)
        {
            return this.CardBars[teamID];
        }
        private CardBar PlayersCardBar(Player player)
        {
            return this.CardBars[player.teamID];
        }

        private CardBarUtils()
        {
            CardBarUtils instance = this;
        }

        private void Reset()
        {
            this.cardsToShow = new DictionaryOfLists<Player, CardInfo>() { };
        }
        private void Reset(Player player)
        {
            this.cardsToShow[player] = new List<CardInfo>() { };
        }

        public void ShowAtEndOfPhase(Player player, CardInfo card)
        {
            List<CardInfo> newList = new List<CardInfo>(this.cardsToShow[player]);
            newList.Add(card);
            this.cardsToShow[player] = newList;
        }

        public void ShowCard(Player player, CardInfo card)
        {
            this.ShowCard(player.teamID, card);
        }
        public void ShowCard(int teamID, CardInfo card)
        {
            this.ShowCard(teamID, Cards.instance.GetCardID(card));
        }
        public void ShowCard(Player player, int cardID)
        {
            this.ShowCard(player.teamID, cardID);
        }
        public void ShowCard(int teamID, int cardID)
        {
            try
            {
                if (Cards.instance.GetCardWithID(cardID) == null) { return; }
            }
            catch
            {
                return;
            }
            this.PlayersCardBar(teamID).OnHover(Cards.instance.GetCardWithID(cardID), Vector3.zero);
            ((GameObject)Traverse.Create(this.PlayersCardBar(teamID)).Field("currentCard").GetValue()).GetComponentInChildren<SetScaleToZero>().transform.localScale = Vector3.one * Utils.CardBarUtils.cardLocalScaleMult;
        }

        public void HideCard(Player player)
        {
            this.HideCard(player.teamID);
        }
        public void HideCard(int teamID)
        {
            this.PlayersCardBar(teamID).StopHover();
        }
        public GameObject GetCardBarSquare(int teamID, int idx)
        {
            return this.GetCardBarSquares(teamID)[idx+1];
        }
        public GameObject GetCardBarSquare(Player player, int idx)
        {
            return this.GetCardBarSquare(player.teamID, idx);
        }

        public GameObject[] GetCardBarSquares(int teamID)
        {
            List<GameObject> children = new List<GameObject>() { };

            foreach (Transform child in this.PlayersCardBar(teamID).transform)
            {
                children.Add(child.gameObject);
            }

            return children.ToArray();
        }

        public GameObject[] GetCardBarSquares(Player player)
        {
            return this.GetCardBarSquares(player.teamID);
        }
        public void ResetPlayersLineColor(int teamID)
        {
            List<Graphic> graphics = this.PlayersCardBar(teamID).gameObject.GetComponentsInChildren<Graphic>().Where(gr => !gr.gameObject.name.Contains("CarrdOrange")).ToList();

            foreach (Graphic graphic in graphics)
            {
                if (graphic.gameObject.name.Contains("Card"))
                {
                    graphic.color = new Color(0.462f, 0.462f, 0.462f, 1f);
                }
                else if (graphic.gameObject.name.Contains("Text"))
                {
                    graphic.color = new Color(0.6509f, 0.6509f, 0.6509f, 1f);
                }
            }
        }
        public void ResetPlayersLineColor(Player player)
        {
            this.ResetPlayersLineColor(player.teamID);
        }
        public void ChangePlayersLineColor(int teamID, Color color)
        {
            List<Graphic> graphics = this.PlayersCardBar(teamID).gameObject.GetComponentsInChildren<Graphic>().Where(gr => !gr.gameObject.name.Contains("CarrdOrange")).ToList();
        
            foreach (Graphic graphic in graphics)
            {
                graphic.color = color;
            }
        }
        public void ChangePlayersLineColor(Player player, Color color)
        {
            this.ChangePlayersLineColor(player.teamID, color);
        }

        public Color GetPlayersBarColor(int teamID)
        {
            List<Graphic> graphics = this.PlayersCardBar(teamID).gameObject.GetComponentsInChildren<Graphic>().Where(gr => gr.gameObject.name.Contains("CarrdOrange")).ToList();

           return graphics[0].color;
        }
        public Color GetPlayersBarColor(Player player)
        {
            return this.GetPlayersBarColor(player.teamID);
        }

        public Color ChangePlayersBarColor(int teamID, Color color)
        {
            List<Graphic> graphics = this.PlayersCardBar(teamID).gameObject.GetComponentsInChildren<Graphic>().Where(gr => gr.gameObject.name.Contains("CarrdOrange")).ToList();

            Color orig = graphics[0].color;

            foreach (Graphic graphic in graphics)
            {
                graphic.color = color;
            }

            return orig;
        }

        public Color ChangePlayersBarColor(Player player, Color color)
        {
            return this.ChangePlayersBarColor(player.teamID, color);
        }

        public static void SilentAddToCardBar(int teamID, CardInfo card, string twoLetterCode = "")
        {
            Traverse.Create(CardBarUtils.instance.PlayersCardBar(teamID)).Field("ci").SetValue(card);
            GameObject source = (GameObject)Traverse.Create(CardBarUtils.instance.PlayersCardBar(teamID)).Field("source").GetValue();
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(source, source.transform.position, source.transform.rotation, source.transform.parent);
            gameObject.transform.localScale = Vector3.one;
            string text = card.cardName;
            if (twoLetterCode != "") { text = twoLetterCode; }
            while (text.Length < 2)
            {
                text += " ";
            }
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
        public static void SilentAddToCardBar(Player player, CardInfo card, string twoLetterCode = "")
        {
            CardBarUtils.SilentAddToCardBar(player.teamID, card, twoLetterCode);
        }

        internal System.Collections.IEnumerator EndPickPhaseShow()
        {
            foreach (Player player in PlayerManager.instance.players)
            {
                Color orig = this.GetPlayersBarColor(player);

                if (this.cardsToShow[player].Count > 0)
                {
                    this.PlayersCardBar(player).gameObject.transform.localScale = Vector3.one * Utils.CardBarUtils.barlocalScaleMult;
                    this.PlayersCardBar(player).gameObject.transform.localPosition += CardBarUtils.localShift;
                    this.ChangePlayersLineColor(player, Color.white);
                    Color.RGBToHSV(this.GetPlayersBarColor(player), out float h, out float s, out float v);
                    this.ChangePlayersBarColor(player, Color.HSVToRGB(h, s + 0.1f, v + 0.1f));
                }
                foreach (CardInfo card in this.cardsToShow[player])
                {

                    this.ShowCard(player, card);
                    yield return new WaitForSecondsRealtime(CardBarUtils.displayDuration);
                    this.HideCard(player);
                }
                if (this.cardsToShow[player].Count > 0)
                {
                    this.PlayersCardBar(player).gameObject.transform.localScale = Vector3.one * 1f;
                    this.PlayersCardBar(player).gameObject.transform.localPosition -= CardBarUtils.localShift;
                    this.ResetPlayersLineColor(player);
                    this.ChangePlayersBarColor(player, orig);
                }
            }
            this.Reset();
            yield break;
        }
        internal System.Collections.IEnumerator ShowImmediate(int teamID, int cardID)
        {

            Color orig = this.GetPlayersBarColor(teamID);

            this.PlayersCardBar(teamID).gameObject.transform.localScale = Vector3.one * Utils.CardBarUtils.barlocalScaleMult;
            this.PlayersCardBar(teamID).gameObject.transform.localPosition += CardBarUtils.localShift;
            this.ChangePlayersLineColor(teamID, Color.white);
            Color.RGBToHSV(this.GetPlayersBarColor(teamID), out float h, out float s, out float v);
            this.ChangePlayersBarColor(teamID, Color.HSVToRGB(h, s + 0.1f, v + 0.1f));
                

            this.ShowCard(teamID, cardID);
            yield return new WaitForSecondsRealtime(CardBarUtils.displayDuration);
            this.HideCard(teamID);


            this.PlayersCardBar(teamID).gameObject.transform.localScale = Vector3.one * 1f;
            this.PlayersCardBar(teamID).gameObject.transform.localPosition -= CardBarUtils.localShift;
            this.ResetPlayersLineColor(teamID);
            this.ChangePlayersBarColor(teamID, orig);
            

            yield break;
        }

        internal System.Collections.IEnumerator ShowImmediate(Player player, int cardID)
        {
            return this.ShowImmediate(player.teamID, cardID);
        }
        internal System.Collections.IEnumerator ShowImmediate(Player player, CardInfo card)
        {
            return this.ShowImmediate(player.teamID, Cards.instance.GetCardID(card));
        }
        internal System.Collections.IEnumerator ShowImmediate(int teamID, CardInfo card)
        {
            return this.ShowImmediate(teamID, Cards.instance.GetCardID(card));
        }
        internal System.Collections.IEnumerator ShowImmediate(int teamID, int[] cardIDs)
        {
            Color orig = this.GetPlayersBarColor(teamID);

            this.PlayersCardBar(teamID).gameObject.transform.localScale = Vector3.one * Utils.CardBarUtils.barlocalScaleMult;
            this.PlayersCardBar(teamID).gameObject.transform.localPosition += CardBarUtils.localShift;
            this.ChangePlayersLineColor(teamID, Color.white);
            Color.RGBToHSV(this.GetPlayersBarColor(teamID), out float h, out float s, out float v);
            this.ChangePlayersBarColor(teamID, Color.HSVToRGB(h, s + 0.1f, v + 0.1f));

            foreach (int cardID in cardIDs)
            {
                this.ShowCard(teamID, cardID);
                yield return new WaitForSecondsRealtime(CardBarUtils.displayDuration);
                this.HideCard(teamID);
            }

            this.PlayersCardBar(teamID).gameObject.transform.localScale = Vector3.one * 1f;
            this.PlayersCardBar(teamID).gameObject.transform.localPosition -= CardBarUtils.localShift;
            this.ResetPlayersLineColor(teamID);
            this.ChangePlayersBarColor(teamID, orig);


            yield break;
        }
        internal System.Collections.IEnumerator ShowImmediate(Player player, int[] cardIDs)
        {
            return this.ShowImmediate(player.teamID, cardIDs);
        }
        internal System.Collections.IEnumerator ShowImmediate(int teamID, CardInfo[] cards)
        {
            List<int> cardIDs = new List<int>() { };
            foreach (CardInfo card in cards)
            {
                cardIDs.Add(Cards.instance.GetCardID(card));
            }

            return this.ShowImmediate(teamID, cardIDs.ToArray());
        }
        internal System.Collections.IEnumerator ShowImmediate(Player player, CardInfo[] cards)
        {
            List<int> cardIDs = new List<int>() { };
            foreach (CardInfo card in cards)
            {
                cardIDs.Add(Cards.instance.GetCardID(card));
            }

            return this.ShowImmediate(player.teamID, cardIDs.ToArray());
        }
        public void ClearCardBar(Player player)
        {
            if (PhotonNetwork.OfflineMode)
            {
                this.PlayersCardBar(player).ClearBar();
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(Utils.CardBarUtils), "RPCA_ClearCardBar", new object[] { player.data.view.ControllerActorNr });
            }
        }

        [UnboundRPC]
        private static void RPCA_ClearCardBar(int actorID)
        {
            Player playerToReset = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithActorID",
            BindingFlags.Instance | BindingFlags.InvokeMethod |
            BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { actorID });

            Utils.CardBarUtils.instance.PlayersCardBar(playerToReset.teamID).ClearBar();
        }
    }
    public class DictionaryOfLists<TKey, TListValue> : Dictionary<TKey, List<TListValue>>
    {
        public DictionaryOfLists() : base() { }
        public new List<TListValue> this[TKey key]
        {
            get
            {
                return base.TryGetValue(key, out List<TListValue> t) ? t : new List<TListValue>();
            }
            set { base[key] = value; }
        }
    }
}

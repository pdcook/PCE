using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using System.Reflection;
using System.Linq;
using PCE.Extensions;
using TMPro;
using UnityEngine.UI;
using UnboundLib.Utils;
using UnboundLib.Networking;
using Photon.Pun;
using System.Collections;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;

namespace PCE.Cards
{
    public class RandomCommonCard : RandomCard
    {
        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }

        protected override string GetTitle()
        {
            return "Common";
        }
        protected override string GetDescription()
        {
            return "<color=#ffffffff><b>??? ? ????????? ?????? ???? ???? ???????</b></color>";
        }
    }
    public class RandomUncommonCard : RandomCard
    {
        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.ColdBlue;
        }

        protected override string GetTitle()
        {
            return "Uncommon";
        }
        protected override string GetDescription()
        {
            return "<color=#00ffffff><b>??? ? ????????? ?????? ???? ???? ???????</b></color>";
        }
    }
    public class RandomRareCard : RandomCard
    {
        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.EvilPurple;
        }

        protected override string GetTitle()
        {
            return "Rare";
        }
        protected override string GetDescription()
        {
            return "<color=#ff00ffff><b>??? ? ????????? ?????? ???? ???? ???????</b></color>";
        }
    }
    public abstract class RandomCard : CustomCard
    {
        private static bool randomInProgress = false;

        private static CardCategory[] blacklistedCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("NoRandom"), CustomCardCategories.instance.CardCategory("CardManipulation") };

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.GetAdditionalData().isRandom = true;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            RandomCardEffect effect = player.gameObject.GetOrAddComponent<RandomCardEffect>();
            effect.indeces.Add(player.data.currentCards.Count);
        }
        public override void OnRemoveCard()
        {
        }
        protected override CardInfoStat[] GetStats()
        {
            return null;
        }
        public override string GetModName()
        {
            return "PCE";
        }

        internal static IEnumerator Go()
        {
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(RandomCard), nameof(RPCA_RandomInProgress), new object[] { true });
            }
            yield return new WaitForSecondsRealtime(0.5f);
            if (!PhotonNetwork.OfflineMode && !PhotonNetwork.IsMasterClient)
            {
                while (RandomCard.randomInProgress)
                {
                    yield return null;
                }
                yield break;
            }

            Dictionary<Player, List<CardInfo>> cardsToShow = new Dictionary<Player, List<CardInfo>>();

            foreach (Player player in PlayerManager.instance.players.ToArray())
            {

                if (player.GetComponent<RandomCardEffect>() != null && player.GetComponent<RandomCardEffect>().indeces.Count > 0)
                {
                    List<int> indeces = new List<int>(player.GetComponent<RandomCardEffect>().indeces);
                    List<int> invalidInd = new List<int>() { };
                    List<string> twoLetterCodes = new List<string>() { };
                    List<CardInfo> newCards = new List<CardInfo>() { };
                    foreach (int idx in indeces)
                    {
                        string twoLetterCode = player.GetComponent<RandomCardEffect>().twoLetterCode;
                        CardInfo card = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, null, null, null, null, null, null, null, (card, player, g, ga, d, h, gr, b, s) => ModdingUtils.Utils.Cards.instance.CardDoesNotConflictWithCards(card, newCards.ToArray()) && card.rarity == player.data.currentCards[idx].rarity && ModdingUtils.Extensions.CardInfoExtension.GetAdditionalData(card).canBeReassigned && !Extensions.CardInfoExtension.GetAdditionalData(card).isRandom && ModdingUtils.Utils.Cards.instance.CardIsNotBlacklisted(card, RandomCard.blacklistedCategories));
                        if (card == null)
                        {
                            // if there is no valid card, then try drawing from the list of all cards (inactive + active) but still make sure it is compatible
                            CardInfo[] allCards = ((ObservableCollection<CardInfo>)typeof(CardManager).GetField("activeCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).ToList().Concat((List<CardInfo>)typeof(CardManager).GetField("inactiveCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).ToArray();
                            card = ModdingUtils.Utils.Cards.instance.DrawRandomCardWithCondition(allCards, player, null, null, null, null, null, null, null, (card, player, g, ga, d, h, gr, b, s) => ModdingUtils.Utils.Cards.instance.CardDoesNotConflictWithCards(card, newCards.ToArray()) && card.rarity == player.data.currentCards[idx].rarity && ModdingUtils.Extensions.CardInfoExtension.GetAdditionalData(card).canBeReassigned && !Extensions.CardInfoExtension.GetAdditionalData(card).isRandom && ModdingUtils.Utils.Cards.instance.CardIsNotBlacklisted(card, RandomCard.blacklistedCategories));

                            if (card == null)
                            {
                                // if there is STILL no valid card, then this index is invalid
                                invalidInd.Add(idx);
                                continue;
                            }
                        }
                        twoLetterCodes.Add(twoLetterCode);
                        newCards.Add(card);
                    }
                    indeces = indeces.Except(invalidInd).ToList();
                    cardsToShow[player] = newCards;
                    if (indeces.Count == 0)
                    {
                        continue;
                    }
                    yield return ModdingUtils.Utils.Cards.instance.ReplaceCards(player, indeces.ToArray(), newCards.ToArray(), twoLetterCodes.ToArray());
                }
            }
            yield return new WaitForSecondsRealtime(0.5f);
            float numCardsToShow = 0f;
            foreach (Player player in cardsToShow.Keys)
            {
                numCardsToShow += cardsToShow[player].Count;
            }
            numCardsToShow = UnityEngine.Mathf.Clamp(numCardsToShow, 2f, float.MaxValue);
            foreach (Player player in cardsToShow.Keys)
            {
                if (cardsToShow[player].Count == 0) { continue; }
                yield return ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, cardsToShow[player].ToArray(), 2f / numCardsToShow);
            }
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(RandomCard), nameof(RPCA_RandomInProgress), new object[] { false });
            }
            yield return new WaitForSecondsRealtime(0.1f);
            yield break;
        }
        [UnboundRPC]
        private static void RPCA_RandomInProgress(bool prog)
        {
            RandomCard.randomInProgress = prog;
        }

        internal static IEnumerator ClearRandomCards()
        {
            foreach (Player player in PlayerManager.instance.players)
            {
                CustomEffects.DestroyAllRandomCardEffects(player.gameObject);
            }
            yield break;
        }

        public override void Callback()
        {
            this.gameObject.GetOrAddComponent<RandomCardVisualEffect>();

        }
        private class RandomCardVisualEffect : MonoBehaviour
        {
            private static System.Random random = new System.Random();
            private Color origTriangleColor;
            private float origTriangleHue;
            public static string RandomString(int length)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*()_+-=<>?:\"{ }|,./;'[]\\'~";
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            public static string RandomSentence(int words)
            {
                string sentence = "";

                for (int i = 0; i < words; i++)
                {
                    sentence += RandomString(random.Next(1, 6)) + " ";
                }

                return sentence;
            }
            public static string RandomDescription(int size)
            {
                string desc = "";

                for (int i = 0; i < size; i++)
                {
                    desc += RandomSentence(random.Next(1, 6));
                }

                return desc;
            }
            public static string ObfuscateString(string source)
            {
                string res = "";

                foreach (char c in source)
                {
                    if (c == ' ') { res += " "; }
                    else { res += RandomString(1); }
                }

                return res;

            }
            //chance that this effect will be applied on each call to update
            private readonly float ROTATE_CARD_CHANCE = 5f;

            //chance that each effect will be applied after each image swap
            private readonly float GREYSCALE_CARD_CHANCE = 15f;
            private readonly float ROTATE_IMAGE_CHANCE = 30f;

            private float artStartTime = -1f;
            private float artEffectStartTime = -1f;
            private float rotStartTime = -1f;
            private float scaleStartTime = -1f;
            private readonly float artDelay = 0.05f;
            private readonly float artEffectsDelay = 0.025f;
            private readonly float rotDelay = 0.05f;
            private readonly float scaleDelay = 0.025f;

            private readonly int maxArts = 5;

            private List<GameObject> art = new List<GameObject>() { };
            private GameObject artParent;
            private int artNum = 0;

            private TextMeshProUGUI description;
            private TextMeshProUGUI cardName;
            private List<UnityEngine.UI.Image> triangles = new List<UnityEngine.UI.Image>() { };
            private List<float> triangleTimers = new List<float>() { -1f, -1f, -1f, -1f };
            private List<float> triangleFlashDurations = new List<float>() { 1f, 1f, 1f, 1f };
            private readonly List<float> flashMinMax = new List<float>() { 0.2f, 2f };
            private List<bool> flashUp = new List<bool>() { false, false, false, false };

            private List<CardInfo> activeCards;
            private List<CardInfo> inactiveCards;
            private CardInfo[] allCards;
            private void Awake()
            {
                for (int i = 0; i < this.maxArts; i++)
                {
                    art.Add(null);
                }
            }
            private void Start()
            {
                Image[] images = this.gameObject.GetComponentsInChildren<Image>();
                foreach (Image image in images)
                {
                    if (image.gameObject.name.Contains("Art"))
                    {
                        this.artParent = image.gameObject;
                        break;
                    }
                }

                TextMeshProUGUI[] allChildrenRecursive = this.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
                GameObject effectText = allChildrenRecursive.Where(obj => obj.gameObject.name == "EffectText").FirstOrDefault().gameObject;
                GameObject titleText = allChildrenRecursive.Where(obj => obj.gameObject.name == "Text_Name").FirstOrDefault().gameObject;
                this.description = effectText.GetComponent<TextMeshProUGUI>();
                this.cardName = titleText.GetComponent<TextMeshProUGUI>();

                // add extra text to bottom right
                // create blank object for text, and attach it to the canvas
                // find bottom right edge object
                RectTransform[] allChildrenRecursive2 = this.gameObject.GetComponentsInChildren<RectTransform>();
                GameObject BottomLeftCorner = allChildrenRecursive2.Where(obj => obj.gameObject.name == "EdgePart (1)").FirstOrDefault().gameObject;
                GameObject modNameObj = UnityEngine.GameObject.Instantiate(new GameObject("ExtraCardText", typeof(TextMeshProUGUI), typeof(DestroyOnUnparent)), BottomLeftCorner.transform.position, BottomLeftCorner.transform.rotation, BottomLeftCorner.transform);
                TextMeshProUGUI modText = modNameObj.gameObject.GetComponent<TextMeshProUGUI>();
                modText.text = "ZZComic";
                modText.enableWordWrapping = false;
                modNameObj.transform.Rotate(0f, 0f, 135f);
                modNameObj.transform.localScale = new Vector3(1f, 1f, 1f);
                modNameObj.transform.localPosition = new Vector3(-50f, -50f, 0f);
                modText.alignment = TextAlignmentOptions.Bottom;
                modText.alpha = 0.1f;
                modText.fontSize = 50;


                // find all the triangles
                GameObject front = allChildrenRecursive2.Where(obj => obj.gameObject.name == "Front").FirstOrDefault().gameObject;
                UnityEngine.UI.Image[] allChildrenRecursive3 = front.GetComponentsInChildren<UnityEngine.UI.Image>();
                foreach (UnityEngine.UI.Image img in allChildrenRecursive3)
                {
                    if (img.gameObject.name == "Triangle")
                    {
                        this.triangles.Add(img);
                        this.origTriangleColor = img.color;
                    }
                }
                Color.RGBToHSV(this.origTriangleColor, out this.origTriangleHue, out float s, out float v);

                this.flashUp = new List<bool>() { random.Next(0, 2) == 1, random.Next(0, 2) == 1, random.Next(0, 2) == 1, random.Next(0, 2) == 1 };
                for (int i = 0; i < 4; i++)
                {
                    this.ResetTriangleTimer(i);
                    this.GetNewFlashDuration(i);
                }
            }
            private void Update()
            {
                this.description.text = "<mspace=0.5em>" + ObfuscateString("Get a different random card each battle.") + "</mspace>";
                this.cardName.text = "<mspace=0.5em>" + ObfuscateString("RANDOM") + "</mspace>";

                this.UpdateAllTriangles();
                if (Time.time > this.artStartTime + this.artDelay)
                {
                    this.artStartTime = Time.time;
                    this.UpdateArt();
                }
                if (Time.time > this.artEffectStartTime + this.artEffectsDelay)
                {
                    this.artEffectStartTime = Time.time;
                    this.UpdateArtEffects();
                }
                if (Time.time > this.rotStartTime + this.rotDelay)
                {
                    this.rotStartTime = Time.time;
                    this.UpdateCardRotation();
                }
                if (Time.time > this.scaleStartTime + this.scaleDelay)
                {
                    this.scaleStartTime = Time.time;
                    this.UpdateScaling();
                }
            }
            private void UpdateArt()
            {

                // replace art
                if (this.artParent.gameObject.transform.GetChild(artNum) != null && this.artParent.gameObject.transform.GetChild(artNum).name != "BlockFront") { UnityEngine.GameObject.Destroy(this.artParent.gameObject.transform.GetChild(artNum).gameObject); }
                this.activeCards = ((ObservableCollection<CardInfo>)typeof(CardManager).GetField("activeCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).ToList();
                this.inactiveCards = (List<CardInfo>)typeof(CardManager).GetField("inactiveCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                this.allCards = activeCards.Concat(this.inactiveCards).ToArray();
                GameObject newart = GameObject.Instantiate(ModdingUtils.Utils.Cards.instance.DrawRandomCardWithCondition(this.allCards, null, null, null, null, null, null, null, null, (card, player, g, ga, d, h, gr, b, s) => (!(card.cardArt == null)) && (card.rarity == this.gameObject.GetComponent<CardInfo>().rarity)).cardArt, this.artParent.transform);
                newart.transform.localScale = new Vector3(1f, 1f, 1f);
                newart.transform.SetAsFirstSibling();
                this.art[artNum] = newart;
                this.artNum++;

                if (this.artNum >= this.maxArts)
                {
                    this.artNum = 0;
                }

                float time = Time.time;
            }
            private void UpdateArtEffects()
            {
                int artToChange1 = random.Next(0, this.art.Count);
                int artToChange2 = random.Next(0, this.art.Count);

                if (UnityEngine.Random.Range(0f, 100f) < GREYSCALE_CARD_CHANCE) greyscaleImage(art[artToChange1]);
                if (UnityEngine.Random.Range(0f, 100f) < ROTATE_IMAGE_CHANCE) rotateImage(art[artToChange2]);
            }

            private void UpdateCardRotation()
            {
                resetRotation(this.gameObject);
                if (UnityEngine.Random.Range(0f, 100f) < ROTATE_CARD_CHANCE)
                {
                    rotateCard(this.gameObject);
                }
            }

            private void UpdateScaling()
            {
                int artToChange = random.Next(0, this.art.Count);

                if (art[artToChange] == null) { return; }

                art[artToChange].transform.localScale = new Vector3(UnityEngine.Random.Range(0.25f, 2f), UnityEngine.Random.Range(0.25f, 2f), 1f);
            }

            private void UpdateAllTriangles()
            {
                float time = Time.time;
                for (int i = 0; i < 4; i++)
                {
                    float perc = (time - this.triangleTimers[i]) / this.triangleFlashDurations[i];
                    if (perc > 1f || perc < 0f)
                    {
                        this.ResetTriangleTimer(i);
                        this.GetNewFlashDuration(i);
                        this.flashUp[i] = !this.flashUp[i];
                        continue;
                    }
                    if (!this.flashUp[i])
                    {
                        perc = 1f - perc;
                    }
                    if (this.gameObject.GetComponent<CardInfo>().rarity == CardInfo.Rarity.Common)
                    {
                        this.triangles[i].color = Color.HSVToRGB(this.origTriangleHue, 0f, perc);
                    }
                    else
                    {
                        this.triangles[i].color = Color.HSVToRGB(this.origTriangleHue, perc, perc);
                    }
                }
            }
            private void GetNewFlashDuration(int idx)
            {
                this.triangleFlashDurations[idx] = (this.flashMinMax[1] - this.flashMinMax[0]) * (float)random.NextDouble() + this.flashMinMax[0];
            }
            private void ResetTriangleTimer(int idx)
            {
                this.triangleTimers[idx] = Time.time;
            }

            private void greyscaleImage(GameObject cardArt)
            {
                if (cardArt == null)
                {
                    return;
                }
                Image[] imageComponents = cardArt.GetComponentsInChildren<Image>();
                foreach (Image image in imageComponents)
                {
                    Color currentColor = image.color;
                    float grey = (currentColor.r + currentColor.g + currentColor.b) / 3f;
                    Color greyColor = new Color(grey, grey, grey, currentColor.a);
                    image.color = greyColor;
                }
            }

            private void rotateImage(GameObject cardArt)
            {
                if (cardArt == null)
                {
                    return;
                }
                cardArt.transform.Rotate(0f, 0f, UnityEngine.Random.Range(-180f, 180f));
            }
            private float currentCardRotation;
            private void rotateCard(GameObject cardArt)
            {
                currentCardRotation = UnityEngine.Random.Range(-180f, 180f);
                this.gameObject.transform.Rotate(0f, 0f, currentCardRotation);
            }

            private void resetRotation(GameObject cardArt)
            {
                this.gameObject.transform.Rotate(0f, 0f, -currentCardRotation);
                currentCardRotation = 0f;
            }
        }
    }

    public class RandomCardEffect : MonoBehaviour
    {
        private Player player;
        private static System.Random random = new System.Random();
        internal List<int> indeces = new List<int>() { };
        private float dH = 0.001f;
        private float sign = 1f;

        internal void UpdateIndecesOnRemove(Player player, CardInfo card, int idx)
        {
            if (player.playerID != this.player.playerID)
            {
                return;
            }

            List<int> newIndeces = new List<int>() { };
            foreach (int index in indeces)
            {
                if (index > idx)
                {
                    newIndeces.Add(index - 1);
                }
                else if (index == idx)
                {
                    // remove from list
                }    
                else
                {
                    newIndeces.Add(index);
                }
            }

            indeces = newIndeces;

        }

        internal string twoLetterCode
        {
            get
            {
                const string chars = "!?@#$%&<>";
                return new string(Enumerable.Repeat(chars, 2)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            set { }
        }
        void Start()
        {
            this.player = this.gameObject.GetComponent<Player>();


            ModdingUtils.Utils.Cards.instance.AddOnRemoveCallback(this.UpdateIndecesOnRemove);

        }
        void Update()
        {
            UnityEngine.UI.ProceduralImage.ProceduralImage cardSquare;
            indeces = indeces.Distinct().ToList(); // remove any duplicates
            foreach (int idx in indeces)
            {
                try
                {
                    cardSquare = ModdingUtils.Utils.CardBarUtils.instance.GetCardBarSquare(player, idx).transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.ProceduralImage.ProceduralImage>();
                }
                catch
                {
                    return;
                }
                Color.RGBToHSV(cardSquare.color, out float h, out float s, out float v);
                if (h + this.sign * this.dH > 1f || h + this.sign * this.dH < 0f)
                {
                    this.sign *= -1f;
                }
                Color newColor = Color.HSVToRGB(UnityEngine.Mathf.Clamp(h + this.sign * this.dH * (idx+1) / 3f, 0f, 1f), s, v);
                newColor.a = cardSquare.color.a;
                cardSquare.color = newColor;
            }
        }
    }
}
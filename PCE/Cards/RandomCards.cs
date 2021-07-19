using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using Photon.Pun;
using System.Reflection;
using HarmonyLib;
using System.Linq;
using PCE.Extensions;
using System.Collections;
using UnboundLib.Networking;
using TMPro;

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
    }
    public abstract class RandomCard : CustomCard
    {
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
        protected override string GetDescription()
        {
            return "...";
        }

        protected override CardInfoStat[] GetStats()
        {
            return null;
        }
        public override string GetModName()
        {
            return "PCE";
        }

        internal static void callback(CardInfo card)
        {
            card.gameObject.AddComponent<RandomCardVisualEffect>();

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

            private TextMeshProUGUI description;
            private TextMeshProUGUI cardName;
            private List<UnityEngine.UI.Image> triangles = new List<UnityEngine.UI.Image>() { };
            private List<float> triangleTimers = new List<float>() { -1f, -1f, -1f, -1f };
            private List<float> triangleFlashDurations = new List<float>() { 1f, 1f, 1f, 1f };
            private readonly List<float> flashMinMax = new List<float>() { 0.2f, 2f };
            private List<bool> flashUp = new List<bool>() { false, false, false, false };
            private void Start()
            {
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
                GameObject modNameObj = UnityEngine.GameObject.Instantiate(new GameObject("ExtraCardText", typeof(TextMeshProUGUI)), BottomLeftCorner.transform.position, BottomLeftCorner.transform.rotation, BottomLeftCorner.transform);
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
        }
    }

    public class RandomCardEffect : MonoBehaviour
    {
        private Player player;
        private static System.Random random = new System.Random();
        internal List<int> indeces = new List<int>() { };
        private readonly float dH = 0.001f;
        private float sign = 1f;

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
        }
        void Update()
        {
            foreach (int idx in indeces)
            {
                UnityEngine.UI.ProceduralImage.ProceduralImage cardSquare = Utils.CardBarUtils.instance.GetCardBarSquare(player, idx).transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.ProceduralImage.ProceduralImage>();
                Color.RGBToHSV(cardSquare.color, out float h, out float s, out float v);
                if (h + this.sign*this.dH > 1f || h + this.sign*this.dH < 0f)
                {
                    this.sign *= -1f;
                }
                Color newColor = Color.HSVToRGB(UnityEngine.Mathf.Clamp(h + this.sign * this.dH, 0f, 1f), s, v);
                newColor.a = cardSquare.color.a;
                cardSquare.color = newColor;
            }
        }


    }
    
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HarmonyLib;
using ModdingUtils.Extensions;

namespace PCE.Extensions
{
    // ADD FIELDS TO CARDINFO
    [Serializable]
    public class CardInfoAdditionalData
    {
        public bool isRandom;

        public CardInfoAdditionalData()
        {
            isRandom = false;
        }
    }
    public static class CardInfoExtension
    {
        public static readonly ConditionalWeakTable<CardInfo, CardInfoAdditionalData> data =
            new ConditionalWeakTable<CardInfo, CardInfoAdditionalData>();

        public static CardInfoAdditionalData GetAdditionalData(this CardInfo cardInfo)
        {
            return data.GetOrCreateValue(cardInfo);
        }

        public static void AddData(this CardInfo cardInfo, CardInfoAdditionalData value)
        {
            try
            {
                data.Add(cardInfo, value);
            }
            catch (Exception) { }
        }
    }
}

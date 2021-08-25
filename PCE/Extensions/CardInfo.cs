using System;
using System.Runtime.CompilerServices;

namespace PCE.Extensions
{
    // ADD FIELDS TO CARDINFO
    [Serializable]
    public class CardInfoAdditionalData
    {
        public bool isRandom;
        public bool isClassBlacklistCard;

        public CardInfoAdditionalData()
        {
            isRandom = false;
            isClassBlacklistCard = false;
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

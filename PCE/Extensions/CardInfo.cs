using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace PCE.Extensions
{
    // ADD FIELDS TO BLOCK
    [Serializable]
    public class CardInfoAdditionalData
    {
        public bool canBeReassigned;

        public CardInfoAdditionalData()
        {
            canBeReassigned = true;
        }
    }
    public static class CardInfoExtension
    {
        public static readonly ConditionalWeakTable<CardInfo, CardInfoAdditionalData> data =
            new ConditionalWeakTable<CardInfo, CardInfoAdditionalData>();

        public static CardInfoAdditionalData GetAdditionalData(this CardInfo block)
        {
            return data.GetOrCreateValue(block);
        }

        public static void AddData(this CardInfo block, CardInfoAdditionalData value)
        {
            try
            {
                data.Add(block, value);
            }
            catch (Exception) { }
        }
    }
}

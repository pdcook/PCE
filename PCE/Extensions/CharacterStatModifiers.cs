using System;
using System.Runtime.CompilerServices;
using HarmonyLib;


namespace PCE.Extensions
{
    // ADD FIELDS TO CHARACTERSTATMODIFIERS
    [Serializable]
    public class CharacterStatModifiersAdditionalData
    {
        public int thankyousirmayihaveanother;
        public float glare;
        public int remainingMulligans;
        public int mulligans;
        public int shuffles;
        public bool removeSpeedCompensation;
        public float piercingPerc;
        public int wraps;
        public int remainingWraps;
        public CharacterStatModifiersAdditionalData()
        {
            thankyousirmayihaveanother = 0;
            glare = 0f;
            remainingMulligans = 0;
            mulligans = 0;
            shuffles = 0;
            removeSpeedCompensation = false;
            piercingPerc = 0f;
            wraps = 0;
            remainingWraps = 0;
        }
    }
    public static class CharacterStatModifiersExtension
    {
        public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData> data =
            new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData>();

        public static CharacterStatModifiersAdditionalData GetAdditionalData(this CharacterStatModifiers characterstats)
        {
            return data.GetOrCreateValue(characterstats);
        }

        public static void AddData(this CharacterStatModifiers characterstats, CharacterStatModifiersAdditionalData value)
        {
            try
            {
                data.Add(characterstats, value);
            }
            catch (Exception) { }
        }

    }
    // reset additional CharacterStatModifiers when ResetStats is called
    [HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
    class CharacterStatModifiersPatchResetStats
    {
        private static void Prefix(CharacterStatModifiers __instance)
        {
            __instance.GetAdditionalData().thankyousirmayihaveanother = 0;
            __instance.GetAdditionalData().glare = 0f;
            __instance.GetAdditionalData().remainingMulligans = 0;
            __instance.GetAdditionalData().mulligans = 0;
            __instance.GetAdditionalData().shuffles = 0;
            __instance.GetAdditionalData().removeSpeedCompensation = false;
            __instance.GetAdditionalData().piercingPerc = 0f;
            __instance.GetAdditionalData().wraps = 0;
            __instance.GetAdditionalData().remainingWraps = 0;

        }
    }
}

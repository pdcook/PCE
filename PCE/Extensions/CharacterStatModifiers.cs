using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HarmonyLib;
using PCE.MonoBehaviours;
using PCE.RoundsEffects;
using UnityEngine;


namespace PCE.Extensions
{
    // ADD FIELDS TO CHARACTERSTATMODIFIERS
    [Serializable]
    public class CharacterStatModifiersAdditionalData
    {
        public float gravityMultiplierOnDoDamage;
        public float gravityDurationOnDoDamage;
        public float defaultGravityForce;
        public float defaultGravityExponent;
        public int murder;
        public int thankyousirmayihaveanother;
        public float glare;
        public HitEffect[] HitEffects;
        public WasHitEffect[] WasHitEffects;

        public CharacterStatModifiersAdditionalData()
        {
            gravityMultiplierOnDoDamage = 1f;
            gravityDurationOnDoDamage = 0f;
            murder = 0;
            thankyousirmayihaveanother = 0;
            glare = 0f;
            HitEffects = null;
            WasHitEffects = null;
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
    // get default gravity fields on Start()
    [HarmonyPatch(typeof(CharacterStatModifiers), "Start")]
    class CharacterStatModifiersPatchStart
    {
        private static void Postfix(CharacterStatModifiers __instance)
        {
            if (__instance != null && __instance.GetComponent<Player>() != null && __instance.GetComponent<Player>().GetComponent<Gravity>() != null)
            {
                __instance.GetAdditionalData().defaultGravityExponent = __instance.GetComponent<Player>().GetComponent<Gravity>().exponent;
                __instance.GetAdditionalData().defaultGravityForce = __instance.GetComponent<Player>().GetComponent<Gravity>().gravityForce;
            }

        }
    }
    // reset additional CharacterStatModifiers when ResetStats is called
    [HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
    class CharacterStatModifiersPatchResetStats
    {
        private static void Prefix(CharacterStatModifiers __instance)
        {
            __instance.GetAdditionalData().gravityMultiplierOnDoDamage = 1f;
            __instance.GetAdditionalData().gravityDurationOnDoDamage = 0f;
            __instance.GetAdditionalData().murder = 0;
            __instance.GetAdditionalData().thankyousirmayihaveanother = 0;
            __instance.GetAdditionalData().glare = 0f;
            __instance.GetAdditionalData().HitEffects = null;
            __instance.GetAdditionalData().WasHitEffects = null;

            Gravity gravity = __instance.GetComponent<Gravity>();
            gravity.gravityForce = __instance.GetAdditionalData().defaultGravityForce;
            gravity.exponent = __instance.GetAdditionalData().defaultGravityExponent;

        }
    }

    // update additional stats properly
    [HarmonyPatch(typeof(CharacterStatModifiers), "WasUpdated")]
    class CharacterStatModifiersPatchWasUpdated
    {
        private static void Postfix(CharacterStatModifiers __instance)
        {
            __instance.GetAdditionalData().HitEffects = __instance.GetComponentsInChildren<HitEffect>();
            __instance.GetAdditionalData().WasHitEffects = __instance.GetComponentsInChildren<WasHitEffect>();

        }
    }
}

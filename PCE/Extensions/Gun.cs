using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using UnboundLib;

namespace PCE.Extensions
{
    // ADD FIELDS TO GUN
    [Serializable]
    public class GunAdditionalData
    {
        public float minDistanceMultiplier;

        public GunAdditionalData()
        {
            minDistanceMultiplier = 1f;
        }
    }
    public static class GunExtension
    {
        public static readonly ConditionalWeakTable<Gun, GunAdditionalData> data =
            new ConditionalWeakTable<Gun, GunAdditionalData>();

        public static GunAdditionalData GetAdditionalData(this Gun gun)
        {
            return data.GetOrCreateValue(gun);
        }

        public static void AddData(this Gun gun, GunAdditionalData value)
        {
            try
            {
                data.Add(gun, value);
            }
            catch (Exception) { }
        }
    }
    // apply additional projectile stats
    [HarmonyPatch(typeof(Gun), "ApplyProjectileStats")]
    class GunPatchApplyProjectileStats
    {
        private static void Prefix(Gun __instance, GameObject obj, int numOfProj = 1, float damageM = 1f, float randomSeed = 0f)
        {
            if (__instance.GetAdditionalData().minDistanceMultiplier != 1f)
            {
                obj.GetOrAddComponent<ChangeDamageMultiplierAfterDistanceTravelled>().distance *= __instance.GetAdditionalData().minDistanceMultiplier;
            }
        }
    }
    // reset extra gun attributes when resetstats is called
    [HarmonyPatch(typeof(Gun), "ResetStats")]
    class GunPatchResetStats
    {
        private static void Prefix(Gun __instance)
        {
            __instance.GetAdditionalData().minDistanceMultiplier = 1f;
        }
    }
}

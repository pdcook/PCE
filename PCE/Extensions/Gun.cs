using System;
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
        public int fragmentationProjectiles;
        public int fireworkProjectiles;
        public int wraps;
        public bool allowStop;
        public bool canBeLaser;
        public bool isLaser;
        public GameObject laserGun;

        public GunAdditionalData()
        {
            fragmentationProjectiles = 0;
            fireworkProjectiles = 0;
            wraps = 0;
            allowStop = false;
            canBeLaser = false;
            isLaser = false;
            laserGun = null;
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
            MoveTransform component3 = obj.GetComponent<MoveTransform>();
            component3.allowStop = __instance.GetAdditionalData().allowStop;
        }
    }
    // reset extra gun attributes when resetstats is called
    [HarmonyPatch(typeof(Gun), "ResetStats")]
    class GunPatchResetStats
    {
        private static void Prefix(Gun __instance)
        {
            __instance.GetAdditionalData().fragmentationProjectiles = 0;
            __instance.GetAdditionalData().fireworkProjectiles = 0;
            __instance.GetAdditionalData().wraps = 0;
            __instance.GetAdditionalData().allowStop = false;
            __instance.GetAdditionalData().canBeLaser = false;
            __instance.GetAdditionalData().isLaser = false;
            __instance.GetAdditionalData().laserGun = null;
        }
    }
}

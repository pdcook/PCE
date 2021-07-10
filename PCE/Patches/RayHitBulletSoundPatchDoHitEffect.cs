using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HarmonyLib;
using PCE.MonoBehaviours;
using PCE.RoundsEffects;
using UnityEngine;
using PCE.Extensions;


namespace PCE.Patches
{
    [Serializable]
    [HarmonyPatch(typeof(RayHitBulletSound), "DoHitEffect")]
    class RayHitBulletSoundPatchDoHitEffect
    {
        static void Finalizer(RayHitBulletSound __instance, Exception __exception)
        {
            if (__exception is NullReferenceException)
            {
                UnityEngine.GameObject.Destroy(__instance.gameObject);
            }
        }
    }
}

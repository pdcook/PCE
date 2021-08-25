using System;
using HarmonyLib;


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

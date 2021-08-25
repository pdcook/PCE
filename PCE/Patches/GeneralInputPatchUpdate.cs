using System;
using HarmonyLib;
using UnityEngine;
using PCE.Extensions;


namespace PCE.Patches
{
    [Serializable]
    [HarmonyPatch(typeof(GeneralInput), "Update")]
    class GeneralInputPatchUpdate
    {
        // remove speed compensation
        private static void Postfix(GeneralInput __instance)
        {

            if (((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).stats.GetAdditionalData().removeSpeedCompensation && __instance.aimDirection != Vector3.zero)
            {
                    __instance.aimDirection -= Vector3.up * 0.13f / Mathf.Clamp(((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).weaponHandler.gun.projectileSpeed, 1f, 100f);
            }

        }
    }
}

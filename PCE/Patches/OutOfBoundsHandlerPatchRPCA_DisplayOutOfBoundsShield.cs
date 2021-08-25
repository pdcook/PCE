using System;
using HarmonyLib;
using UnityEngine;
using PCE.Extensions;


namespace PCE.Patches
{
    [Serializable]
    [HarmonyPatch(typeof(OutOfBoundsHandler), "RPCA_DisplayOutOfBoundsShield")]
    class OutOfBoundsHandlerPatchRPCA_DisplayOutOfBoundsShield
    {
        // patch for Masochist
        private static void Prefix(OutOfBoundsHandler __instance)
        {
            ((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).block.GetAdditionalData().timeOfLastSuccessfulBlock = Time.time;
        }
    }
}

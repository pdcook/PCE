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

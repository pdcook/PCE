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
    [HarmonyPatch(typeof(HealthHandler), "Revive")]
    class HealtHandlerPatchRevive
    {
        // patch for Mulligan
        private static void Prefix(HealthHandler __instance, bool isFullRevive = true)
        {
            if (isFullRevive)
            {
                ((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).stats.GetAdditionalData().remainingMulligans = ((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).stats.GetAdditionalData().mulligans;
            }

        }
    }
}

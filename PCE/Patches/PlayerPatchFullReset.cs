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
    // patch to reset cards and effects
    [Serializable]
    [HarmonyPatch(typeof(Player), "FullReset")]
    class PlayerPatchFullReset
    {
        private static void Prefix(Player __instance)
        {
            CustomEffects.DestroyAllEffects(__instance.gameObject);
            __instance.data.currentCards = new List<CardInfo> { };
        }
    }
}

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
    [HarmonyPatch(typeof(Player), "FullReset")]
    class PlayerPatchFullReset
    {
        private static void Prefix(Player __instance)
        {
            CustomEffects.ClearAllEffects(__instance.gameObject);
        }
    }
}

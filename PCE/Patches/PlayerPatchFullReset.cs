using System;
using HarmonyLib;
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
        }
    }
}

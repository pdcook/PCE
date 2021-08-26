using System;
using HarmonyLib;
using PCE.Extensions;


namespace PCE.Patches
{
    [Serializable]
    [HarmonyPatch(typeof(HealthHandler), "Revive")]
    class HealtHandlerPatchRevive
    {
        // patch for Mulligan and PacPlayer
        private static void Prefix(HealthHandler __instance, bool isFullRevive = true)
        {
            if (isFullRevive)
            {
                ((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).stats.GetAdditionalData().remainingMulligans = ((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).stats.GetAdditionalData().mulligans;
                ((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).stats.GetAdditionalData().remainingWraps = ((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).stats.GetAdditionalData().wraps;
            }

        }
    }
}

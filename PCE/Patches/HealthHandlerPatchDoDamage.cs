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
    [HarmonyPatch(typeof(HealthHandler), "DoDamage")]
    class HealtHandlerPatchDoDamage
    {
        // patch for Mulligan
        private static void Prefix(HealthHandler __instance, Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, ref bool lethal, bool ignoreBlock)
        {

            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
			Player player = data.player;
			if (!data.isPlaying)
			{
				return;
			}
			if (data.dead)
			{
				return;
			}
			if (__instance.isRespawning)
			{
				return;
			}

			if (((CharacterStatModifiers)Traverse.Create(__instance).Field("stats").GetValue()).GetAdditionalData().mulligan)
            {
				lethal = false;
			}
        }
    }
}

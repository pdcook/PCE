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
        // patch for Mulligan and Masochist
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
			if (data.block.IsBlocking() && !ignoreBlock)
            {
				// reset time since successful block

				data.block.GetAdditionalData().timeOfLastSuccessfulBlock = Time.time;

				return;
            }

			// if the damage is lethal and would've killed the player, check for mulligans
			if (lethal && data.health < damage.magnitude && data.stats.GetAdditionalData().remainingMulligans > 0)
            {
				// if lethal, whould've killed, and there are mulligans available, use a mulligan
				if (player.GetComponent<MulliganEffect>() != null)
				{ player.GetComponent<MulliganEffect>().UseMulligan(); }
                else { return; }

				lethal = false;
			}
        }
    }
}

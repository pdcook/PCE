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
	[HarmonyPatch(typeof(HealthHandler), "CallTakeDamage")]
    class HealtHandlerPatchCallTakeDamage
    {
        // patch for Masochist
        private static void Prefix(HealthHandler __instance, Vector2 damage)
        {

            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
			Player player = data.player;
			if (damage == Vector2.zero)
            {
				return;
            }
			
			if (data.block.IsBlocking())
            {
				// reset time since successful block

				data.block.GetAdditionalData().timeOfLastSuccessfulBlock = Time.time;

				return;
            }
        }
    }
}

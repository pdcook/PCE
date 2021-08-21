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
	[HarmonyPatch(typeof(HealthHandler), "TakeDamage", new Type[]{typeof(Vector2), typeof(Vector2), typeof(Color), typeof(GameObject), typeof(Player), typeof(bool), typeof(bool)})]
    class HealtHandlerPatchTakeDamage
    {
        // patch for Masochist
        private static void Prefix(HealthHandler __instance, Vector2 damage, Vector2 position, Color dmgColor, GameObject damagingWeapon, Player damagingPlayer, bool lethal, bool ignoreBlock)
        {

            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
			Player player = data.player;
			if (damage == Vector2.zero)
            {
				return;
            }
			if (!data.isPlaying)
			{
				return;
			}
			if (!(bool)Traverse.Create(data.playerVel).Field("simulated").GetValue())
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
        }
    }
}

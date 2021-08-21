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
    [HarmonyPatch(typeof(Block), "blocked")]
    class BlockPatchblocked
    {
        private static void Prefix(Block __instance, GameObject projectile, Vector3 forward, Vector3 hitPos)
        {
            bool destroy = false;

            ProjectileHit proj = projectile.GetComponent<ProjectileHit>();
            HealthHandler healthHandler = (HealthHandler)Traverse.Create(__instance).Field("health").GetValue();

            // apply piercing
            if (proj.ownPlayer.data.stats.GetAdditionalData().piercingPerc > 0f)
            {
                Vector2 damage = (proj.bulletCanDealDeamage ? proj.damage : 1f) * proj.ownPlayer.data.stats.GetAdditionalData().piercingPerc * forward.normalized;
                healthHandler.TakeDamage(damage, hitPos, proj.projectileColor, proj.ownWeapon, proj.ownPlayer, true, true);

                destroy = true;
            }

            if (destroy)
            {                 
                // destroy the bullet
                UnityEngine.GameObject.Destroy(projectile);
            }
        }
        private static void Postfix(Block __instance)
        {
            __instance.GetAdditionalData().timeOfLastSuccessfulBlock = Time.time;
        }
    }
}

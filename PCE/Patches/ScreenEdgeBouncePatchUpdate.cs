using System;
using HarmonyLib;
using PCE.Extensions;
using PCE.MonoBehaviours;
using System.Reflection;

namespace PCE.Patches
{
    // patch for pac-bulllets to ignore screen edge bounces until after all warps have been used
    [Serializable]
    [HarmonyPatch(typeof(ScreenEdgeBounce), "Update")]
    class ScreenEdgeBouncePatchUpdate
    {
        private static bool Prefix(ScreenEdgeBounce __instance)
        {
            ProjectileHit proj = (ProjectileHit)Traverse.Create(__instance).Field("projHit").GetValue();
            if (proj.gameObject.GetComponentInChildren<PacBulletEffect>() != null)
            {
                PacBulletEffect effect = proj.gameObject.GetComponentInChildren<PacBulletEffect>();
                if (effect.numWraps > effect.wraps)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

using System;
using System.Collections;
using System.Text;
using System.Runtime.CompilerServices;
using HarmonyLib;
using PCE.MonoBehaviours;
using PCE.RoundsEffects;
using UnityEngine;
using PCE.Extensions;
using Sonigon;
using Photon.Pun;
using System.Reflection;

namespace PCE.Patches
{
	[Serializable]
	[HarmonyPatch(typeof(ProjectileHit), "Start")]
	class ProjectileHitPatchStart
	{
		private static void Prefix(ProjectileHit __instance)
        {
			__instance.GetAdditionalData().startTime = Time.time;
        }
	}
	[Serializable]
    [HarmonyPatch(typeof(ProjectileHit), "RPCA_DoHit")]
    class ProjectileHitPatchRPCA_DoHit
    {
		// prefix to prevent unwanted bullet on bullet collisions
		private static bool Prefix(ProjectileHit __instance, Vector2 hitPoint, Vector2 hitNormal, Vector2 vel, int viewID, int colliderID, bool wasBlocked)
        {
			if (__instance.ownPlayer != null && __instance.ownPlayer.GetComponent<Holding>().holdable.GetComponent<Gun>() != null)
            {
				if (Time.time < __instance.GetAdditionalData().startTime + __instance.GetAdditionalData().inactiveDelay || Time.time < __instance.GetAdditionalData().startTime + __instance.ownPlayer.GetComponent<Holding>().holdable.GetComponent<Gun>().GetAdditionalData().inactiveDelay)
				{
					return false; // don't run DoHit if the initial delay is not over
				}

			}
			return true;
        }
		// postfix to run HitEffect s and WasHitEffect s and HitSurfaceEffect s
		private static void Postfix(ProjectileHit __instance, Vector2 hitPoint, Vector2 hitNormal, Vector2 vel, int viewID, int colliderID, bool wasBlocked)
        {
			HitInfo hitInfo = new HitInfo();
			hitInfo.collider = null;
			if (viewID != -1)
			{
				PhotonView photonView = PhotonNetwork.GetPhotonView(viewID);
				hitInfo.collider = photonView.GetComponentInChildren<Collider2D>();
				hitInfo.transform = photonView.transform;
			}
			else if (colliderID != -1)
			{
				hitInfo.collider = MapManager.instance.currentMap.Map.GetComponentsInChildren<Collider2D>()[colliderID];
				hitInfo.transform = hitInfo.collider.transform;
			}

			// if the bullet hit a collider, run the hit effects
			if (hitInfo.collider && __instance.gameObject.GetComponentInChildren<StopRecursion>() == null)
            {
				HitSurfaceEffect[] hitSurfaceEffects = __instance.ownPlayer.data.stats.GetAdditionalData().HitSurfaceEffects;
				foreach (HitSurfaceEffect hitSurfaceEffect in hitSurfaceEffects)
				{
					hitSurfaceEffect.Hit(hitPoint, hitNormal, vel);
				}
            }				

			HealthHandler healthHandler = null;
			if (hitInfo.transform)
			{
				healthHandler = hitInfo.transform.GetComponent<HealthHandler>();
			}

			if (healthHandler == null) { return; }

			// if there's a healthHandler then try to run the HitEffects
			CharacterStatModifiers characterStatModifiers = healthHandler.GetComponent<CharacterStatModifiers>();
			if (characterStatModifiers == null) { return; }

			Player damagingPlayer = __instance.ownPlayer;
			Player damagedPlayer = ((CharacterData)Traverse.Create(characterStatModifiers).Field("data").GetValue()).player;

			bool selfDamage = damagingPlayer != null && damagingPlayer.transform.root == damagedPlayer.transform;

			WasHitEffect[] wasHitEffects = characterStatModifiers.GetAdditionalData().WasHitEffects;
			foreach(WasHitEffect wasHitEffect in wasHitEffects)
            {
				wasHitEffect.WasDealtDamage(__instance.transform.forward * __instance.damage * __instance.dealDamageMultiplierr, selfDamage);
            }

			HitEffect[] hitEffects = damagingPlayer.data.stats.GetAdditionalData().HitEffects;
			foreach (HitEffect hitEffect in hitEffects)
			{
				hitEffect.DealtDamage(__instance.transform.forward * __instance.damage * __instance.dealDamageMultiplierr, selfDamage, damagedPlayer);
			}

			return;

		}
	}
}

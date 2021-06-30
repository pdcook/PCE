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
    [HarmonyPatch(typeof(ProjectileHit), "RPCA_DoHit")]
    class ProjectileHitPatchRPCA_DotHit
    {
		// postfix to run HitEffect s and WasHitEffect s
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

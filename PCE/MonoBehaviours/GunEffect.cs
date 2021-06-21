using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;
using UnityEngine.UI;
using Sonigon;

namespace PCE.MonoBehaviours
{

    public class GunEffect : MonoBehaviour
    {

		private Gun originalGun = null;
		//private GunAmmo originalGunAmmo = null;
		private GunAmmoStats originalGunAmmoStats;
		
		private Player player;
		private GunAmmo playersGunAmmo; 
		private Gun gunToSet = null;
		//private GunAmmo gunAmmoToSet = null;
		private GunAmmoStats gunAmmoStatsToSet;

		void Awake()
		{

			this.originalGun = this.gameObject.AddComponent<Gun>();
			//this.originalGunAmmo = this.gameObject.AddComponent<GunAmmo>();

            this.player = this.gameObject.GetComponent<Player>();
			this.playersGunAmmo = this.player.data.weaponHandler.gun.GetComponentInChildren<GunAmmo>();


			GunEffect.CopyGunStats(this.player.data.weaponHandler.gun, this.originalGun);
			this.originalGunAmmoStats = GunEffect.GetGunAmmoStats(this.playersGunAmmo);
			//GunEffect.CopyGunAmmoStats(this.playersGunAmmo, this.originalGunAmmo);		
		}

		void Start()
        {

			if (this.gunToSet != null)
			{
				GunEffect.CopyGunStats(this.gunToSet, this.player.data.weaponHandler.gun);
				GunEffect.ApplyGunAmmoStats(this.gunAmmoStatsToSet, this.playersGunAmmo);

			}

		}
		void Update()
        {

        }
		void LateUpdate()
        {

		}
        public void OnDestroy()
        {
			// reset gun and gunAmmo back to original
			if (this.originalGun != null)
			{
				GunEffect.CopyGunStats(this.originalGun, this.player.data.weaponHandler.gun);
				GunEffect.ApplyGunAmmoStats(this.originalGunAmmoStats, this.playersGunAmmo);

			}
			//if (this.originalGunAmmo != null)
			//{
			//	GunEffect.CopyGunAmmoStats(this.originalGunAmmo, this.playersGunAmmo);
			//}
			// destroy the new gun, gunAmmo, and the copies of the originals
			if (this.gunToSet != null) { Destroy(this.gunToSet); }
			//if (this.gunAmmoToSet != null) { Destroy(this.gunAmmoToSet); }
			if (this.originalGun != null) { Destroy(this.originalGun); }
			//if (this.originalGunAmmo != null) { Destroy(this.originalGunAmmo); }

		}
		public void Destroy()
		{
			UnityEngine.Object.Destroy(this);
		}
		private void SetGun(Gun gun)
		{
			this.gunToSet = gun;
		}
		//public void SetGunAmmo(GunAmmo gunAmmo)
        //{
		//	this.gunAmmoToSet = gunAmmo;
        //}
		private void SetGunAmmoStats(GunAmmoStats gunAmmoStats)
        {
			this.gunAmmoStatsToSet = gunAmmoStats;
        }
		public void SetGunAndGunAmmoStats(Gun gun, GunAmmoStats gunAmmoStats)
        {
			this.SetGun(gun);
			this.SetGunAmmoStats(gunAmmoStats);
		}
		public static void CopyGunStats(Gun copyFromGun, Gun copyToGun)
		{
			
			copyToGun.ammo = copyFromGun.ammo;
			copyToGun.ammoReg = copyFromGun.ammoReg;
			copyToGun.attackID = copyFromGun.attackID;
			copyToGun.attackSpeed = copyFromGun.attackSpeed;
			copyToGun.attackSpeedMultiplier = copyFromGun.attackSpeedMultiplier;
			copyToGun.bodyRecoil = copyFromGun.bodyRecoil;
			copyToGun.bulletDamageMultiplier = copyFromGun.bulletDamageMultiplier;
			copyToGun.bulletPortal = copyFromGun.bulletPortal;
			copyToGun.bursts = copyFromGun.bursts;
			copyToGun.chargeDamageMultiplier = copyFromGun.chargeDamageMultiplier;
			copyToGun.chargeEvenSpreadTo = copyFromGun.chargeEvenSpreadTo;
			copyToGun.chargeNumberOfProjectilesTo = copyFromGun.chargeNumberOfProjectilesTo;
			copyToGun.chargeRecoilTo = copyFromGun.chargeRecoilTo;
			copyToGun.chargeSpeedTo = copyFromGun.chargeSpeedTo;
			copyToGun.chargeSpreadTo = copyFromGun.chargeSpreadTo;
			copyToGun.cos = copyFromGun.cos;
			copyToGun.currentCharge = copyFromGun.currentCharge;
			copyToGun.damage = copyFromGun.damage;
			copyToGun.damageAfterDistanceMultiplier = copyFromGun.damageAfterDistanceMultiplier;
			copyToGun.defaultCooldown = copyFromGun.defaultCooldown;
			copyToGun.dmgMOnBounce = copyFromGun.dmgMOnBounce;
			copyToGun.dontAllowAutoFire = copyFromGun.dontAllowAutoFire;
			copyToGun.drag = copyFromGun.drag;
			copyToGun.dragMinSpeed = copyFromGun.dragMinSpeed;
			copyToGun.evenSpread = copyFromGun.evenSpread;
			copyToGun.explodeNearEnemyDamage = copyFromGun.explodeNearEnemyDamage;
			copyToGun.explodeNearEnemyRange = copyFromGun.explodeNearEnemyRange;
			copyToGun.forceSpecificAttackSpeed = copyFromGun.forceSpecificAttackSpeed;
			copyToGun.forceSpecificShake = copyFromGun.forceSpecificShake;
			copyToGun.gravity = copyFromGun.gravity;
			copyToGun.hitMovementMultiplier = copyFromGun.hitMovementMultiplier;
			//copyToGun.holdable = copyFromGun.holdable;
			copyToGun.ignoreWalls = copyFromGun.ignoreWalls;
			copyToGun.isProjectileGun = copyFromGun.isProjectileGun;
			copyToGun.isReloading = copyFromGun.isReloading;
			copyToGun.knockback = copyFromGun.knockback;
			copyToGun.lockGunToDefault = copyFromGun.lockGunToDefault;
			copyToGun.multiplySpread = copyFromGun.multiplySpread;
			copyToGun.numberOfProjectiles = copyFromGun.numberOfProjectiles;
			copyToGun.objectsToSpawn = copyFromGun.objectsToSpawn;
			copyToGun.overheatMultiplier = copyFromGun.overheatMultiplier;
			copyToGun.percentageDamage = copyFromGun.percentageDamage;
			copyToGun.player = copyFromGun.player;
			copyToGun.projectielSimulatonSpeed = copyFromGun.projectielSimulatonSpeed;
			copyToGun.projectileColor = copyFromGun.projectileColor;
			copyToGun.projectiles = copyFromGun.projectiles;
			copyToGun.projectileSize = copyFromGun.projectileSize;
			copyToGun.projectileSpeed = copyFromGun.projectileSpeed;
			copyToGun.randomBounces = copyFromGun.randomBounces;
			copyToGun.recoil = copyFromGun.recoil;
			copyToGun.recoilMuiltiplier = copyFromGun.recoilMuiltiplier;
			copyToGun.reflects = copyFromGun.reflects;
			copyToGun.reloadTime = copyFromGun.reloadTime;
			copyToGun.reloadTimeAdd = copyFromGun.reloadTimeAdd;
			copyToGun.shake = copyFromGun.shake;
			copyToGun.shakeM = copyFromGun.shakeM;
			copyToGun.ShootPojectileAction = copyFromGun.ShootPojectileAction;
			//copyToGun.shootPosition = copyFromGun.shootPosition;
			copyToGun.sinceAttack = copyFromGun.sinceAttack;
			copyToGun.size = copyFromGun.size;
			copyToGun.slow = copyFromGun.slow;
			copyToGun.smartBounce = copyFromGun.smartBounce;
			copyToGun.soundDisableRayHitBulletSound = copyFromGun.soundDisableRayHitBulletSound;
			copyToGun.soundGun = copyFromGun.soundGun;
			copyToGun.soundImpactModifier = copyFromGun.soundImpactModifier;
			copyToGun.soundShotModifier = copyFromGun.soundShotModifier;
			copyToGun.spawnSkelletonSquare = copyFromGun.spawnSkelletonSquare;
			copyToGun.speedMOnBounce = copyFromGun.speedMOnBounce;
			copyToGun.spread = copyFromGun.spread;
			copyToGun.teleport = copyFromGun.teleport;
			copyToGun.timeBetweenBullets = copyFromGun.timeBetweenBullets;
			copyToGun.timeToReachFullMovementMultiplier = copyFromGun.timeToReachFullMovementMultiplier;
			copyToGun.unblockable = copyFromGun.unblockable;
			copyToGun.useCharge = copyFromGun.useCharge;
			copyToGun.waveMovement = copyFromGun.waveMovement;

			Traverse.Create(copyToGun).Field("attackAction").SetValue((Action)Traverse.Create(copyFromGun).Field("attackAction").GetValue());
			//Traverse.Create(copyToGun).Field("gunAmmo").SetValue((GunAmmo)Traverse.Create(copyFromGun).Field("gunAmmo").GetValue());
			Traverse.Create(copyToGun).Field("gunID").SetValue((int)Traverse.Create(copyFromGun).Field("gunID").GetValue());
			Traverse.Create(copyToGun).Field("spreadOfLastBullet").SetValue((float)Traverse.Create(copyFromGun).Field("spreadOfLastBullet").GetValue());
			




		}
		/*
		public static void CopyGunAmmoStats(GunAmmo copyFromGunAmmo, GunAmmo copyToGunAmmo)
        {
			copyToGunAmmo.maxAmmo = copyFromGunAmmo.maxAmmo;
			copyToGunAmmo.reloadTime = copyFromGunAmmo.reloadTime;
			copyToGunAmmo.reloadTimeMultiplier = copyFromGunAmmo.reloadTimeMultiplier;
			copyToGunAmmo.reloadTimeAdd = copyFromGunAmmo.reloadTimeAdd;
			copyToGunAmmo.ammoReg = copyFromGunAmmo.ammoReg;
			//copyToGunAmmo.populate = copyFromGunAmmo.populate;
			//copyToGunAmmo.reloadAnim = copyFromGunAmmo.reloadAnim;
			//Traverse.Create(copyToGunAmmo).Field("gun").SetValue((Gun)Traverse.Create(copyFromGunAmmo).Field("gun").GetValue());
			//Traverse.Create(copyToGunAmmo).Field("reloadRing").SetValue((Image)Traverse.Create(copyFromGunAmmo).Field("reloadRing").GetValue());
			copyToGunAmmo.soundReloadComplete = copyFromGunAmmo.soundReloadComplete;
			Traverse.Create(copyToGunAmmo).Field("soundReloadInProgressIntensity").SetValue((SoundParameterIntensity)Traverse.Create(copyFromGunAmmo).Field("soundReloadInProgressIntensity").GetValue());
			copyToGunAmmo.soundReloadInProgressLoop = copyFromGunAmmo.soundReloadInProgressLoop;
			Traverse.Create(copyToGunAmmo).Field("soundReloadInProgressPlaying").SetValue((bool)Traverse.Create(copyFromGunAmmo).Field("soundReloadInProgressPlaying").GetValue());
			Traverse.Create(copyToGunAmmo).Field("soundReloadTime").SetValue((float)Traverse.Create(copyFromGunAmmo).Field("soundReloadTime").GetValue());
			
			copyToGunAmmo.cooldownRing = copyFromGunAmmo.cooldownRing;
			Traverse.Create(copyToGunAmmo).Field("currentAmmo").SetValue((int)Traverse.Create(copyFromGunAmmo).Field("currentAmmo").GetValue());
			Traverse.Create(copyToGunAmmo).Field("currentRegCounter").SetValue((float)Traverse.Create(copyFromGunAmmo).Field("currentRegCounter").GetValue());
			Traverse.Create(copyToGunAmmo).Field("freeReloadCounter").SetValue((float)Traverse.Create(copyFromGunAmmo).Field("freeReloadCounter").GetValue());
			Traverse.Create(copyToGunAmmo).Field("lastMaxAmmo").SetValue((int)Traverse.Create(copyFromGunAmmo).Field("lastMaxAmmo").GetValue());
			Traverse.Create(copyToGunAmmo).Field("reloadCounter").SetValue((float)Traverse.Create(copyFromGunAmmo).Field("reloadCounter").GetValue());
		}*/

		public static GunAmmoStats GetGunAmmoStats(GunAmmo gunAmmo)
        {
			GunAmmoStats gunAmmoStats = new GunAmmoStats();

			gunAmmoStats.maxAmmo = gunAmmo.maxAmmo;
			gunAmmoStats.reloadTime = gunAmmo.reloadTime;
			gunAmmoStats.reloadTimeMultiplier = gunAmmo.reloadTimeMultiplier;
			gunAmmoStats.reloadTimeAdd = gunAmmo.reloadTimeAdd;

			return gunAmmoStats;
        }
		private static void ApplyGunAmmoStats(GunAmmoStats gunAmmoStats, GunAmmo gunAmmo)
        {
			gunAmmo.maxAmmo = gunAmmoStats.maxAmmo;
			gunAmmo.reloadTime = gunAmmoStats.reloadTime;
			gunAmmo.reloadTimeMultiplier = gunAmmoStats.reloadTimeMultiplier;
			gunAmmo.reloadTimeAdd = gunAmmoStats.reloadTimeAdd;
        }
	}

	public struct GunAmmoStats
    {
		public int maxAmmo;
		public float reloadTime;
		public float reloadTimeMultiplier;
		public float reloadTimeAdd;
    }
}

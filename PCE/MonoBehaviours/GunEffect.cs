using System;using System.Collections.Generic;using System.Text;using UnityEngine;using UnboundLib;using HarmonyLib;using System.Reflection;using PCE.MonoBehaviours;namespace PCE.MonoBehaviours
{

    public class GunEffect : MonoBehaviour
    {

		private Gun originalGun;
		private GunAmmo originalGunAmmo;
		
		private Player player;
		private GunAmmo playersGunAmmo; 
		private Gun gunToSet = null;
		private GunAmmo gunAmmoToSet = null;

		void Awake()
		{

			this.originalGun = this.gameObject.AddComponent<Gun>();
			//this.originalGunAmmo = this.gameObject.AddComponent<GunAmmo>();

            this.player = this.gameObject.GetComponent<Player>();
			//this.playersGunAmmo = (GunAmmo)Traverse.Create(this.player.data.weaponHandler.gun).Field("gunAmmo").GetValue();
			GunEffect.CopyGunStats(this.player.data.weaponHandler.gun, this.originalGun);
			//GunEffect.CopyGunAmmoStats(this.playersGunAmmo, this.originalGunAmmo);
		}

        void Start()
        {
			if (this.gunToSet != null)
			{
				GunEffect.CopyGunStats(this.gunToSet, this.player.data.weaponHandler.gun);
			}
			if (this.gunAmmoToSet != null)
			{
				//GunEffect.CopyGunAmmoStats(this.gunAmmoToSet, this.playersGunAmmo);
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
			GunEffect.CopyGunStats(this.originalGun, this.player.data.weaponHandler.gun);
			//GunEffect.CopyGunAmmoStats(this.originalGunAmmo, this.playersGunAmmo);
			// destroy the new gun, gunAmmo, and the copies of the originals
			Destroy(this.gunToSet);
			//Destroy(this.gunAmmoToSet);
			Destroy(this.originalGun);
			//Destroy(this.originalGunAmmo);
		}
		public void Destroy()
		{
			UnityEngine.Object.Destroy(this);
		}
		public void SetGun(Gun gun)
		{
			this.gunToSet = gun;
		}
		public void SetGunAmmo(GunAmmo gunAmmo)
        {
			this.gunAmmoToSet = gunAmmo;
        }
		public static void CopyGunStats(Gun copyFromGun, Gun copyToGun)
		{
			copyToGun.unblockable = copyFromGun.unblockable;
			copyToGun.ignoreWalls = copyFromGun.ignoreWalls;
			copyToGun.damage = copyFromGun.damage;
			copyToGun.size += copyFromGun.size;
			copyToGun.chargeDamageMultiplier = copyFromGun.chargeDamageMultiplier;
			copyToGun.knockback = copyFromGun.knockback;
			copyToGun.projectileSpeed = copyFromGun.projectileSpeed;
			copyToGun.projectielSimulatonSpeed = copyFromGun.projectielSimulatonSpeed;
			copyToGun.gravity = copyFromGun.gravity;
			copyToGun.multiplySpread = copyFromGun.multiplySpread;
			copyToGun.attackSpeed = copyFromGun.attackSpeed;
			copyToGun.bodyRecoil = copyFromGun.recoilMuiltiplier;
			copyToGun.speedMOnBounce = copyFromGun.speedMOnBounce;
			copyToGun.dmgMOnBounce = copyFromGun.dmgMOnBounce;
			copyToGun.bulletDamageMultiplier = copyFromGun.bulletDamageMultiplier;
			copyToGun.spread = copyFromGun.spread;
			copyToGun.drag = copyFromGun.drag;
			copyToGun.timeBetweenBullets = copyFromGun.timeBetweenBullets;
			copyToGun.dragMinSpeed = copyFromGun.dragMinSpeed;
			copyToGun.evenSpread = copyFromGun.evenSpread;
			copyToGun.numberOfProjectiles = copyFromGun.numberOfProjectiles;
			copyToGun.reflects = copyFromGun.reflects;
			copyToGun.smartBounce = copyFromGun.smartBounce;
			copyToGun.bulletPortal = copyFromGun.bulletPortal;
			copyToGun.randomBounces = copyFromGun.randomBounces;
			copyToGun.bursts = copyFromGun.bursts;
			copyToGun.slow = copyFromGun.slow;
			copyToGun.overheatMultiplier = copyFromGun.overheatMultiplier;
			copyToGun.projectileSize = copyFromGun.projectileSize;
			copyToGun.percentageDamage = copyFromGun.percentageDamage;
			copyToGun.damageAfterDistanceMultiplier = copyFromGun.damageAfterDistanceMultiplier;
			copyToGun.timeToReachFullMovementMultiplier = copyFromGun.timeToReachFullMovementMultiplier;
			copyToGun.cos = copyFromGun.cos;
			copyToGun.dontAllowAutoFire = copyFromGun.dontAllowAutoFire;
			copyToGun.destroyBulletAfter = copyFromGun.destroyBulletAfter;
			copyToGun.chargeSpreadTo = copyFromGun.chargeSpreadTo;
			copyToGun.chargeSpeedTo = copyFromGun.chargeSpeedTo;
			copyToGun.chargeEvenSpreadTo = copyFromGun.chargeEvenSpreadTo;
			copyToGun.chargeNumberOfProjectilesTo = copyFromGun.chargeNumberOfProjectilesTo;
			copyToGun.chargeRecoilTo = copyFromGun.chargeRecoilTo;
			copyToGun.projectileColor = copyFromGun.projectileColor;

			Traverse.Create(copyToGun).Field("forceShootDir").SetValue((Vector3)Traverse.Create(copyFromGun).Field("forceShootDir").GetValue());

			copyToGun.soundGun = copyFromGun.soundGun;

			copyToGun.objectsToSpawn = copyFromGun.objectsToSpawn;
			copyToGun.useCharge = copyFromGun.useCharge;
		}
		public static void CopyGunAmmoStats(GunAmmo copyFromGunAmmo, GunAmmo copyToGunAmmo)
        {
			copyToGunAmmo.maxAmmo = copyFromGunAmmo.maxAmmo;
			copyToGunAmmo.reloadTime = copyFromGunAmmo.reloadTime;
			copyToGunAmmo.reloadTimeMultiplier = copyFromGunAmmo.reloadTimeMultiplier;
			copyToGunAmmo.reloadTime = copyFromGunAmmo.reloadTimeAdd;
			copyToGunAmmo.ammoReg = copyFromGunAmmo.ammoReg;
        }
	}
}

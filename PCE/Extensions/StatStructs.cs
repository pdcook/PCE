using System;
using UnityEngine;
using SoundImplementation;
using HarmonyLib;

namespace PCE.Extensions
{
	public struct GunAmmoStats
	{
		public int maxAmmo;
		public float reloadTime;
		public float reloadTimeMultiplier;
		public float reloadTimeAdd;

		public void GetStatsFromGunAmmo(GunAmmo gunAmmo)
        {
			this.maxAmmo = gunAmmo.maxAmmo;
			this.reloadTime = gunAmmo.reloadTime;
			this.reloadTimeMultiplier = gunAmmo.reloadTimeMultiplier;
			this.reloadTimeAdd = gunAmmo.reloadTimeAdd;
		}
		public static GunAmmoStats GetGunAmmoStats(GunAmmo gunAmmo)
		{
			GunAmmoStats gunAmmoStats = new GunAmmoStats();

			gunAmmoStats.maxAmmo = gunAmmo.maxAmmo;
			gunAmmoStats.reloadTime = gunAmmo.reloadTime;
			gunAmmoStats.reloadTimeMultiplier = gunAmmo.reloadTimeMultiplier;
			gunAmmoStats.reloadTimeAdd = gunAmmo.reloadTimeAdd;

			return gunAmmoStats;
		}
		public static void ApplyGunAmmoStats(GunAmmoStats gunAmmoStats, GunAmmo gunAmmo)
		{
			gunAmmo.maxAmmo = gunAmmoStats.maxAmmo;
			gunAmmo.reloadTime = gunAmmoStats.reloadTime;
			gunAmmo.reloadTimeMultiplier = gunAmmoStats.reloadTimeMultiplier;
			gunAmmo.reloadTimeAdd = gunAmmoStats.reloadTimeAdd;
		}
		public void ApplyGunAmmoStats(GunAmmo gunAmmo)
		{
			gunAmmo.maxAmmo = this.maxAmmo;
			gunAmmo.reloadTime = this.reloadTime;
			gunAmmo.reloadTimeMultiplier = this.reloadTimeMultiplier;
			gunAmmo.reloadTimeAdd = this.reloadTimeAdd;
		}

	}

	public struct GunStats
    {
		public int ammo;
		public float ammoReg;
		public int attackID;
		public float attackSpeed;
		public float attackSpeedMultiplier;
		public float bodyRecoil;
		public float bulletDamageMultiplier;
		public int bulletPortal;
		public int bursts;
		public float chargeDamageMultiplier;
		public float chargeEvenSpreadTo;
		public float chargeNumberOfProjectilesTo;
		public float chargeRecoilTo;
		public float chargeSpeedTo;
		public float chargeSpreadTo;
		public float cos;
		public float currentCharge;
		public float damage;
		public float damageAfterDistanceMultiplier;
		public float defaultCooldown;
		public float dmgMOnBounce;
		public bool dontAllowAutoFire;
		public float drag;
		public float dragMinSpeed;
		public float evenSpread;
		public float explodeNearEnemyDamage;
		public float explodeNearEnemyRange;
		public float forceSpecificAttackSpeed;
		public bool forceSpecificShake;
		public float gravity;
		public float hitMovementMultiplier;
		//public Holdable holdable;
		public bool ignoreWalls;
		public bool isProjectileGun;
		public bool isReloading;
		public float knockback;
		public bool lockGunToDefault;
		public float multiplySpread;
		public int numberOfProjectiles;
		public ObjectsToSpawn[] objectsToSpawn;
		public float overheatMultiplier;
		public float percentageDamage;
		public Player player;
		public float projectielSimulatonSpeed;
		public Color projectileColor;
		public ProjectilesToSpawn[] projectiles;
		public float projectileSize;
		public float projectileSpeed;
		public int randomBounces;
		public float recoil;
		public float recoilMuiltiplier;
		public int reflects;
		public float reloadTime;
		public float reloadTimeAdd;
		public float shake;
		public float shakeM;
		public Action<GameObject> ShootPojectileAction;
		//public Transform shootPosition;
		public float sinceAttack;
		public float size;
		public float slow;
		public int smartBounce;
		public bool soundDisableRayHitBulletSound;
		public SoundGun soundGun;
		public SoundImpactModifier soundImpactModifier;
		public SoundShotModifier soundShotModifier;
		public bool spawnSkelletonSquare;
		public float speedMOnBounce;
		public float spread;
		public bool teleport;
		public float timeBetweenBullets;
		public float timeToReachFullMovementMultiplier;
		public bool unblockable;
		public bool useCharge;
		public bool waveMovement;

		public Action attackAction;
		public int gunID;
		public float spreadOfLastBullet;

		public static void CopyGun(Gun copyFromGun, Gun copyToGun)
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
		public static void GetGunStats(Gun gun)
		{
			GunStats gunStats = new GunStats();

			gunStats.ammo = gun.ammo;
			gunStats.ammoReg = gun.ammoReg;
			gunStats.attackID = gun.attackID;
			gunStats.attackSpeed = gun.attackSpeed;
			gunStats.attackSpeedMultiplier = gun.attackSpeedMultiplier;
			gunStats.bodyRecoil = gun.bodyRecoil;
			gunStats.bulletDamageMultiplier = gun.bulletDamageMultiplier;
			gunStats.bulletPortal = gun.bulletPortal;
			gunStats.bursts = gun.bursts;
			gunStats.chargeDamageMultiplier = gun.chargeDamageMultiplier;
			gunStats.chargeEvenSpreadTo = gun.chargeEvenSpreadTo;
			gunStats.chargeNumberOfProjectilesTo = gun.chargeNumberOfProjectilesTo;
			gunStats.chargeRecoilTo = gun.chargeRecoilTo;
			gunStats.chargeSpeedTo = gun.chargeSpeedTo;
			gunStats.chargeSpreadTo = gun.chargeSpreadTo;
			gunStats.cos = gun.cos;
			gunStats.currentCharge = gun.currentCharge;
			gunStats.damage = gun.damage;
			gunStats.damageAfterDistanceMultiplier = gun.damageAfterDistanceMultiplier;
			gunStats.defaultCooldown = gun.defaultCooldown;
			gunStats.dmgMOnBounce = gun.dmgMOnBounce;
			gunStats.dontAllowAutoFire = gun.dontAllowAutoFire;
			gunStats.drag = gun.drag;
			gunStats.dragMinSpeed = gun.dragMinSpeed;
			gunStats.evenSpread = gun.evenSpread;
			gunStats.explodeNearEnemyDamage = gun.explodeNearEnemyDamage;
			gunStats.explodeNearEnemyRange = gun.explodeNearEnemyRange;
			gunStats.forceSpecificAttackSpeed = gun.forceSpecificAttackSpeed;
			gunStats.forceSpecificShake = gun.forceSpecificShake;
			gunStats.gravity = gun.gravity;
			gunStats.hitMovementMultiplier = gun.hitMovementMultiplier;
			//gunStats.holdable = gun.holdable;
			gunStats.ignoreWalls = gun.ignoreWalls;
			gunStats.isProjectileGun = gun.isProjectileGun;
			gunStats.isReloading = gun.isReloading;
			gunStats.knockback = gun.knockback;
			gunStats.lockGunToDefault = gun.lockGunToDefault;
			gunStats.multiplySpread = gun.multiplySpread;
			gunStats.numberOfProjectiles = gun.numberOfProjectiles;
			gunStats.objectsToSpawn = gun.objectsToSpawn;
			gunStats.overheatMultiplier = gun.overheatMultiplier;
			gunStats.percentageDamage = gun.percentageDamage;
			gunStats.player = gun.player;
			gunStats.projectielSimulatonSpeed = gun.projectielSimulatonSpeed;
			gunStats.projectileColor = gun.projectileColor;
			gunStats.projectiles = gun.projectiles;
			gunStats.projectileSize = gun.projectileSize;
			gunStats.projectileSpeed = gun.projectileSpeed;
			gunStats.randomBounces = gun.randomBounces;
			gunStats.recoil = gun.recoil;
			gunStats.recoilMuiltiplier = gun.recoilMuiltiplier;
			gunStats.reflects = gun.reflects;
			gunStats.reloadTime = gun.reloadTime;
			gunStats.reloadTimeAdd = gun.reloadTimeAdd;
			gunStats.shake = gun.shake;
			gunStats.shakeM = gun.shakeM;
			gunStats.ShootPojectileAction = gun.ShootPojectileAction;
			//gunStats.shootPosition = gun.shootPosition;
			gunStats.sinceAttack = gun.sinceAttack;
			gunStats.size = gun.size;
			gunStats.slow = gun.slow;
			gunStats.smartBounce = gun.smartBounce;
			gunStats.soundDisableRayHitBulletSound = gun.soundDisableRayHitBulletSound;
			gunStats.soundGun = gun.soundGun;
			gunStats.soundImpactModifier = gun.soundImpactModifier;
			gunStats.soundShotModifier = gun.soundShotModifier;
			gunStats.spawnSkelletonSquare = gun.spawnSkelletonSquare;
			gunStats.speedMOnBounce = gun.speedMOnBounce;
			gunStats.spread = gun.spread;
			gunStats.teleport = gun.teleport;
			gunStats.timeBetweenBullets = gun.timeBetweenBullets;
			gunStats.timeToReachFullMovementMultiplier = gun.timeToReachFullMovementMultiplier;
			gunStats.unblockable = gun.unblockable;
			gunStats.useCharge = gun.useCharge;
			gunStats.waveMovement = gun.waveMovement;


			gunStats.attackAction = (Action)Traverse.Create(gun).Field("attackAction").GetValue();
			//gunStats.gunAmmo = (GunAmmo)Traverse.Create(gun).Field("gunAmmo").GetValue();
			gunStats.gunID = (int)Traverse.Create(gun).Field("gunID").GetValue();
			gunStats.spreadOfLastBullet = (float)Traverse.Create(gun).Field("spreadOfLastBullet").GetValue();

		}
		public static void ApplyGunStats(GunStats gunStats, Gun gun)
		{

			gun.ammo = gunStats.ammo;
			gun.ammoReg = gunStats.ammoReg;
			gun.attackID = gunStats.attackID;
			gun.attackSpeed = gunStats.attackSpeed;
			gun.attackSpeedMultiplier = gunStats.attackSpeedMultiplier;
			gun.bodyRecoil = gunStats.bodyRecoil;
			gun.bulletDamageMultiplier = gunStats.bulletDamageMultiplier;
			gun.bulletPortal = gunStats.bulletPortal;
			gun.bursts = gunStats.bursts;
			gun.chargeDamageMultiplier = gunStats.chargeDamageMultiplier;
			gun.chargeEvenSpreadTo = gunStats.chargeEvenSpreadTo;
			gun.chargeNumberOfProjectilesTo = gunStats.chargeNumberOfProjectilesTo;
			gun.chargeRecoilTo = gunStats.chargeRecoilTo;
			gun.chargeSpeedTo = gunStats.chargeSpeedTo;
			gun.chargeSpreadTo = gunStats.chargeSpreadTo;
			gun.cos = gunStats.cos;
			gun.currentCharge = gunStats.currentCharge;
			gun.damage = gunStats.damage;
			gun.damageAfterDistanceMultiplier = gunStats.damageAfterDistanceMultiplier;
			gun.defaultCooldown = gunStats.defaultCooldown;
			gun.dmgMOnBounce = gunStats.dmgMOnBounce;
			gun.dontAllowAutoFire = gunStats.dontAllowAutoFire;
			gun.drag = gunStats.drag;
			gun.dragMinSpeed = gunStats.dragMinSpeed;
			gun.evenSpread = gunStats.evenSpread;
			gun.explodeNearEnemyDamage = gunStats.explodeNearEnemyDamage;
			gun.explodeNearEnemyRange = gunStats.explodeNearEnemyRange;
			gun.forceSpecificAttackSpeed = gunStats.forceSpecificAttackSpeed;
			gun.forceSpecificShake = gunStats.forceSpecificShake;
			gun.gravity = gunStats.gravity;
			gun.hitMovementMultiplier = gunStats.hitMovementMultiplier;
			//gun.holdable = gunStats.holdable;
			gun.ignoreWalls = gunStats.ignoreWalls;
			gun.isProjectileGun = gunStats.isProjectileGun;
			gun.isReloading = gunStats.isReloading;
			gun.knockback = gunStats.knockback;
			gun.lockGunToDefault = gunStats.lockGunToDefault;
			gun.multiplySpread = gunStats.multiplySpread;
			gun.numberOfProjectiles = gunStats.numberOfProjectiles;
			gun.objectsToSpawn = gunStats.objectsToSpawn;
			gun.overheatMultiplier = gunStats.overheatMultiplier;
			gun.percentageDamage = gunStats.percentageDamage;
			gun.player = gunStats.player;
			gun.projectielSimulatonSpeed = gunStats.projectielSimulatonSpeed;
			gun.projectileColor = gunStats.projectileColor;
			gun.projectiles = gunStats.projectiles;
			gun.projectileSize = gunStats.projectileSize;
			gun.projectileSpeed = gunStats.projectileSpeed;
			gun.randomBounces = gunStats.randomBounces;
			gun.recoil = gunStats.recoil;
			gun.recoilMuiltiplier = gunStats.recoilMuiltiplier;
			gun.reflects = gunStats.reflects;
			gun.reloadTime = gunStats.reloadTime;
			gun.reloadTimeAdd = gunStats.reloadTimeAdd;
			gun.shake = gunStats.shake;
			gun.shakeM = gunStats.shakeM;
			gun.ShootPojectileAction = gunStats.ShootPojectileAction;
			//gun.shootPosition = gunStats.shootPosition;
			gun.sinceAttack = gunStats.sinceAttack;
			gun.size = gunStats.size;
			gun.slow = gunStats.slow;
			gun.smartBounce = gunStats.smartBounce;
			gun.soundDisableRayHitBulletSound = gunStats.soundDisableRayHitBulletSound;
			gun.soundGun = gunStats.soundGun;
			gun.soundImpactModifier = gunStats.soundImpactModifier;
			gun.soundShotModifier = gunStats.soundShotModifier;
			gun.spawnSkelletonSquare = gunStats.spawnSkelletonSquare;
			gun.speedMOnBounce = gunStats.speedMOnBounce;
			gun.spread = gunStats.spread;
			gun.teleport = gunStats.teleport;
			gun.timeBetweenBullets = gunStats.timeBetweenBullets;
			gun.timeToReachFullMovementMultiplier = gunStats.timeToReachFullMovementMultiplier;
			gun.unblockable = gunStats.unblockable;
			gun.useCharge = gunStats.useCharge;
			gun.waveMovement = gunStats.waveMovement;

			Traverse.Create(gun).Field("attackAction").SetValue(gunStats.attackAction);
			//Traverse.Create(gun).Field("gunAmmo").SetValue(gunStats.gunAmmo);
			Traverse.Create(gun).Field("gunID").SetValue(gunStats.gunID);
			Traverse.Create(gun).Field("spreadOfLastBullet").SetValue(gunStats.spreadOfLastBullet);

		}
		public void ApplyGunStats(Gun gun)
		{

			gun.ammo = this.ammo;
			gun.ammoReg = this.ammoReg;
			gun.attackID = this.attackID;
			gun.attackSpeed = this.attackSpeed;
			gun.attackSpeedMultiplier = this.attackSpeedMultiplier;
			gun.bodyRecoil = this.bodyRecoil;
			gun.bulletDamageMultiplier = this.bulletDamageMultiplier;
			gun.bulletPortal = this.bulletPortal;
			gun.bursts = this.bursts;
			gun.chargeDamageMultiplier = this.chargeDamageMultiplier;
			gun.chargeEvenSpreadTo = this.chargeEvenSpreadTo;
			gun.chargeNumberOfProjectilesTo = this.chargeNumberOfProjectilesTo;
			gun.chargeRecoilTo = this.chargeRecoilTo;
			gun.chargeSpeedTo = this.chargeSpeedTo;
			gun.chargeSpreadTo = this.chargeSpreadTo;
			gun.cos = this.cos;
			gun.currentCharge = this.currentCharge;
			gun.damage = this.damage;
			gun.damageAfterDistanceMultiplier = this.damageAfterDistanceMultiplier;
			gun.defaultCooldown = this.defaultCooldown;
			gun.dmgMOnBounce = this.dmgMOnBounce;
			gun.dontAllowAutoFire = this.dontAllowAutoFire;
			gun.drag = this.drag;
			gun.dragMinSpeed = this.dragMinSpeed;
			gun.evenSpread = this.evenSpread;
			gun.explodeNearEnemyDamage = this.explodeNearEnemyDamage;
			gun.explodeNearEnemyRange = this.explodeNearEnemyRange;
			gun.forceSpecificAttackSpeed = this.forceSpecificAttackSpeed;
			gun.forceSpecificShake = this.forceSpecificShake;
			gun.gravity = this.gravity;
			gun.hitMovementMultiplier = this.hitMovementMultiplier;
			//gun.holdable = this.holdable;
			gun.ignoreWalls = this.ignoreWalls;
			gun.isProjectileGun = this.isProjectileGun;
			gun.isReloading = this.isReloading;
			gun.knockback = this.knockback;
			gun.lockGunToDefault = this.lockGunToDefault;
			gun.multiplySpread = this.multiplySpread;
			gun.numberOfProjectiles = this.numberOfProjectiles;
			gun.objectsToSpawn = this.objectsToSpawn;
			gun.overheatMultiplier = this.overheatMultiplier;
			gun.percentageDamage = this.percentageDamage;
			gun.player = this.player;
			gun.projectielSimulatonSpeed = this.projectielSimulatonSpeed;
			gun.projectileColor = this.projectileColor;
			gun.projectiles = this.projectiles;
			gun.projectileSize = this.projectileSize;
			gun.projectileSpeed = this.projectileSpeed;
			gun.randomBounces = this.randomBounces;
			gun.recoil = this.recoil;
			gun.recoilMuiltiplier = this.recoilMuiltiplier;
			gun.reflects = this.reflects;
			gun.reloadTime = this.reloadTime;
			gun.reloadTimeAdd = this.reloadTimeAdd;
			gun.shake = this.shake;
			gun.shakeM = this.shakeM;
			gun.ShootPojectileAction = this.ShootPojectileAction;
			//gun.shootPosition = this.shootPosition;
			gun.sinceAttack = this.sinceAttack;
			gun.size = this.size;
			gun.slow = this.slow;
			gun.smartBounce = this.smartBounce;
			gun.soundDisableRayHitBulletSound = this.soundDisableRayHitBulletSound;
			gun.soundGun = this.soundGun;
			gun.soundImpactModifier = this.soundImpactModifier;
			gun.soundShotModifier = this.soundShotModifier;
			gun.spawnSkelletonSquare = this.spawnSkelletonSquare;
			gun.speedMOnBounce = this.speedMOnBounce;
			gun.spread = this.spread;
			gun.teleport = this.teleport;
			gun.timeBetweenBullets = this.timeBetweenBullets;
			gun.timeToReachFullMovementMultiplier = this.timeToReachFullMovementMultiplier;
			gun.unblockable = this.unblockable;
			gun.useCharge = this.useCharge;
			gun.waveMovement = this.waveMovement;

			Traverse.Create(gun).Field("attackAction").SetValue(this.attackAction);
			//Traverse.Create(gun).Field("gunAmmo").SetValue(this.gunAmmo);
			Traverse.Create(gun).Field("gunID").SetValue(this.gunID);
			Traverse.Create(gun).Field("spreadOfLastBullet").SetValue(this.spreadOfLastBullet);

		}
	}
}

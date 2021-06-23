using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;
using Photon.Pun;

namespace PCE.MonoBehaviours
{

    public class SpawnBulletsEffect : MonoBehaviour
    {
		private readonly float initialDelay = 1f;

		private int numBullets = 1;
		private int numShot = 0;
		private Gun gunToShootFrom;
		private Vector3 directionToShoot = Vector3.zero;
		private Vector3 positionToShootFrom = Vector3.zero;
		private float timeBetweenShots = 0f;
		private float timeSinceLastShot;

		private Player player;

        void Awake()
        {
            this.player = this.gameObject.GetComponent<Player>();
        }

        void Start()
        {
			this.ResetTimer();
			this.timeSinceLastShot += this.initialDelay;
		}

        void Update()
        {
			if (this.numShot >= this.numBullets)
            {
				Destroy(this);
            }
			else if (Time.time >= this.timeSinceLastShot + this.timeBetweenShots)
            {
				Shoot();
            }
        }
        public void OnDestroy()
        {
			Destroy(this.gunToShootFrom);
		}

		private void Shoot()
        {
			numShot++;
			int currentNumberOfProjectiles = this.gunToShootFrom.lockGunToDefault ? 1 : (this.gunToShootFrom.numberOfProjectiles + Mathf.RoundToInt(this.gunToShootFrom.chargeNumberOfProjectilesTo * 0f));
			for (int i = 0; i < this.gunToShootFrom.projectiles.Length; i++)
			{
				for (int j = 0; j < currentNumberOfProjectiles; j++)
				{
					Vector3 directionToShootThisBullet = this.directionToShoot;
					if (this.gunToShootFrom.spread != 0f)
					{
						// randomly spread shots
						float d = this.gunToShootFrom.multiplySpread;
						float num = UnityEngine.Random.Range(-this.gunToShootFrom.spread, this.gunToShootFrom.spread);
						num /= (1f + this.gunToShootFrom.projectileSpeed * 0.5f) * 0.5f;
						directionToShootThisBullet += Vector3.Cross(directionToShootThisBullet, Vector3.forward) * num * d;
					}

					if ((bool)typeof(Gun).InvokeMember("CheckIsMine", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, this.gunToShootFrom, new object[] { }))
					{

						GameObject gameObject = PhotonNetwork.Instantiate(this.gunToShootFrom.projectiles[i].objectToSpawn.gameObject.name, this.positionToShootFrom, Quaternion.LookRotation(directionToShootThisBullet), 0, null);

						if (PhotonNetwork.OfflineMode)
						{
							typeof(ProjectileInit).InvokeMember("OFFLINE_Init_SeparateGun", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, gameObject.GetComponent<ProjectileInit>(), new object[] { base.GetComponentInParent<Player>().playerID, (int)Traverse.Create(this.gunToShootFrom).Field("gunID").GetValue(), currentNumberOfProjectiles, 1f, UnityEngine.Random.Range(0f, 1f) });

						}
						else
						{
							gameObject.GetComponent<PhotonView>().RPC("RPCA_Init_SeparateGun", RpcTarget.All, new object[]
							{

										base.GetComponentInParent<CharacterData>().view.OwnerActorNr,
										(int)Traverse.Create(this.gunToShootFrom).Field("gunID").GetValue(),
										currentNumberOfProjectiles,
										1f,
										UnityEngine.Random.Range(0f, 1f)
							});
						}
					}
				}
			}
			this.ResetTimer();

		}
		private void RPCA_Shoot(int numProj, float dmgM, float seed)
        {
			this.gunToShootFrom.BulletInit(base.gameObject, numProj, dmgM, seed, true);
        }
		public void SetGun(Gun gun)
		{
			this.gunToShootFrom = this.gameObject.AddComponent<Gun>();
			SpawnBulletsEffect.CopyGunStats(gun, this.gunToShootFrom);
		}
		public void SetNumBullets(int num)
        {
			this.numBullets = num;
        }
		public void SetPosition(Vector3 pos)
        {
			this.positionToShootFrom = pos;
        }
		public void SetDirection(Vector3 dir)
        {
			this.directionToShoot = dir;
        }
		public void SetTimeBetweenShots(float delay)
        {
			this.timeBetweenShots = delay;
        }
		private void ResetTimer()
        {
			this.timeSinceLastShot = Time.time;
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

			Traverse.Create(copyToGun).Field("forceShootDir").SetValue((Vector3)Traverse.Create(copyFromGun).Field("forceShootDir").GetValue());

		}

	}
}

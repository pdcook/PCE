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
			if (this.directionToShoot != Vector3.zero)
            {
				Traverse.Create(this.gunToShootFrom).Field("forceShootDir").SetValue(this.directionToShoot);
			}
			Shoot();

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
					GameObject gameObject = PhotonNetwork.Instantiate(this.gunToShootFrom.projectiles[i].objectToSpawn.gameObject.name, this.positionToShootFrom, (Quaternion)typeof(Gun).InvokeMember("getShootRotation", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, this.gunToShootFrom, new object[] { j, currentNumberOfProjectiles, 0f }), 0, null);
					

					if (PhotonNetwork.OfflineMode)
					{
						typeof(ProjectileInit).InvokeMember("OFFLINE_Init_SeparateGun", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, gameObject.GetComponent<ProjectileInit>(), new object[] { base.GetComponentInParent<Player>().playerID, (int)Traverse.Create(this.gunToShootFrom).Field("gunID").GetValue(), currentNumberOfProjectiles, 1f, UnityEngine.Random.Range(0f, 1f)});
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
			this.ResetTimer();

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
			Traverse.Create(copyToGun).Field("gunID").SetValue((int)Traverse.Create(copyFromGun).Field("gunID").GetValue());

			copyToGun.soundGun = copyFromGun.soundGun;

			copyToGun.objectsToSpawn = copyFromGun.objectsToSpawn;
			copyToGun.projectiles = copyFromGun.projectiles;
			copyToGun.shootPosition = copyFromGun.shootPosition;
			copyToGun.useCharge = copyFromGun.useCharge;
			copyToGun.lockGunToDefault = copyFromGun.lockGunToDefault;
		}

	}
}

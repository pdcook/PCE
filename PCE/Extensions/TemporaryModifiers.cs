using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sonigon;
using SoundImplementation;
using System.Reflection;
using HarmonyLib;
using UnboundLib;
using PCE.MonoBehaviours;
using System.Linq;

namespace PCE.Extensions
{
    public class BlockModifier
    {
        public List<GameObject> objectsToSpawn_add = new List<GameObject>();
        public float counter_add = 0f;
        public float counter_mult = 1f;
        public float cdMultiplier_add = 0f;
        public float cdMultiplier_mult = 1f;
        public float cdAdd_add = 0f;
        public float cdAdd_mult = 1f;
        public float forceToAdd_add = 0f;
        public float forceToAdd_mult = 1f;
        public float forceToAddUp_add = 0f;
        public float forceToAddUp_mult = 1f;
        public int additionalBlocks_add = 0;
        public int additionalBlocks_mult = 1;
        public float healing_add = 0f;
        public float healing_mult = 1f;

        private float counter_delta = 0f;
        private float cdMultiplier_delta = 0f;
        private float cdAdd_delta = 0f;
        private float forceToAdd_delta = 0f;
        private float forceToAddUp_delta = 0f;
        private int additionalBlocks_delta = 0;
        private float healing_delta = 0f;

        public static void ApplyBlockModifier(BlockModifier blockModifier, Block block)
        {
            blockModifier.counter_delta = block.counter * blockModifier.counter_mult + blockModifier.counter_add - block.counter;
            blockModifier.cdMultiplier_delta = block.cdMultiplier * blockModifier.cdMultiplier_mult + blockModifier.cdMultiplier_add - block.cdMultiplier;
            blockModifier.cdAdd_delta = block.cdAdd * blockModifier.cdAdd_mult + blockModifier.cdAdd_add - block.cdAdd;
            blockModifier.forceToAdd_delta = block.forceToAdd * blockModifier.forceToAdd_mult + blockModifier.forceToAdd_add - block.forceToAdd;
            blockModifier.forceToAddUp_delta = block.forceToAddUp * blockModifier.forceToAddUp_mult + blockModifier.forceToAddUp_add - block.forceToAddUp;
            blockModifier.additionalBlocks_delta = block.additionalBlocks * blockModifier.additionalBlocks_mult + blockModifier.additionalBlocks_add - block.additionalBlocks;
            blockModifier.healing_delta = block.healing * blockModifier.healing_mult + blockModifier.healing_add - block.healing;

            foreach (GameObject objectToSpawn in blockModifier.objectsToSpawn_add)
            {
                block.objectsToSpawn.Add(objectToSpawn);
            }

            block.counter += blockModifier.counter_delta;
            block.cdMultiplier += blockModifier.cdMultiplier_delta;
            block.cdAdd += blockModifier.cdAdd_delta;
            block.forceToAdd += blockModifier.forceToAdd_delta;
            block.forceToAddUp += blockModifier.forceToAddUp_delta;
            block.additionalBlocks += blockModifier.additionalBlocks_delta;
            block.healing += blockModifier.healing_delta;
        }
        public void ApplyBlockModifier(Block block)
        {
            this.counter_delta = block.counter * this.counter_mult + this.counter_add - block.counter;
            this.cdMultiplier_delta = block.cdMultiplier * this.cdMultiplier_mult + this.cdMultiplier_add - block.cdMultiplier;
            this.cdAdd_delta = block.cdAdd * this.cdAdd_mult + this.cdAdd_add - block.cdAdd;
            this.forceToAdd_delta = block.forceToAdd * this.forceToAdd_mult + this.forceToAdd_add - block.forceToAdd;
            this.forceToAddUp_delta = block.forceToAddUp * this.forceToAddUp_mult + this.forceToAddUp_add - block.forceToAddUp;
            this.additionalBlocks_delta = block.additionalBlocks * this.additionalBlocks_mult + this.additionalBlocks_add - block.additionalBlocks;
            this.healing_delta = block.healing * this.healing_mult + this.healing_add - block.healing;

            foreach (GameObject objectToSpawn in this.objectsToSpawn_add)
            {
                block.objectsToSpawn.Add(objectToSpawn);
            }

            block.counter += this.counter_delta;
            block.cdMultiplier += this.cdMultiplier_delta;
            block.cdAdd += this.cdAdd_delta;
            block.forceToAdd += this.forceToAdd_delta;
            block.forceToAddUp += this.forceToAddUp_delta;
            block.additionalBlocks += this.additionalBlocks_delta;
            block.healing += this.healing_delta;
        }
        public static void RemoveBlockModifier(BlockModifier blockModifier, Block block)
        {
            foreach (GameObject objectToSpawn in blockModifier.objectsToSpawn_add)
            {
                block.objectsToSpawn.Remove(objectToSpawn);
            }

            block.counter -= blockModifier.counter_delta;
            block.cdMultiplier -= blockModifier.cdMultiplier_delta;
            block.cdAdd -= blockModifier.cdAdd_delta;
            block.forceToAdd -= blockModifier.forceToAdd_delta;
            block.forceToAddUp -= blockModifier.forceToAddUp_delta;
            block.additionalBlocks -= blockModifier.additionalBlocks_delta;
            block.healing -= blockModifier.healing_delta;

            // reset deltas
            blockModifier.objectsToSpawn_add = new List<GameObject>();
            blockModifier.counter_delta = 0f;
            blockModifier.cdMultiplier_delta = 0f;
            blockModifier.cdAdd_delta = 0f;
            blockModifier.forceToAdd_delta = 0f;
            blockModifier.forceToAddUp_delta = 0f;
            blockModifier.additionalBlocks_delta = 0;
            blockModifier.healing_delta = 0f;

        }
        public void RemoveBlockModifier(Block block)
        {
            foreach (GameObject objectToSpawn in this.objectsToSpawn_add)
            {
                block.objectsToSpawn.Remove(objectToSpawn);
            }

            block.counter -= this.counter_delta;
            block.cdMultiplier -= this.cdMultiplier_delta;
            block.cdAdd -= this.cdAdd_delta;
            block.forceToAdd -= this.forceToAdd_delta;
            block.forceToAddUp -= this.forceToAddUp_delta;
            block.additionalBlocks -= this.additionalBlocks_delta;
            block.healing -= this.healing_delta;

            // reset deltas
            this.objectsToSpawn_add = new List<GameObject>();
            this.counter_delta = 0f;
            this.cdMultiplier_delta = 0f;
            this.cdAdd_delta = 0f;
            this.forceToAdd_delta = 0f;
            this.forceToAddUp_delta = 0f;
            this.additionalBlocks_delta = 0;
            this.healing_delta = 0f;
        }

    }
    public class GunAmmoStatModifier
    {
        public int maxAmmo_mult = 1;
        public int maxAmmo_add = 0;
        public float reloadTimeMultiplier_mult = 1f;
        public float reloadTimeAdd_add = 0f;

        private int maxAmmo_delta = 0;
        private float reloadTimeMultiplier_delta = 0f;
        private float reloadTimeAdd_delta = 0f;

        public static void ApplyGunAmmoStatModifier(GunAmmoStatModifier gunAmmoStatModifier, GunAmmo gunAmmo)
        {
            gunAmmoStatModifier.maxAmmo_delta = gunAmmo.maxAmmo * gunAmmoStatModifier.maxAmmo_mult + gunAmmoStatModifier.maxAmmo_add - gunAmmo.maxAmmo;
            gunAmmoStatModifier.reloadTimeMultiplier_delta = gunAmmo.reloadTimeMultiplier * gunAmmoStatModifier.reloadTimeMultiplier_mult - gunAmmo.reloadTimeMultiplier;
            gunAmmoStatModifier.reloadTimeAdd_delta = gunAmmoStatModifier.reloadTimeAdd_add;

            gunAmmo.maxAmmo += gunAmmoStatModifier.maxAmmo_delta;
            gunAmmo.reloadTimeMultiplier += gunAmmoStatModifier.reloadTimeMultiplier_delta;
            gunAmmo.reloadTimeAdd += gunAmmoStatModifier.reloadTimeAdd_delta;
        }
        public void ApplyGunAmmoStatModifier(GunAmmo gunAmmo)
        {
            this.maxAmmo_delta = gunAmmo.maxAmmo * this.maxAmmo_mult + this.maxAmmo_add - gunAmmo.maxAmmo;
            this.reloadTimeMultiplier_delta = gunAmmo.reloadTimeMultiplier * this.reloadTimeMultiplier_mult - gunAmmo.reloadTimeMultiplier;
            this.reloadTimeAdd_delta = this.reloadTimeAdd_add;

            gunAmmo.maxAmmo += this.maxAmmo_delta;
            gunAmmo.reloadTimeMultiplier += this.reloadTimeMultiplier_delta;
            gunAmmo.reloadTimeAdd += this.reloadTimeAdd_delta;
        }
        public static void RemoveGunAmmoStatModifier(GunAmmoStatModifier gunAmmoStatModifier, GunAmmo gunAmmo)
        {
            gunAmmo.maxAmmo -= gunAmmoStatModifier.maxAmmo_delta;
            gunAmmo.reloadTimeMultiplier -= gunAmmoStatModifier.reloadTimeMultiplier_delta;
            gunAmmo.reloadTimeAdd -= gunAmmoStatModifier.reloadTimeAdd_delta;

            // reset deltas
            gunAmmoStatModifier.maxAmmo_delta = 0;
            gunAmmoStatModifier.reloadTimeMultiplier_delta = 0f;
            gunAmmoStatModifier.reloadTimeAdd_delta = 0f;
        }
        public void RemoveGunAmmoStatModifier(GunAmmo gunAmmo)
        {
            gunAmmo.maxAmmo -= this.maxAmmo_delta;
            gunAmmo.reloadTimeMultiplier -= this.reloadTimeMultiplier_delta;
            gunAmmo.reloadTimeAdd -= this.reloadTimeAdd_delta;

            // reset deltas
            this.maxAmmo_delta = 0;
            this.reloadTimeMultiplier_delta = 0f;
            this.reloadTimeAdd_delta = 0f;
        }

    }

    public class GunStatModifier
    {
        public float damage_add = 0f;
        public float damage_mult = 1f;
        public float recoilMuiltiplier_add = 0f;
        public float recoilMuiltiplier_mult = 1f;
        public float knockback_add = 0f;
        public float knockback_mult = 1f;
        public float attackSpeed_add = 0f;
        public float attackSpeed_mult = 1f;
        public float projectileSpeed_add = 0f;
        public float projectileSpeed_mult = 1f;
        public float projectielSimulatonSpeed_add = 0f;
        public float projectielSimulatonSpeed_mult = 1f;
        public float gravity_add = 0f;
        public float gravity_mult = 1f;
        public float damageAfterDistanceMultiplier_add = 0f;
        public float damageAfterDistanceMultiplier_mult = 1f;
        public float bulletDamageMultiplier_add = 0f;
        public float bulletDamageMultiplier_mult = 1f;
        public float multiplySpread_add = 0f;
        public float multiplySpread_mult = 1f;
        public float size_add = 0f;
        public float size_mult = 1f;
        public float timeToReachFullMovementMultiplier_add = 0f;
        public float timeToReachFullMovementMultiplier_mult = 1f;
        public int numberOfProjectiles_add = 0;
        public int numberOfProjectiles_mult = 1;
        public int bursts_add = 0;
        public int bursts_mult = 1;
        public int reflects_add = 0;
        public int reflects_mult = 1;
        public int smartBounce_add = 0;
        public int smartBounce_mult = 1;
        public int randomBounces_add = 0;
        public int randomBounces_mult = 1;
        public float timeBetweenBullets_add = 0f;
        public float timeBetweenBullets_mult = 1f;
        public float projectileSize_add = 0f;
        public float projectileSize_mult = 1f;
        public float speedMOnBounce_add = 0f;
        public float speedMOnBounce_mult = 1f;
        public float dmgMOnBounce_add = 0f;
        public float dmgMOnBounce_mult = 1f;
        public float drag_add = 0f;
        public float drag_mult = 1f;
        public float dragMinSpeed_add = 0f;
        public float dragMinSpeed_mult = 1f;
        public float spread_add = 0f;
        public float spread_mult = 1f;
        public float evenSpread_add = 0f;
        public float evenSpread_mult = 1f;
        public float percentageDamage_add = 0f;
        public float percentageDamage_mult = 1f;
        public float slow_add = 0f;
        public float slow_mult = 1f;
        public float destroyBulletAfter_add = 0f;
        public float destroyBulletAfter_mult = 1f;
        public float forceSpecificAttackSpeed_add = 0f;
        public float forceSpecificAttackSpeed_mult = 1f;
        public float explodeNearEnemyRange_add = 0f;
        public float explodeNearEnemyRange_mult = 1f;
        public float explodeNearEnemyDamage_add = 0f;
        public float explodeNearEnemyDamage_mult = 1f;
        public float hitMovementMultiplier_add = 0f;
        public float hitMovementMultiplier_mult = 1f;
        public float attackSpeedMultiplier_add = 0f;
        public float attackSpeedMultiplier_mult = 1f;
        public List<ObjectsToSpawn> objectsToSpawn_add = new List<ObjectsToSpawn>();
        public Color projectileColor = Color.black;

        // extra stuff from extensions
        public float minDistanceMultiplier_add = 0f;
        public float minDistanceMultiplier_mult = 1f;

        private float damage_delta = 0f;
        private float recoilMuiltiplier_delta = 0f;
        private float knockback_delta = 0f;
        private float attackSpeed_delta = 0f;
        private float projectileSpeed_delta = 0f;
        private float projectielSimulatonSpeed_delta = 0f;
        private float gravity_delta = 0f;
        private float damageAfterDistanceMultiplier_delta = 0f;
        private float bulletDamageMultiplier_delta = 0f;
        private float multiplySpread_delta = 0f;
        private float size_delta = 0f;
        private float timeToReachFullMovementMultiplier_delta = 0f;
        private int numberOfProjectiles_delta = 0;
        private int bursts_delta = 0;
        private int reflects_delta = 0;
        private int smartBounce_delta = 0;
        private int randomBounces_delta = 0;
        private float timeBetweenBullets_delta = 0f;
        private float projectileSize_delta = 0f;
        private float speedMOnBounce_delta = 0f;
        private float dmgMOnBounce_delta = 0f;
        private float drag_delta = 0f;
        private float dragMinSpeed_delta = 0f;
        private float spread_delta = 0f;
        private float evenSpread_delta = 0f;
        private float percentageDamage_delta = 0f;
        private float slow_delta = 0f;
        private float destroyBulletAfter_delta = 0f;
        private float forceSpecificAttackSpeed_delta = 0f;
        private float explodeNearEnemyRange_delta = 0f;
        private float explodeNearEnemyDamage_delta = 0f;
        private float hitMovementMultiplier_delta = 0f;
        private float attackSpeedMultiplier_delta = 0f;

        private GunColorEffect gunColorEffect = null;

        // extra
        private float minDistanceMultiplier_delta = 0f;


        public static void ApplyGunStatModifier(GunStatModifier gunStatModifier, Gun gun)
        {
            // regular expressions protected me against arthritis here.
            gunStatModifier.damage_delta = gun.damage * gunStatModifier.damage_mult + gunStatModifier.damage_add - gun.damage;
            gunStatModifier.recoilMuiltiplier_delta = gun.recoilMuiltiplier * gunStatModifier.recoilMuiltiplier_mult + gunStatModifier.recoilMuiltiplier_add - gun.recoilMuiltiplier;
            gunStatModifier.knockback_delta = gun.knockback * gunStatModifier.knockback_mult + gunStatModifier.knockback_add - gun.knockback;
            gunStatModifier.attackSpeed_delta = gun.attackSpeed * gunStatModifier.attackSpeed_mult + gunStatModifier.attackSpeed_add - gun.attackSpeed;
            gunStatModifier.projectileSpeed_delta = gun.projectileSpeed * gunStatModifier.projectileSpeed_mult + gunStatModifier.projectileSpeed_add - gun.projectileSpeed;
            gunStatModifier.projectielSimulatonSpeed_delta = gun.projectielSimulatonSpeed * gunStatModifier.projectielSimulatonSpeed_mult + gunStatModifier.projectielSimulatonSpeed_add - gun.projectielSimulatonSpeed;
            gunStatModifier.gravity_delta = gun.gravity * gunStatModifier.gravity_mult + gunStatModifier.gravity_add - gun.gravity;
            gunStatModifier.damageAfterDistanceMultiplier_delta = gun.damageAfterDistanceMultiplier * gunStatModifier.damageAfterDistanceMultiplier_mult + gunStatModifier.damageAfterDistanceMultiplier_add - gun.damageAfterDistanceMultiplier;
            gunStatModifier.bulletDamageMultiplier_delta = gun.bulletDamageMultiplier * gunStatModifier.bulletDamageMultiplier_mult + gunStatModifier.bulletDamageMultiplier_add - gun.bulletDamageMultiplier;
            gunStatModifier.multiplySpread_delta = gun.multiplySpread * gunStatModifier.multiplySpread_mult + gunStatModifier.multiplySpread_add - gun.multiplySpread;
            gunStatModifier.size_delta = gun.size * gunStatModifier.size_mult + gunStatModifier.size_add - gun.size;
            gunStatModifier.timeToReachFullMovementMultiplier_delta = gun.timeToReachFullMovementMultiplier * gunStatModifier.timeToReachFullMovementMultiplier_mult + gunStatModifier.timeToReachFullMovementMultiplier_add - gun.timeToReachFullMovementMultiplier;
            gunStatModifier.numberOfProjectiles_delta = gun.numberOfProjectiles * gunStatModifier.numberOfProjectiles_mult + gunStatModifier.numberOfProjectiles_add - gun.numberOfProjectiles;
            gunStatModifier.bursts_delta = gun.bursts * gunStatModifier.bursts_mult + gunStatModifier.bursts_add - gun.bursts;
            gunStatModifier.reflects_delta = gun.reflects * gunStatModifier.reflects_mult + gunStatModifier.reflects_add - gun.reflects;
            gunStatModifier.smartBounce_delta = gun.smartBounce * gunStatModifier.smartBounce_mult + gunStatModifier.smartBounce_add - gun.smartBounce;
            gunStatModifier.randomBounces_delta = gun.randomBounces * gunStatModifier.randomBounces_mult + gunStatModifier.randomBounces_add - gun.randomBounces;
            gunStatModifier.timeBetweenBullets_delta = gun.timeBetweenBullets * gunStatModifier.timeBetweenBullets_mult + gunStatModifier.timeBetweenBullets_add - gun.timeBetweenBullets;
            gunStatModifier.projectileSize_delta = gun.projectileSize * gunStatModifier.projectileSize_mult + gunStatModifier.projectileSize_add - gun.projectileSize;
            gunStatModifier.speedMOnBounce_delta = gun.speedMOnBounce * gunStatModifier.speedMOnBounce_mult + gunStatModifier.speedMOnBounce_add - gun.speedMOnBounce;
            gunStatModifier.dmgMOnBounce_delta = gun.dmgMOnBounce * gunStatModifier.dmgMOnBounce_mult + gunStatModifier.dmgMOnBounce_add - gun.dmgMOnBounce;
            gunStatModifier.drag_delta = gun.drag * gunStatModifier.drag_mult + gunStatModifier.drag_add - gun.drag;
            gunStatModifier.dragMinSpeed_delta = gun.dragMinSpeed * gunStatModifier.dragMinSpeed_mult + gunStatModifier.dragMinSpeed_add - gun.dragMinSpeed;
            gunStatModifier.spread_delta = gun.spread * gunStatModifier.spread_mult + gunStatModifier.spread_add - gun.spread;
            gunStatModifier.evenSpread_delta = gun.evenSpread * gunStatModifier.evenSpread_mult + gunStatModifier.evenSpread_add - gun.evenSpread;
            gunStatModifier.percentageDamage_delta = gun.percentageDamage * gunStatModifier.percentageDamage_mult + gunStatModifier.percentageDamage_add - gun.percentageDamage;
            gunStatModifier.slow_delta = gun.slow * gunStatModifier.slow_mult + gunStatModifier.slow_add - gun.slow;
            gunStatModifier.destroyBulletAfter_delta = gun.destroyBulletAfter * gunStatModifier.destroyBulletAfter_mult + gunStatModifier.destroyBulletAfter_add - gun.destroyBulletAfter;
            gunStatModifier.forceSpecificAttackSpeed_delta = gun.forceSpecificAttackSpeed * gunStatModifier.forceSpecificAttackSpeed_mult + gunStatModifier.forceSpecificAttackSpeed_add - gun.forceSpecificAttackSpeed;
            gunStatModifier.explodeNearEnemyRange_delta = gun.explodeNearEnemyRange * gunStatModifier.explodeNearEnemyRange_mult + gunStatModifier.explodeNearEnemyRange_add - gun.explodeNearEnemyRange;
            gunStatModifier.explodeNearEnemyDamage_delta = gun.explodeNearEnemyDamage * gunStatModifier.explodeNearEnemyDamage_mult + gunStatModifier.explodeNearEnemyDamage_add - gun.explodeNearEnemyDamage;
            gunStatModifier.hitMovementMultiplier_delta = gun.hitMovementMultiplier * gunStatModifier.hitMovementMultiplier_mult + gunStatModifier.hitMovementMultiplier_add - gun.hitMovementMultiplier;
            gunStatModifier.attackSpeedMultiplier_delta = gun.attackSpeedMultiplier * gunStatModifier.attackSpeedMultiplier_mult + gunStatModifier.attackSpeedMultiplier_add - gun.attackSpeedMultiplier;



            gunStatModifier.minDistanceMultiplier_delta = gun.GetAdditionalData().minDistanceMultiplier * gunStatModifier.minDistanceMultiplier_mult + gunStatModifier.minDistanceMultiplier_add - gun.GetAdditionalData().minDistanceMultiplier;


            // apply everything
            gun.damage += gunStatModifier.damage_delta;
            gun.recoilMuiltiplier += gunStatModifier.recoilMuiltiplier_delta;
            gun.knockback += gunStatModifier.knockback_delta;
            gun.attackSpeed += gunStatModifier.attackSpeed_delta;
            gun.projectileSpeed += gunStatModifier.projectileSpeed_delta;
            gun.projectielSimulatonSpeed += gunStatModifier.projectielSimulatonSpeed_delta;
            gun.gravity += gunStatModifier.gravity_delta;
            gun.damageAfterDistanceMultiplier += gunStatModifier.damageAfterDistanceMultiplier_delta;
            gun.bulletDamageMultiplier += gunStatModifier.bulletDamageMultiplier_delta;
            gun.multiplySpread += gunStatModifier.multiplySpread_delta;
            gun.size += gunStatModifier.size_delta;
            gun.timeToReachFullMovementMultiplier += gunStatModifier.timeToReachFullMovementMultiplier_delta;
            gun.numberOfProjectiles += gunStatModifier.numberOfProjectiles_delta;
            gun.bursts += gunStatModifier.bursts_delta;
            gun.reflects += gunStatModifier.reflects_delta;
            gun.smartBounce += gunStatModifier.smartBounce_delta;
            gun.randomBounces += gunStatModifier.randomBounces_delta;
            gun.timeBetweenBullets += gunStatModifier.timeBetweenBullets_delta;
            gun.projectileSize += gunStatModifier.projectileSize_delta;
            gun.speedMOnBounce += gunStatModifier.speedMOnBounce_delta;
            gun.dmgMOnBounce += gunStatModifier.dmgMOnBounce_delta;
            gun.drag += gunStatModifier.drag_delta;
            gun.dragMinSpeed += gunStatModifier.dragMinSpeed_delta;
            gun.spread += gunStatModifier.spread_delta;
            gun.evenSpread += gunStatModifier.evenSpread_delta;
            gun.percentageDamage += gunStatModifier.percentageDamage_delta;
            gun.slow += gunStatModifier.slow_delta;
            gun.destroyBulletAfter += gunStatModifier.destroyBulletAfter_delta;
            gun.forceSpecificAttackSpeed += gunStatModifier.forceSpecificAttackSpeed_delta;
            gun.explodeNearEnemyRange += gunStatModifier.explodeNearEnemyRange_delta;
            gun.explodeNearEnemyDamage += gunStatModifier.explodeNearEnemyDamage_delta;
            gun.hitMovementMultiplier += gunStatModifier.hitMovementMultiplier_delta;
            gun.attackSpeedMultiplier += gunStatModifier.attackSpeedMultiplier_delta;

            List<ObjectsToSpawn> gunObjectsToSpawn = new List<ObjectsToSpawn>(gun.objectsToSpawn);

            foreach (ObjectsToSpawn objectToSpawn in gunStatModifier.objectsToSpawn_add)
            {
                gunObjectsToSpawn.Add(objectToSpawn);
            }
            gun.objectsToSpawn = gunObjectsToSpawn.ToArray();

            gunStatModifier.gunColorEffect = gun.player.gameObject.AddComponent<GunColorEffect>();
            gunStatModifier.gunColorEffect.SetColor(gunStatModifier.projectileColor);

            gun.GetAdditionalData().minDistanceMultiplier += gunStatModifier.minDistanceMultiplier_delta;

        }
        public void ApplyGunStatModifier(Gun gun)
        {
            // regular expressions protected me against arthritis here.
            this.damage_delta = gun.damage * this.damage_mult + this.damage_add - gun.damage;
            this.recoilMuiltiplier_delta = gun.recoilMuiltiplier * this.recoilMuiltiplier_mult + this.recoilMuiltiplier_add - gun.recoilMuiltiplier;
            this.knockback_delta = gun.knockback * this.knockback_mult + this.knockback_add - gun.knockback;
            this.attackSpeed_delta = gun.attackSpeed * this.attackSpeed_mult + this.attackSpeed_add - gun.attackSpeed;
            this.projectileSpeed_delta = gun.projectileSpeed * this.projectileSpeed_mult + this.projectileSpeed_add - gun.projectileSpeed;
            this.projectielSimulatonSpeed_delta = gun.projectielSimulatonSpeed * this.projectielSimulatonSpeed_mult + this.projectielSimulatonSpeed_add - gun.projectielSimulatonSpeed;
            this.gravity_delta = gun.gravity * this.gravity_mult + this.gravity_add - gun.gravity;
            this.damageAfterDistanceMultiplier_delta = gun.damageAfterDistanceMultiplier * this.damageAfterDistanceMultiplier_mult + this.damageAfterDistanceMultiplier_add - gun.damageAfterDistanceMultiplier;
            this.bulletDamageMultiplier_delta = gun.bulletDamageMultiplier * this.bulletDamageMultiplier_mult + this.bulletDamageMultiplier_add - gun.bulletDamageMultiplier;
            this.multiplySpread_delta = gun.multiplySpread * this.multiplySpread_mult + this.multiplySpread_add - gun.multiplySpread;
            this.size_delta = gun.size * this.size_mult + this.size_add - gun.size;
            this.timeToReachFullMovementMultiplier_delta = gun.timeToReachFullMovementMultiplier * this.timeToReachFullMovementMultiplier_mult + this.timeToReachFullMovementMultiplier_add - gun.timeToReachFullMovementMultiplier;
            this.numberOfProjectiles_delta = gun.numberOfProjectiles * this.numberOfProjectiles_mult + this.numberOfProjectiles_add - gun.numberOfProjectiles;
            this.bursts_delta = gun.bursts * this.bursts_mult + this.bursts_add - gun.bursts;
            this.reflects_delta = gun.reflects * this.reflects_mult + this.reflects_add - gun.reflects;
            this.smartBounce_delta = gun.smartBounce * this.smartBounce_mult + this.smartBounce_add - gun.smartBounce;
            this.randomBounces_delta = gun.randomBounces * this.randomBounces_mult + this.randomBounces_add - gun.randomBounces;
            this.timeBetweenBullets_delta = gun.timeBetweenBullets * this.timeBetweenBullets_mult + this.timeBetweenBullets_add - gun.timeBetweenBullets;
            this.projectileSize_delta = gun.projectileSize * this.projectileSize_mult + this.projectileSize_add - gun.projectileSize;
            this.speedMOnBounce_delta = gun.speedMOnBounce * this.speedMOnBounce_mult + this.speedMOnBounce_add - gun.speedMOnBounce;
            this.dmgMOnBounce_delta = gun.dmgMOnBounce * this.dmgMOnBounce_mult + this.dmgMOnBounce_add - gun.dmgMOnBounce;
            this.drag_delta = gun.drag * this.drag_mult + this.drag_add - gun.drag;
            this.dragMinSpeed_delta = gun.dragMinSpeed * this.dragMinSpeed_mult + this.dragMinSpeed_add - gun.dragMinSpeed;
            this.spread_delta = gun.spread * this.spread_mult + this.spread_add - gun.spread;
            this.evenSpread_delta = gun.evenSpread * this.evenSpread_mult + this.evenSpread_add - gun.evenSpread;
            this.percentageDamage_delta = gun.percentageDamage * this.percentageDamage_mult + this.percentageDamage_add - gun.percentageDamage;
            this.slow_delta = gun.slow * this.slow_mult + this.slow_add - gun.slow;
            this.destroyBulletAfter_delta = gun.destroyBulletAfter * this.destroyBulletAfter_mult + this.destroyBulletAfter_add - gun.destroyBulletAfter;
            this.forceSpecificAttackSpeed_delta = gun.forceSpecificAttackSpeed * this.forceSpecificAttackSpeed_mult + this.forceSpecificAttackSpeed_add - gun.forceSpecificAttackSpeed;
            this.explodeNearEnemyRange_delta = gun.explodeNearEnemyRange * this.explodeNearEnemyRange_mult + this.explodeNearEnemyRange_add - gun.explodeNearEnemyRange;
            this.explodeNearEnemyDamage_delta = gun.explodeNearEnemyDamage * this.explodeNearEnemyDamage_mult + this.explodeNearEnemyDamage_add - gun.explodeNearEnemyDamage;
            this.hitMovementMultiplier_delta = gun.hitMovementMultiplier * this.hitMovementMultiplier_mult + this.hitMovementMultiplier_add - gun.hitMovementMultiplier;
            this.attackSpeedMultiplier_delta = gun.attackSpeedMultiplier * this.attackSpeedMultiplier_mult + this.attackSpeedMultiplier_add - gun.attackSpeedMultiplier;

            this.minDistanceMultiplier_delta = gun.GetAdditionalData().minDistanceMultiplier * this.minDistanceMultiplier_mult + this.minDistanceMultiplier_add - gun.GetAdditionalData().minDistanceMultiplier;

            // apply everything
            gun.damage += this.damage_delta;
            gun.recoilMuiltiplier += this.recoilMuiltiplier_delta;
            gun.knockback += this.knockback_delta;
            gun.attackSpeed += this.attackSpeed_delta;
            gun.projectileSpeed += this.projectileSpeed_delta;
            gun.projectielSimulatonSpeed += this.projectielSimulatonSpeed_delta;
            gun.gravity += this.gravity_delta;
            gun.damageAfterDistanceMultiplier += this.damageAfterDistanceMultiplier_delta;
            gun.bulletDamageMultiplier += this.bulletDamageMultiplier_delta;
            gun.multiplySpread += this.multiplySpread_delta;
            gun.size += this.size_delta;
            gun.timeToReachFullMovementMultiplier += this.timeToReachFullMovementMultiplier_delta;
            gun.numberOfProjectiles += this.numberOfProjectiles_delta;
            gun.bursts += this.bursts_delta;
            gun.reflects += this.reflects_delta;
            gun.smartBounce += this.smartBounce_delta;
            gun.randomBounces += this.randomBounces_delta;
            gun.timeBetweenBullets += this.timeBetweenBullets_delta;
            gun.projectileSize += this.projectileSize_delta;
            gun.speedMOnBounce += this.speedMOnBounce_delta;
            gun.dmgMOnBounce += this.dmgMOnBounce_delta;
            gun.drag += this.drag_delta;
            gun.dragMinSpeed += this.dragMinSpeed_delta;
            gun.spread += this.spread_delta;
            gun.evenSpread += this.evenSpread_delta;
            gun.percentageDamage += this.percentageDamage_delta;
            gun.slow += this.slow_delta;
            gun.destroyBulletAfter += this.destroyBulletAfter_delta;
            gun.forceSpecificAttackSpeed += this.forceSpecificAttackSpeed_delta;
            gun.explodeNearEnemyRange += this.explodeNearEnemyRange_delta;
            gun.explodeNearEnemyDamage += this.explodeNearEnemyDamage_delta;
            gun.hitMovementMultiplier += this.hitMovementMultiplier_delta;
            gun.attackSpeedMultiplier += this.attackSpeedMultiplier_delta;

            List<ObjectsToSpawn> gunObjectsToSpawn = new List<ObjectsToSpawn>(gun.objectsToSpawn);

            foreach (ObjectsToSpawn objectToSpawn in this.objectsToSpawn_add)
            {
                gunObjectsToSpawn.Add(objectToSpawn);
            }
            gun.objectsToSpawn = gunObjectsToSpawn.ToArray();

            this.gunColorEffect = gun.player.gameObject.AddComponent<GunColorEffect>();
            this.gunColorEffect.SetColor(this.projectileColor);

            gun.GetAdditionalData().minDistanceMultiplier += this.minDistanceMultiplier_delta;


        }

        public static void RemoveGunStatModifier(GunStatModifier gunStatModifier, Gun gun)
        {
            gun.damage -= gunStatModifier.damage_delta;
            gun.recoilMuiltiplier -= gunStatModifier.recoilMuiltiplier_delta;
            gun.knockback -= gunStatModifier.knockback_delta;
            gun.attackSpeed -= gunStatModifier.attackSpeed_delta;
            gun.projectileSpeed -= gunStatModifier.projectileSpeed_delta;
            gun.projectielSimulatonSpeed -= gunStatModifier.projectielSimulatonSpeed_delta;
            gun.gravity -= gunStatModifier.gravity_delta;
            gun.damageAfterDistanceMultiplier -= gunStatModifier.damageAfterDistanceMultiplier_delta;
            gun.bulletDamageMultiplier -= gunStatModifier.bulletDamageMultiplier_delta;
            gun.multiplySpread -= gunStatModifier.multiplySpread_delta;
            gun.size -= gunStatModifier.size_delta;
            gun.timeToReachFullMovementMultiplier -= gunStatModifier.timeToReachFullMovementMultiplier_delta;
            gun.numberOfProjectiles -= gunStatModifier.numberOfProjectiles_delta;
            gun.bursts -= gunStatModifier.bursts_delta;
            gun.reflects -= gunStatModifier.reflects_delta;
            gun.smartBounce -= gunStatModifier.smartBounce_delta;
            gun.randomBounces -= gunStatModifier.randomBounces_delta;
            gun.timeBetweenBullets -= gunStatModifier.timeBetweenBullets_delta;
            gun.projectileSize -= gunStatModifier.projectileSize_delta;
            gun.speedMOnBounce -= gunStatModifier.speedMOnBounce_delta;
            gun.dmgMOnBounce -= gunStatModifier.dmgMOnBounce_delta;
            gun.drag -= gunStatModifier.drag_delta;
            gun.dragMinSpeed -= gunStatModifier.dragMinSpeed_delta;
            gun.spread -= gunStatModifier.spread_delta;
            gun.evenSpread -= gunStatModifier.evenSpread_delta;
            gun.percentageDamage -= gunStatModifier.percentageDamage_delta;
            gun.slow -= gunStatModifier.slow_delta;
            gun.destroyBulletAfter -= gunStatModifier.destroyBulletAfter_delta;
            gun.forceSpecificAttackSpeed -= gunStatModifier.forceSpecificAttackSpeed_delta;
            gun.explodeNearEnemyRange -= gunStatModifier.explodeNearEnemyRange_delta;
            gun.explodeNearEnemyDamage -= gunStatModifier.explodeNearEnemyDamage_delta;
            gun.hitMovementMultiplier -= gunStatModifier.hitMovementMultiplier_delta;
            gun.attackSpeedMultiplier -= gunStatModifier.attackSpeedMultiplier_delta;

            List<ObjectsToSpawn> gunObjectsToSpawn = new List<ObjectsToSpawn>(gun.objectsToSpawn);

            foreach (ObjectsToSpawn objectToSpawn in gunStatModifier.objectsToSpawn_add)
            {
                gunObjectsToSpawn.Remove(objectToSpawn);
            }
            gun.objectsToSpawn = gunObjectsToSpawn.ToArray();

            if (gunStatModifier.gunColorEffect != null) { gunStatModifier.gunColorEffect.Destroy(); }

            gun.GetAdditionalData().minDistanceMultiplier -= gunStatModifier.minDistanceMultiplier_delta;

            // reset deltas
            gunStatModifier.damage_delta = 0f;
            gunStatModifier.recoilMuiltiplier_delta = 0f;
            gunStatModifier.knockback_delta = 0f;
            gunStatModifier.attackSpeed_delta = 0f;
            gunStatModifier.projectileSpeed_delta = 0f;
            gunStatModifier.projectielSimulatonSpeed_delta = 0f;
            gunStatModifier.gravity_delta = 0f;
            gunStatModifier.damageAfterDistanceMultiplier_delta = 0f;
            gunStatModifier.bulletDamageMultiplier_delta = 0f;
            gunStatModifier.multiplySpread_delta = 0f;
            gunStatModifier.size_delta = 0f;
            gunStatModifier.timeToReachFullMovementMultiplier_delta = 0f;
            gunStatModifier.numberOfProjectiles_delta = 0;
            gunStatModifier.bursts_delta = 0;
            gunStatModifier.reflects_delta = 0;
            gunStatModifier.smartBounce_delta = 0;
            gunStatModifier.randomBounces_delta = 0;
            gunStatModifier.timeBetweenBullets_delta = 0f;
            gunStatModifier.projectileSize_delta = 0f;
            gunStatModifier.speedMOnBounce_delta = 0f;
            gunStatModifier.dmgMOnBounce_delta = 0f;
            gunStatModifier.drag_delta = 0f;
            gunStatModifier.dragMinSpeed_delta = 0f;
            gunStatModifier.spread_delta = 0f;
            gunStatModifier.evenSpread_delta = 0f;
            gunStatModifier.percentageDamage_delta = 0f;
            gunStatModifier.slow_delta = 0f;
            gunStatModifier.destroyBulletAfter_delta = 0f;
            gunStatModifier.forceSpecificAttackSpeed_delta = 0f;
            gunStatModifier.explodeNearEnemyRange_delta = 0f;
            gunStatModifier.explodeNearEnemyDamage_delta = 0f;
            gunStatModifier.hitMovementMultiplier_delta = 0f;
            gunStatModifier.attackSpeedMultiplier_delta = 0f;

            gunStatModifier.gunColorEffect = null;

            // extra
            gunStatModifier.minDistanceMultiplier_delta = 0f;

        }
        public void RemoveGunStatModifier(Gun gun)
        {
            gun.damage -= this.damage_delta;
            gun.recoilMuiltiplier -= this.recoilMuiltiplier_delta;
            gun.knockback -= this.knockback_delta;
            gun.attackSpeed -= this.attackSpeed_delta;
            gun.projectileSpeed -= this.projectileSpeed_delta;
            gun.projectielSimulatonSpeed -= this.projectielSimulatonSpeed_delta;
            gun.gravity -= this.gravity_delta;
            gun.damageAfterDistanceMultiplier -= this.damageAfterDistanceMultiplier_delta;
            gun.bulletDamageMultiplier -= this.bulletDamageMultiplier_delta;
            gun.multiplySpread -= this.multiplySpread_delta;
            gun.size -= this.size_delta;
            gun.timeToReachFullMovementMultiplier -= this.timeToReachFullMovementMultiplier_delta;
            gun.numberOfProjectiles -= this.numberOfProjectiles_delta;
            gun.bursts -= this.bursts_delta;
            gun.reflects -= this.reflects_delta;
            gun.smartBounce -= this.smartBounce_delta;
            gun.randomBounces -= this.randomBounces_delta;
            gun.timeBetweenBullets -= this.timeBetweenBullets_delta;
            gun.projectileSize -= this.projectileSize_delta;
            gun.speedMOnBounce -= this.speedMOnBounce_delta;
            gun.dmgMOnBounce -= this.dmgMOnBounce_delta;
            gun.drag -= this.drag_delta;
            gun.dragMinSpeed -= this.dragMinSpeed_delta;
            gun.spread -= this.spread_delta;
            gun.evenSpread -= this.evenSpread_delta;
            gun.percentageDamage -= this.percentageDamage_delta;
            gun.slow -= this.slow_delta;
            gun.destroyBulletAfter -= this.destroyBulletAfter_delta;
            gun.forceSpecificAttackSpeed -= this.forceSpecificAttackSpeed_delta;
            gun.explodeNearEnemyRange -= this.explodeNearEnemyRange_delta;
            gun.explodeNearEnemyDamage -= this.explodeNearEnemyDamage_delta;
            gun.hitMovementMultiplier -= this.hitMovementMultiplier_delta;
            gun.attackSpeedMultiplier -= this.attackSpeedMultiplier_delta;

            List<ObjectsToSpawn> gunObjectsToSpawn = new List<ObjectsToSpawn>(gun.objectsToSpawn);

            foreach (ObjectsToSpawn objectToSpawn in this.objectsToSpawn_add)
            {
                gunObjectsToSpawn.Remove(objectToSpawn);
            }
            gun.objectsToSpawn = gunObjectsToSpawn.ToArray();

            if (this.gunColorEffect != null) { this.gunColorEffect.Destroy(); }

            gun.GetAdditionalData().minDistanceMultiplier -= this.minDistanceMultiplier_delta;

            // reset deltas
            this.damage_delta = 0f;
            this.recoilMuiltiplier_delta = 0f;
            this.knockback_delta = 0f;
            this.attackSpeed_delta = 0f;
            this.projectileSpeed_delta = 0f;
            this.projectielSimulatonSpeed_delta = 0f;
            this.gravity_delta = 0f;
            this.damageAfterDistanceMultiplier_delta = 0f;
            this.bulletDamageMultiplier_delta = 0f;
            this.multiplySpread_delta = 0f;
            this.size_delta = 0f;
            this.timeToReachFullMovementMultiplier_delta = 0f;
            this.numberOfProjectiles_delta = 0;
            this.bursts_delta = 0;
            this.reflects_delta = 0;
            this.smartBounce_delta = 0;
            this.randomBounces_delta = 0;
            this.timeBetweenBullets_delta = 0f;
            this.projectileSize_delta = 0f;
            this.speedMOnBounce_delta = 0f;
            this.dmgMOnBounce_delta = 0f;
            this.drag_delta = 0f;
            this.dragMinSpeed_delta = 0f;
            this.spread_delta = 0f;
            this.evenSpread_delta = 0f;
            this.percentageDamage_delta = 0f;
            this.slow_delta = 0f;
            this.destroyBulletAfter_delta = 0f;
            this.forceSpecificAttackSpeed_delta = 0f;
            this.explodeNearEnemyRange_delta = 0f;
            this.explodeNearEnemyDamage_delta = 0f;
            this.hitMovementMultiplier_delta = 0f;
            this.attackSpeedMultiplier_delta = 0f;

            this.gunColorEffect = null;

            // extra
            this.minDistanceMultiplier_delta = 0f;

        }


    }


    public class CharacterStatModifiersModifier
    {
        public List<GameObject> objectsToAddToPlayer = new List<GameObject>();
        public float sizeMultiplier_add = 0f;
        public float sizeMultiplier_mult = 1f;
        public float health_add = 0f;
        public float health_mult = 1f;
        public float movementSpeed_add = 0f;
        public float movementSpeed_mult = 1f;
        public float jump_add = 0f;
        public float jump_mult = 1f;
        public float gravity_add = 0f;
        public float gravity_mult = 1f;
        public float slow_add = 0f;
        public float slow_mult = 1f;
        public float slowSlow_add = 0f;
        public float slowSlow_mult = 1f;
        public float fastSlow_add = 0f;
        public float fastSlow_mult = 1f;
        public float secondsToTakeDamageOver_add = 0f;
        public float secondsToTakeDamageOver_mult = 1f;
        public int numberOfJumps_add = 0;
        public int numberOfJumps_mult = 1;
        public float regen_add = 0f;
        public float regen_mult = 1f;
        public float lifeSteal_add = 0f;
        public float lifeSteal_mult = 1f;
        public int respawns_add = 0;
        public int respawns_mult = 1;
        public float tasteOfBloodSpeed_add = 0f;
        public float tasteOfBloodSpeed_mult = 1f;
        public float rageSpeed_add = 0f;
        public float rageSpeed_mult = 1f;
        public float attackSpeedMultiplier_add = 0f;
        public float attackSpeedMultiplier_mult = 1f;

        // extra stuff from extensions
        public float gravityMultiplierOnDoDamage_add = 0f;
        public float gravityMultiplierOnDoDamage_mult = 1f;
        public float gravityDurationOnDoDamage_add = 0f;
        public float gravityDurationOnDoDamage_mult = 1f;
        public float defaultGravityForce_add = 0f;
        public float defaultGravityForce_mult = 1f;
        public float defaultGravityExponent_add = 0f;
        public float defaultGravityExponent_mult = 1f;
        public int murder_add = 0;
        public int murder_mult = 1;

        private List<GameObject> objectsAddedToPlayer = new List<GameObject>();
        private float sizeMultiplier_delta = 0f;
        private float health_delta = 0f;
        private float movementSpeed_delta = 0f;
        private float jump_delta = 0f;
        private float gravity_delta = 0f;
        private float slow_delta = 0f;
        private float slowSlow_delta = 0f;
        private float fastSlow_delta = 0f;
        private float secondsToTakeDamageOver_delta = 0f;
        private int numberOfJumps_delta = 0;
        private float regen_delta = 0f;
        private float lifeSteal_delta = 0f;
        private int respawns_delta = 0;
        private float tasteOfBloodSpeed_delta = 0f;
        private float rageSpeed_delta = 0f;
        private float attackSpeedMultiplier_delta = 0f;

        // extra stuff from extensions
        private float gravityMultiplierOnDoDamage_delta = 0f;
        private float gravityDurationOnDoDamage_delta = 0f;
        private float defaultGravityForce_delta = 0f;
        private float defaultGravityExponent_delta = 0f;
        private int murder_delta = 0;

        public static void ApplyCharacterStatModifiersModifier(CharacterStatModifiersModifier characterStatModifiersModifier, CharacterStatModifiers characterStatModifiers)
        {
            characterStatModifiersModifier.sizeMultiplier_delta = characterStatModifiers.sizeMultiplier * characterStatModifiersModifier.sizeMultiplier_mult + characterStatModifiersModifier.sizeMultiplier_add - characterStatModifiers.sizeMultiplier;
            characterStatModifiersModifier.health_delta = characterStatModifiers.health * characterStatModifiersModifier.health_mult + characterStatModifiersModifier.health_add - characterStatModifiers.health;
            characterStatModifiersModifier.movementSpeed_delta = characterStatModifiers.movementSpeed * characterStatModifiersModifier.movementSpeed_mult + characterStatModifiersModifier.movementSpeed_add - characterStatModifiers.movementSpeed;
            characterStatModifiersModifier.jump_delta = characterStatModifiers.jump * characterStatModifiersModifier.jump_mult + characterStatModifiersModifier.jump_add - characterStatModifiers.jump;
            characterStatModifiersModifier.gravity_delta = characterStatModifiers.gravity * characterStatModifiersModifier.gravity_mult + characterStatModifiersModifier.gravity_add - characterStatModifiers.gravity;
            characterStatModifiersModifier.slow_delta = characterStatModifiers.slow * characterStatModifiersModifier.slow_mult + characterStatModifiersModifier.slow_add - characterStatModifiers.slow;
            characterStatModifiersModifier.slowSlow_delta = characterStatModifiers.slowSlow * characterStatModifiersModifier.slowSlow_mult + characterStatModifiersModifier.slowSlow_add - characterStatModifiers.slowSlow;
            characterStatModifiersModifier.fastSlow_delta = characterStatModifiers.fastSlow * characterStatModifiersModifier.fastSlow_mult + characterStatModifiersModifier.fastSlow_add - characterStatModifiers.fastSlow;
            characterStatModifiersModifier.secondsToTakeDamageOver_delta = characterStatModifiers.secondsToTakeDamageOver * characterStatModifiersModifier.secondsToTakeDamageOver_mult + characterStatModifiersModifier.secondsToTakeDamageOver_add - characterStatModifiers.secondsToTakeDamageOver;
            characterStatModifiersModifier.numberOfJumps_delta = characterStatModifiers.numberOfJumps * characterStatModifiersModifier.numberOfJumps_mult + characterStatModifiersModifier.numberOfJumps_add - characterStatModifiers.numberOfJumps;
            characterStatModifiersModifier.regen_delta = characterStatModifiers.regen * characterStatModifiersModifier.regen_mult + characterStatModifiersModifier.regen_add - characterStatModifiers.regen;
            characterStatModifiersModifier.lifeSteal_delta = characterStatModifiers.lifeSteal * characterStatModifiersModifier.lifeSteal_mult + characterStatModifiersModifier.lifeSteal_add - characterStatModifiers.lifeSteal;
            characterStatModifiersModifier.respawns_delta = characterStatModifiers.respawns * characterStatModifiersModifier.respawns_mult + characterStatModifiersModifier.respawns_add - characterStatModifiers.respawns;
            characterStatModifiersModifier.tasteOfBloodSpeed_delta = characterStatModifiers.tasteOfBloodSpeed * characterStatModifiersModifier.tasteOfBloodSpeed_mult + characterStatModifiersModifier.tasteOfBloodSpeed_add - characterStatModifiers.tasteOfBloodSpeed;
            characterStatModifiersModifier.rageSpeed_delta = characterStatModifiers.rageSpeed * characterStatModifiersModifier.rageSpeed_mult + characterStatModifiersModifier.rageSpeed_add - characterStatModifiers.rageSpeed;
            characterStatModifiersModifier.attackSpeedMultiplier_delta = characterStatModifiers.attackSpeedMultiplier * characterStatModifiersModifier.attackSpeedMultiplier_mult + characterStatModifiersModifier.attackSpeedMultiplier_add - characterStatModifiers.attackSpeedMultiplier;

            // extra stuff from extensions
            characterStatModifiersModifier.gravityMultiplierOnDoDamage_delta = characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage * characterStatModifiersModifier.gravityMultiplierOnDoDamage_mult + characterStatModifiersModifier.gravityMultiplierOnDoDamage_add - characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage;
            characterStatModifiersModifier.gravityDurationOnDoDamage_delta = characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage * characterStatModifiersModifier.gravityDurationOnDoDamage_mult + characterStatModifiersModifier.gravityDurationOnDoDamage_add - characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage;
            characterStatModifiersModifier.defaultGravityForce_delta = characterStatModifiers.GetAdditionalData().defaultGravityForce * characterStatModifiersModifier.defaultGravityForce_mult + characterStatModifiersModifier.defaultGravityForce_add - characterStatModifiers.GetAdditionalData().defaultGravityForce;
            characterStatModifiersModifier.defaultGravityExponent_delta = characterStatModifiers.GetAdditionalData().defaultGravityExponent * characterStatModifiersModifier.defaultGravityExponent_mult + characterStatModifiersModifier.defaultGravityExponent_add - characterStatModifiers.GetAdditionalData().defaultGravityExponent;
            characterStatModifiersModifier.murder_delta = characterStatModifiers.GetAdditionalData().murder * characterStatModifiersModifier.murder_mult + characterStatModifiersModifier.murder_add - characterStatModifiers.GetAdditionalData().murder;

            characterStatModifiers.sizeMultiplier += characterStatModifiersModifier.sizeMultiplier_delta;
            characterStatModifiers.health += characterStatModifiersModifier.health_delta;
            characterStatModifiers.movementSpeed += characterStatModifiersModifier.movementSpeed_delta;
            characterStatModifiers.jump += characterStatModifiersModifier.jump_delta;
            characterStatModifiers.gravity += characterStatModifiersModifier.gravity_delta;
            characterStatModifiers.slow += characterStatModifiersModifier.slow_delta;
            characterStatModifiers.slowSlow += characterStatModifiersModifier.slowSlow_delta;
            characterStatModifiers.fastSlow += characterStatModifiersModifier.fastSlow_delta;
            characterStatModifiers.secondsToTakeDamageOver += characterStatModifiersModifier.secondsToTakeDamageOver_delta;
            characterStatModifiers.numberOfJumps += characterStatModifiersModifier.numberOfJumps_delta;
            characterStatModifiers.regen += characterStatModifiersModifier.regen_delta;
            characterStatModifiers.lifeSteal += characterStatModifiersModifier.lifeSteal_delta;
            characterStatModifiers.respawns += characterStatModifiersModifier.respawns_delta;
            characterStatModifiers.tasteOfBloodSpeed += characterStatModifiersModifier.tasteOfBloodSpeed_delta;
            characterStatModifiers.rageSpeed += characterStatModifiersModifier.rageSpeed_delta;
            characterStatModifiers.attackSpeedMultiplier += characterStatModifiersModifier.attackSpeedMultiplier_delta;

            // extra stuff from extensions
            characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage += characterStatModifiersModifier.gravityMultiplierOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage += characterStatModifiersModifier.gravityDurationOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityForce += characterStatModifiersModifier.defaultGravityForce_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityExponent += characterStatModifiersModifier.defaultGravityExponent_delta;
            characterStatModifiers.GetAdditionalData().murder += characterStatModifiersModifier.murder_delta;

            // special stuff
            Player player = characterStatModifiers.GetComponent<Player>();
            foreach (GameObject objectToAddToPlayer in characterStatModifiersModifier.objectsToAddToPlayer)
            {
                GameObject instantiatedObject = UnityEngine.Object.Instantiate<GameObject>(objectToAddToPlayer, player.transform.position, player.transform.rotation, player.transform);

                characterStatModifiersModifier.objectsAddedToPlayer.Add(instantiatedObject);
                characterStatModifiers.objectsAddedToPlayer.Add(instantiatedObject);
            }

            // update the characterStatModifiers
            characterStatModifiers.WasUpdated();
        }
        public void ApplyCharacterStatModifiersModifier(CharacterStatModifiers characterStatModifiers)
        {
            this.sizeMultiplier_delta = characterStatModifiers.sizeMultiplier * this.sizeMultiplier_mult + this.sizeMultiplier_add - characterStatModifiers.sizeMultiplier;
            this.health_delta = characterStatModifiers.health * this.health_mult + this.health_add - characterStatModifiers.health;
            this.movementSpeed_delta = characterStatModifiers.movementSpeed * this.movementSpeed_mult + this.movementSpeed_add - characterStatModifiers.movementSpeed;
            this.jump_delta = characterStatModifiers.jump * this.jump_mult + this.jump_add - characterStatModifiers.jump;
            this.gravity_delta = characterStatModifiers.gravity * this.gravity_mult + this.gravity_add - characterStatModifiers.gravity;
            this.slow_delta = characterStatModifiers.slow * this.slow_mult + this.slow_add - characterStatModifiers.slow;
            this.slowSlow_delta = characterStatModifiers.slowSlow * this.slowSlow_mult + this.slowSlow_add - characterStatModifiers.slowSlow;
            this.fastSlow_delta = characterStatModifiers.fastSlow * this.fastSlow_mult + this.fastSlow_add - characterStatModifiers.fastSlow;
            this.secondsToTakeDamageOver_delta = characterStatModifiers.secondsToTakeDamageOver * this.secondsToTakeDamageOver_mult + this.secondsToTakeDamageOver_add - characterStatModifiers.secondsToTakeDamageOver;
            this.numberOfJumps_delta = characterStatModifiers.numberOfJumps * this.numberOfJumps_mult + this.numberOfJumps_add - characterStatModifiers.numberOfJumps;
            this.regen_delta = characterStatModifiers.regen * this.regen_mult + this.regen_add - characterStatModifiers.regen;
            this.lifeSteal_delta = characterStatModifiers.lifeSteal * this.lifeSteal_mult + this.lifeSteal_add - characterStatModifiers.lifeSteal;
            this.respawns_delta = characterStatModifiers.respawns * this.respawns_mult + this.respawns_add - characterStatModifiers.respawns;
            this.tasteOfBloodSpeed_delta = characterStatModifiers.tasteOfBloodSpeed * this.tasteOfBloodSpeed_mult + this.tasteOfBloodSpeed_add - characterStatModifiers.tasteOfBloodSpeed;
            this.rageSpeed_delta = characterStatModifiers.rageSpeed * this.rageSpeed_mult + this.rageSpeed_add - characterStatModifiers.rageSpeed;
            this.attackSpeedMultiplier_delta = characterStatModifiers.attackSpeedMultiplier * this.attackSpeedMultiplier_mult + this.attackSpeedMultiplier_add - characterStatModifiers.attackSpeedMultiplier;

            // extra stuff from extensions
            this.gravityMultiplierOnDoDamage_delta = characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage * this.gravityMultiplierOnDoDamage_mult + this.gravityMultiplierOnDoDamage_add - characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage;
            this.gravityDurationOnDoDamage_delta = characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage * this.gravityDurationOnDoDamage_mult + this.gravityDurationOnDoDamage_add - characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage;
            this.defaultGravityForce_delta = characterStatModifiers.GetAdditionalData().defaultGravityForce * this.defaultGravityForce_mult + this.defaultGravityForce_add - characterStatModifiers.GetAdditionalData().defaultGravityForce;
            this.defaultGravityExponent_delta = characterStatModifiers.GetAdditionalData().defaultGravityExponent * this.defaultGravityExponent_mult + this.defaultGravityExponent_add - characterStatModifiers.GetAdditionalData().defaultGravityExponent;
            this.murder_delta = characterStatModifiers.GetAdditionalData().murder * this.murder_mult + this.murder_add - characterStatModifiers.GetAdditionalData().murder;
            
            
            characterStatModifiers.sizeMultiplier += this.sizeMultiplier_delta;
            characterStatModifiers.health += this.health_delta;
            characterStatModifiers.movementSpeed += this.movementSpeed_delta;
            characterStatModifiers.jump += this.jump_delta;
            characterStatModifiers.gravity += this.gravity_delta;
            characterStatModifiers.slow += this.slow_delta;
            characterStatModifiers.slowSlow += this.slowSlow_delta;
            characterStatModifiers.fastSlow += this.fastSlow_delta;
            characterStatModifiers.secondsToTakeDamageOver += this.secondsToTakeDamageOver_delta;
            characterStatModifiers.numberOfJumps += this.numberOfJumps_delta;
            characterStatModifiers.regen += this.regen_delta;
            characterStatModifiers.lifeSteal += this.lifeSteal_delta;
            characterStatModifiers.respawns += this.respawns_delta;
            characterStatModifiers.tasteOfBloodSpeed += this.tasteOfBloodSpeed_delta;
            characterStatModifiers.rageSpeed += this.rageSpeed_delta;
            characterStatModifiers.attackSpeedMultiplier += this.attackSpeedMultiplier_delta;

            // extra stuff from extensions
            characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage += this.gravityMultiplierOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage += this.gravityDurationOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityForce += this.defaultGravityForce_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityExponent += this.defaultGravityExponent_delta;
            characterStatModifiers.GetAdditionalData().murder += this.murder_delta;

            // special stuff
            Player player = characterStatModifiers.GetComponent<Player>();
            foreach (GameObject objectToAddToPlayer in this.objectsToAddToPlayer)
            {
                GameObject instantiatedObject = UnityEngine.Object.Instantiate<GameObject>(objectToAddToPlayer, player.transform.position, player.transform.rotation, player.transform);

                this.objectsAddedToPlayer.Add(instantiatedObject);
                characterStatModifiers.objectsAddedToPlayer.Add(instantiatedObject);
            }


            // update the characterStatModifiers
            characterStatModifiers.WasUpdated();
        }
        public static void RemoveCharacterStatModifiersModifier(CharacterStatModifiersModifier characterStatModifiersModifier, CharacterStatModifiers characterStatModifiers)
        {
            characterStatModifiers.sizeMultiplier -= characterStatModifiersModifier.sizeMultiplier_delta;
            characterStatModifiers.health -= characterStatModifiersModifier.health_delta;
            characterStatModifiers.movementSpeed -= characterStatModifiersModifier.movementSpeed_delta;
            characterStatModifiers.jump -= characterStatModifiersModifier.jump_delta;
            characterStatModifiers.gravity -= characterStatModifiersModifier.gravity_delta;
            characterStatModifiers.slow -= characterStatModifiersModifier.slow_delta;
            characterStatModifiers.slowSlow -= characterStatModifiersModifier.slowSlow_delta;
            characterStatModifiers.fastSlow -= characterStatModifiersModifier.fastSlow_delta;
            characterStatModifiers.secondsToTakeDamageOver -= characterStatModifiersModifier.secondsToTakeDamageOver_delta;
            characterStatModifiers.numberOfJumps -= characterStatModifiersModifier.numberOfJumps_delta;
            characterStatModifiers.regen -= characterStatModifiersModifier.regen_delta;
            characterStatModifiers.lifeSteal -= characterStatModifiersModifier.lifeSteal_delta;
            characterStatModifiers.respawns -= characterStatModifiersModifier.respawns_delta;
            characterStatModifiers.tasteOfBloodSpeed -= characterStatModifiersModifier.tasteOfBloodSpeed_delta;
            characterStatModifiers.rageSpeed -= characterStatModifiersModifier.rageSpeed_delta;
            characterStatModifiers.attackSpeedMultiplier -= characterStatModifiersModifier.attackSpeedMultiplier_delta;

            // extra stuff from extensions
            characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage -= characterStatModifiersModifier.gravityMultiplierOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage -= characterStatModifiersModifier.gravityDurationOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityForce -= characterStatModifiersModifier.defaultGravityForce_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityExponent -= characterStatModifiersModifier.defaultGravityExponent_delta;
            characterStatModifiers.GetAdditionalData().murder -= characterStatModifiersModifier.murder_delta;

            // special stuff
            foreach (GameObject objectAddedToPlayer in characterStatModifiersModifier.objectsAddedToPlayer)
            {
                characterStatModifiers.objectsAddedToPlayer.Remove(objectAddedToPlayer);
                if (objectAddedToPlayer != null) { UnityEngine.Object.Destroy(objectAddedToPlayer); }

            }

            // reset deltas
            characterStatModifiersModifier.objectsAddedToPlayer = new List<GameObject>();
            characterStatModifiersModifier.sizeMultiplier_delta = 0f;
            characterStatModifiersModifier.health_delta = 0f;
            characterStatModifiersModifier.movementSpeed_delta = 0f;
            characterStatModifiersModifier.jump_delta = 0f;
            characterStatModifiersModifier.gravity_delta = 0f;
            characterStatModifiersModifier.slow_delta = 0f;
            characterStatModifiersModifier.slowSlow_delta = 0f;
            characterStatModifiersModifier.fastSlow_delta = 0f;
            characterStatModifiersModifier.secondsToTakeDamageOver_delta = 0f;
            characterStatModifiersModifier.numberOfJumps_delta = 0;
            characterStatModifiersModifier.regen_delta = 0f;
            characterStatModifiersModifier.lifeSteal_delta = 0f;
            characterStatModifiersModifier.respawns_delta = 0;
            characterStatModifiersModifier.tasteOfBloodSpeed_delta = 0f;
            characterStatModifiersModifier.rageSpeed_delta = 0f;
            characterStatModifiersModifier.attackSpeedMultiplier_delta = 0f;

            // extra stuff from extensions
            characterStatModifiersModifier.gravityMultiplierOnDoDamage_delta = 0f;
            characterStatModifiersModifier.gravityDurationOnDoDamage_delta = 0f;
            characterStatModifiersModifier.defaultGravityForce_delta = 0f;
            characterStatModifiersModifier.defaultGravityExponent_delta = 0f;
            characterStatModifiersModifier.murder_delta = 0;

            // update the characterStatModifiers
            characterStatModifiers.WasUpdated();
        }
        public void RemoveCharacterStatModifiersModifier(CharacterStatModifiers characterStatModifiers)
        {
            characterStatModifiers.sizeMultiplier -= this.sizeMultiplier_delta;
            characterStatModifiers.health -= this.health_delta;
            characterStatModifiers.movementSpeed -= this.movementSpeed_delta;
            characterStatModifiers.jump -= this.jump_delta;
            characterStatModifiers.gravity -= this.gravity_delta;
            characterStatModifiers.slow -= this.slow_delta;
            characterStatModifiers.slowSlow -= this.slowSlow_delta;
            characterStatModifiers.fastSlow -= this.fastSlow_delta;
            characterStatModifiers.secondsToTakeDamageOver -= this.secondsToTakeDamageOver_delta;
            characterStatModifiers.numberOfJumps -= this.numberOfJumps_delta;
            characterStatModifiers.regen -= this.regen_delta;
            characterStatModifiers.lifeSteal -= this.lifeSteal_delta;
            characterStatModifiers.respawns -= this.respawns_delta;
            characterStatModifiers.tasteOfBloodSpeed -= this.tasteOfBloodSpeed_delta;
            characterStatModifiers.rageSpeed -= this.rageSpeed_delta;
            characterStatModifiers.attackSpeedMultiplier -= this.attackSpeedMultiplier_delta;

            // extra stuff from extensions
            characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage -= this.gravityMultiplierOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage -= this.gravityDurationOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityForce -= this.defaultGravityForce_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityExponent -= this.defaultGravityExponent_delta;
            characterStatModifiers.GetAdditionalData().murder -= this.murder_delta;

            // special stuff
            foreach (GameObject objectAddedToPlayer in this.objectsAddedToPlayer)
            {
                characterStatModifiers.objectsAddedToPlayer.Remove(objectAddedToPlayer);
                if (objectAddedToPlayer != null) { UnityEngine.Object.Destroy(objectAddedToPlayer); }

            }

            // reset deltas
            this.objectsAddedToPlayer = new List<GameObject>();
            this.sizeMultiplier_delta = 0f;
            this.health_delta = 0f;
            this.movementSpeed_delta = 0f;
            this.jump_delta = 0f;
            this.gravity_delta = 0f;
            this.slow_delta = 0f;
            this.slowSlow_delta = 0f;
            this.fastSlow_delta = 0f;
            this.secondsToTakeDamageOver_delta = 0f;
            this.numberOfJumps_delta = 0;
            this.regen_delta = 0f;
            this.lifeSteal_delta = 0f;
            this.respawns_delta = 0;
            this.tasteOfBloodSpeed_delta = 0f;
            this.rageSpeed_delta = 0f;
            this.attackSpeedMultiplier_delta = 0f;

            // extra stuff from extensions
            this.gravityMultiplierOnDoDamage_delta = 0f;
            this.gravityDurationOnDoDamage_delta = 0f;
            this.defaultGravityForce_delta = 0f;
            this.defaultGravityExponent_delta = 0f;
            this.murder_delta = 0;

            // update the characterStatModifiers
            characterStatModifiers.WasUpdated();

        }
    }
    public class GravityModifier
    {
        public float gravityForce_add = 0f;
        public float gravityForce_mult = 1f;
        public float exponent_add = 0f;
        public float exponent_mult = 1f;

        private float gravityForce_delta = 0f;
        private float exponent_delta = 0f;


        public static void ApplyGravityModifier(GravityModifier gravityModifier, Gravity gravity)
        {
            gravityModifier.gravityForce_delta = gravity.gravityForce * gravityModifier.gravityForce_mult + gravityModifier.gravityForce_add - gravity.gravityForce;
            gravityModifier.exponent_delta = gravity.exponent * gravityModifier.exponent_mult + gravityModifier.exponent_add - gravity.exponent;

            gravity.gravityForce += gravityModifier.gravityForce_delta;
            gravity.exponent += gravityModifier.exponent_delta;
        
        }
        public void ApplyGravityModifier(Gravity gravity)
        {
            this.gravityForce_delta = gravity.gravityForce * this.gravityForce_mult + this.gravityForce_add - gravity.gravityForce;
            this.exponent_delta = gravity.exponent * this.exponent_mult + this.exponent_add - gravity.exponent;

            gravity.gravityForce += this.gravityForce_delta;
            gravity.exponent += this.exponent_delta;

        }
        public static void RemoveGravityModifier(GravityModifier gravityModifier, Gravity gravity)
        {

            gravity.gravityForce -= gravityModifier.gravityForce_delta;
            gravity.exponent -= gravityModifier.exponent_delta;

            gravityModifier.gravityForce_delta = 0f;
            gravityModifier.exponent_delta = 0f;

        }
        public void RemoveGravityModifier(Gravity gravity)
        {

            gravity.gravityForce -= this.gravityForce_delta;
            gravity.exponent -= this.exponent_delta;

            this.gravityForce_delta = 0f;
            this.exponent_delta = 0f;

        }

    }
}

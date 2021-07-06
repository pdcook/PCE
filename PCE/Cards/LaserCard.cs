using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnboundLib;
using UnityEngine;
using System.Linq;

namespace PCE.Cards
{
    public class LaserCard : CustomCard
    {
        /*
         * fire a laser instead of a gun
         */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // get the screenEdge (with screenEdgeBounce component) from the TargetBounce card as well as the tail effect
            CardInfo[] cards = global::CardChoice.instance.cards;
            CardInfo targetBounceCard = (new List<CardInfo>(cards)).Where(card => card.gameObject.name == "TargetBounce").ToList()[0];
            Gun targetBounceGun = targetBounceCard.GetComponent<Gun>();
            //ObjectsToSpawn screenEdgeToSpawn = (new List<ObjectsToSpawn>(targetBounceGun.objectsToSpawn)).Where(objectToSpawn => objectToSpawn.AddToProjectile.GetComponent<ScreenEdgeBounce>() != null).ToList()[0];

            ObjectsToSpawn trailToSpawn = (new List<ObjectsToSpawn>(targetBounceGun.objectsToSpawn)).Where(objectToSpawn => objectToSpawn.AddToProjectile.GetComponent<BounceTrigger>() != null).ToList()[0];

            // remove target effects from the trail
            if (trailToSpawn.AddToProjectile.GetComponent<BounceTrigger>() != null) { UnityEngine.GameObject.Destroy(trailToSpawn.AddToProjectile.GetComponent<BounceTrigger>()); }
            if (trailToSpawn.AddToProjectile.GetComponent<BounceEffectRetarget>() != null) { UnityEngine.GameObject.Destroy(trailToSpawn.AddToProjectile.GetComponent<BounceEffectRetarget>()); }


            List<ObjectsToSpawn> objectsToSpawn = gun.objectsToSpawn.ToList();
            //objectsToSpawn.Add(screenEdgeToSpawn);
            objectsToSpawn.Add(trailToSpawn);
            gun.objectsToSpawn = objectsToSpawn.ToArray();
            gun.gravity = 0f;
            gun.reflects += 100;


            gun.recoilMuiltiplier = 0f;
            gun.spread = 0f;
            gun.multiplySpread = 0f;
            gun.projectileSpeed += 10f;
            gun.drag = 0f;
            if (gun.projectileColor.a > 0f)
            {
                gun.projectileColor = new Color(1f, 0f, 0f, 0.1f);
            }
            gunAmmo.reloadTimeMultiplier *= 1.5f;
            gunAmmo.maxAmmo = 1;
            gun.destroyBulletAfter = 5f;
            gun.shakeM = 0f;
            gun.knockback *= 0.5f;
            /*
            gun.soundDisableRayHitBulletSound = true;
            gun.forceSpecificShake = false;
            gun.recoilMuiltiplier = 0f;
            gun.spread = 0f;
            gun.multiplySpread = 0f;
            gun.size = 0f;
            gun.bursts = 50;
            gun.knockback *= 0.005f;
            gun.projectileSpeed = Math.Min(Math.Max(gun.projectileSpeed + 10f, 0f), 20f);
            gun.projectielSimulatonSpeed = Math.Min(Math.Max(gun.projectielSimulatonSpeed + 10f, 0f), 20f);
            gun.drag = 0f;
            gun.gravity = 0f;
            gun.timeBetweenBullets = 0.001f;
            if (gun.projectileColor.a > 0f)
            {
                gun.projectileColor = new Color(1f, 0f, 0f, 0.1f);
            }
            gun.multiplySpread = 0f;
            gun.shakeM = 0f;
            gun.bulletDamageMultiplier = 0.01f;
            gunAmmo.reloadTimeMultiplier *= 1.5f;
            gunAmmo.maxAmmo = 1;
            gun.destroyBulletAfter = 1f;
            */
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Laser";
        }
        protected override string GetDescription()
        {
            return "Light Amplification by Stimulated Emission of Radiation";
        }
        protected override GameObject GetCardArt()
        {
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Laser");
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                positive = true,
                stat = "Bullets",
                amount = "Light",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Bullet Speed",
                amount = "+10000%",
                simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Damage",
                amount = "-99%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Reload Time",
                amount = "+50%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Ammo",
                amount = "1",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
    }
}

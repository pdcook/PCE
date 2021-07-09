using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnboundLib;
using UnityEngine;
using System.Linq;
using PCE.MonoBehaviours;
using System.Reflection;

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

            Material material = trailToSpawn.AddToProjectile.GetComponentInChildren<TrailRenderer>().material;
            // remove target effects from the trail
            //if (trailToSpawn.AddToProjectile.GetComponent<BounceTrigger>() != null) { UnityEngine.GameObject.Destroy(trailToSpawn.AddToProjectile.GetComponent<BounceTrigger>()); }
            //if (trailToSpawn.AddToProjectile.GetComponent<BounceEffectRetarget>() != null) { UnityEngine.GameObject.Destroy(trailToSpawn.AddToProjectile.GetComponent<BounceEffectRetarget>()); }


            List<ObjectsToSpawn> objectsToSpawn = gun.objectsToSpawn.ToList();

            ObjectsToSpawn laserTrail = new ObjectsToSpawn { };
            laserTrail.AddToProjectile = new GameObject("LaserTrail", typeof(LaserHurtbox));
            LaserHurtbox laser = laserTrail.AddToProjectile.gameObject.GetComponent<LaserHurtbox>();
            laser.color = Color.red;
            laser.material = new Material(material); // clone the material
            /*
            laserTrail.AddToProjectile = new GameObject("LaserTrail", typeof(PolygonCollider2D), typeof(Rigidbody2D), typeof(MeshFilter), typeof(MeshRenderer));
            laserTrail.AddToProjectile.GetComponent<Rigidbody2D>().isKinematic = true;
            laserTrail.AddToProjectile.AddComponent<UnparentOnHit>();
            TrailRenderer trail = laserTrail.AddToProjectile.AddComponent<TrailRenderer>();
            */

            /*
            laserTrail.AddToProjectile.AddComponent<LaserHurtbox>();
            PolygonCollider2D collider = laserTrail.AddToProjectile.GetComponent<PolygonCollider2D>();
            collider.isTrigger = true;
            collider.enabled = true;
            */
            objectsToSpawn.Add(laserTrail);
            
            gun.objectsToSpawn = objectsToSpawn.ToArray();
            gun.gravity = 0f;
            gun.reflects += 100;


            gun.recoilMuiltiplier = 0f;
            gun.spread = 0f;
            gun.multiplySpread = 0f;
            gun.projectileSpeed += 4f;
            gun.projectielSimulatonSpeed = 1f;
            gun.drag = 0f;
            if (gun.projectileColor.a > 0f)
            {
                gun.projectileColor = new Color(1f, 0f, 0f, 1f);
            }
            gunAmmo.reloadTimeMultiplier *= 1.5f;
            gunAmmo.maxAmmo = 1;
            gun.destroyBulletAfter = 10f;
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
    public static class MyVector3Extension
    {
        public static Vector2[] toVector2Array(this Vector3[] v3)
        {
            return System.Array.ConvertAll<Vector3, Vector2>(v3, getV3fromV2);
        }

        public static Vector2 getV3fromV2(Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }
    }
    class LaserHurtbox : MonoBehaviour
    {
        private float startTime;
        private float damageMultiplier;

        private float duration;
        private readonly float[] minmaxwidth = new float[]{0.1f,0.5f};
        private readonly float baseDamageMultiplier = 0.1f;

        private TrailRenderer trail;
        private readonly int MAX = 100000;
        private int numPos;
        private Vector3[] positions3d;

        private ProjectileHit projectile;
        private Gun gun;
        private Player player;

        public Material material
        {
            get
            {
                return this.trail.material;
            }
            set
            {
                this.trail.material = value; 
            }
        }
        public Color color
        {
            get { return this.trail.startColor; }
            set { this.trail.startColor = value; this.trail.endColor = value; this.trail.material.color = value; }
        }
        private float width
        {
            get { return this.trail.startWidth; }
            set { this.trail.startWidth = value; this.trail.endWidth = value; }
        }
        private float intensity
        {
            get
            {
                if (Time.time - this.startTime < this.duration)
                {
                    return 1f;
                }
                else
                {
                    return (-1f / this.duration) * (Time.time - this.startTime - this.duration) + 1f;
                }
            }
        }

        void Awake()
        {
            this.positions3d = new Vector3[MAX];
            this.trail = this.gameObject.GetOrAddComponent<TrailRenderer>();
        }
        void Start()
        {
            this.ResetTimer();
            this.damageMultiplier = this.baseDamageMultiplier;
            this.trail.minVertexDistance = 0.1f;
            this.trail.enabled = true;
            if (this.gameObject.transform.parent == null) { return; }
            this.projectile = this.gameObject.transform.parent.GetComponent<ProjectileHit>();
            if (this.projectile == null) { return; }
            this.player = this.projectile.ownPlayer;
            this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            this.duration = this.gun.destroyBulletAfter / 2f;
            this.trail.time = this.duration;
            this.UpdateWidth();
        }
        void Update()
        {
            if (this.projectile == null) { return; }

            this.numPos = this.trail.GetPositions(positions3d);
            this.UpdateDamage();
            this.UpdateColor();
            this.HurtBox(positions3d.toVector2Array().ToList<Vector2>().Take(this.numPos).ToArray());
        }
        void UpdateDamage()
        {
            this.damageMultiplier = this.baseDamageMultiplier * this.intensity;
        }
        void UpdateColor()
        {
            this.color = new Color(this.color.r, this.color.g, this.color.b, this.color.a * UnityEngine.Mathf.Clamp(this.intensity, 0.1f, 1f));
        }
        void UpdateWidth()
        {
            this.width = UnityEngine.Mathf.Clamp((this.minmaxwidth[1]-this.minmaxwidth[0])*((this.gun.damage-0.1f)*this.damageMultiplier)/((3f-0.1f)*this.baseDamageMultiplier) + this.minmaxwidth[0], this.minmaxwidth[0], this.minmaxwidth[1]);
        }
        void HurtBox(Vector2[] vertices)
        {
            // take every consecutive pair of vertices, generate a capsule to use Physics.OverlapCapsule to see if it intersects with a player
            for (int i = 0; i < vertices.Length-1; i++)
            {
                Vector2 mid = Vector2.Lerp(vertices[i], vertices[i + 1], 0.5f);
                Vector2 size = new Vector2(this.trail.startWidth, Vector2.Distance(vertices[i], vertices[i + 1]));
                float angle = Vector2.SignedAngle(Vector2.up, vertices[i] - vertices[i + 1]);

                Collider2D[] colliders = Physics2D.OverlapCapsuleAll(mid, size, CapsuleDirection2D.Vertical, angle);   
                foreach (Collider2D collider in colliders)
                {
                    Damagable componentInParent = collider.GetComponentInParent<Damagable>();
                    if (componentInParent)
                    {
                        if ((bool)typeof(Gun).InvokeMember("CheckIsMine",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, this.gun, new object[] { }))
                        {

                            componentInParent.CallTakeDamage(base.transform.forward * this.gun.damage * this.damageMultiplier, collider.transform.position, this.gun.gameObject, this.player, true);

                        }
                    }
                }
            }
        }
        private void ResetTimer()
        {
            this.startTime = Time.time;
        }
        
    }
}

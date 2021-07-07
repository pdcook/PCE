using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnboundLib;
using UnityEngine;
using System.Linq;
using PCE.MonoBehaviours;

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
            /*
            laserTrail.AddToProjectile = new GameObject("LaserTrail", typeof(PolygonCollider2D), typeof(Rigidbody2D), typeof(MeshFilter), typeof(MeshRenderer));
            laserTrail.AddToProjectile.GetComponent<Rigidbody2D>().isKinematic = true;
            laserTrail.AddToProjectile.AddComponent<UnparentOnHit>();
            TrailRenderer trail = laserTrail.AddToProjectile.AddComponent<TrailRenderer>();
            trail.material = material;
            trail.startColor = new Color(1f, 0f, 0f, 1f);
            trail.endColor = new Color(1f, 0f, 0f, 1f);
            trail.startWidth = 0.2f;
            trail.endWidth = 0.2f;
            trail.minVertexDistance = 1f;
            trail.time = 10f;
            trail.enabled = true;
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
    public class LaserHurtbox : MonoBehaviour
    {
        private TrailRenderer trail;
        private MeshFilter meshFilter;
        private PolygonCollider2D collider;
        private Rigidbody2D rigidbody;
        void Awake()
        {
            trail = this.gameObject.GetOrAddComponent<TrailRenderer>();
            meshFilter = this.gameObject.GetOrAddComponent<MeshFilter>();
            collider = this.gameObject.GetOrAddComponent<PolygonCollider2D>();
            rigidbody = this.gameObject.GetOrAddComponent<Rigidbody2D>();
            rigidbody.isKinematic = true;

            trail.startColor = new Color(1f, 0f, 0f, 1f);
            trail.endColor = new Color(1f, 0f, 0f, 1f);
            trail.startWidth = 0.2f;
            trail.endWidth = 0.2f;
            trail.minVertexDistance = 1f;
            trail.time = 10f;
            trail.enabled = true;

            collider.isTrigger = true;
            collider.enabled = true;
        }
        void Start()
        {
        }
        void Update()
        {
            if (this.gameObject.transform.parent == null)
            {
                this.transform.position = new Vector2(1000f, 1000f);
                return;
            }

            this.UpdateCollider(this.gameObject.GetComponent<TrailRenderer>());
        }
        public void UpdateCollider(TrailRenderer trail)
        {

            if (trail != null)
            {
                //UnityEngine.Debug.Log("updating collider.");

                Mesh mesh = new Mesh();
                trail.BakeMesh(mesh, Camera.main, true);
                this.gameObject.GetComponent<MeshFilter>().mesh = mesh;

                PolygonCollider2D collider = this.gameObject.GetComponent<PolygonCollider2D>();

                List<Vector2> verts = new List<Vector2>();
                foreach (Vector2 vertex in mesh.vertices)
                {
                    verts.Add(vertex);
                }

                collider.points = verts.ToArray();

            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {

            UnityEngine.Debug.Log("Laser Triggered by: " + other.name);

            //HealthHandler health = other.GetComponent<HealthHandler>();

            //health.DoDamage(100f * Vector2.up, Vector2.zero, Color.red);

        }
        private void OnTriggerStay2D(Collider2D other)
        {

            UnityEngine.Debug.Log("Laser Stayed by: " + other.name);

            HealthHandler health = other.GetComponent<HealthHandler>();

            if (health != null)
            {
                health.DoDamage(0.1f * Vector2.up, Vector2.zero, Color.red);
            }
        }
    }
    /*
    public class LaserHurtbox : MonoBehaviour
    {
        void Start()
        {
            if (this.transform.parent == null)
            {
                Destroy(this);
            }
        }
        void Update()
        {
            this.UpdateCollider(this.gameObject.GetComponent<TrailRenderer>());
        }
        public void UpdateCollider(TrailRenderer trail)
        {
            if (trail != null)
            {
                UnityEngine.Debug.Log("updating collider.");

                Mesh mesh = new Mesh();
                trail.BakeMesh(mesh, Camera.main, true);
                this.gameObject.GetComponent<MeshFilter>().mesh = mesh;

                PolygonCollider2D collider = this.gameObject.GetComponent<PolygonCollider2D>();
                
                List<Vector2> verts = new List<Vector2>();
                foreach (Vector2 vertex in mesh.vertices)
                {
                    verts.Add(vertex);
                }

                collider.points = verts.ToArray();
                
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {

            UnityEngine.Debug.Log("Laser Triggered");

            //HealthHandler health = other.GetComponent<HealthHandler>();
            
            //health.DoDamage(100f * Vector2.up, Vector2.zero, Color.red);
            
        }
        private void OnTriggerStay2D(Collider2D other)
        {

            UnityEngine.Debug.Log("Laser Stayed");

            HealthHandler health = other.GetComponent<HealthHandler>();

            if (health != null)
            {
                health.DoDamage(0.1f * Vector2.up, Vector2.zero, Color.red);
            }
        }
    }
    */
}

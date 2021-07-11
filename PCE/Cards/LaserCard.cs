using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnboundLib;
using UnityEngine;
using System.Linq;
using PCE.MonoBehaviours;
using System.Reflection;
using Photon.Pun;
using UnboundLib.Networking;

namespace PCE.Cards
{
    public static class Assets
    {
        //public static GameObject laserspawner = new GameObject("LaserSpawner", typeof(LaserSpawner));
        //public static GameObject lasergun = new GameObject("LaserGun", typeof(LaserGun), typeof(PhotonView));

    }
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
            List<ObjectsToSpawn> objectsToSpawn = gun.objectsToSpawn.ToList();
            ObjectsToSpawn laserSpawner = new ObjectsToSpawn { };
            laserSpawner.AddToProjectile = new GameObject("LaserSpawner", typeof(LaserSpawner));
            //laserSpawner.AddToProjectile.GetComponent<LaserSpawner>().view = base.GetComponent<PhotonView>();
            objectsToSpawn.Add(laserSpawner);
            //ObjectsToSpawn laserGun = new ObjectsToSpawn { };
            //laserGun.AddToProjectile = new GameObject("LaserGun", typeof(LaserGun));
            //objectsToSpawn.Add(laserGun);
            
            gun.objectsToSpawn = objectsToSpawn.ToArray();
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
            return "<b>L</b>ight <b>A</b>mplification by <b>S</b>timulated <b>E</b>mission of <b>R</b>adiation";
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
    public class LaserSpawner : MonoBehaviour
    {

        GameObject lasergun = new GameObject("LaserGun", typeof(LaserGun), typeof(PhotonView));

        //private static bool Initialized = false;

        void Awake()
        {
            PhotonNetwork.PrefabPool.RegisterPrefab(lasergun.name, lasergun);
        }

        void Start()
        {
            UnityEngine.Debug.Log("START SPAWN");


            /*
            if (!Initialized)
            {
                Initialized = true;
                return;
            }*/

            //Destroy(gameObject, 1f);

            //if (!PhotonNetwork.OfflineMode && !PhotonNetwork.IsMasterClient) return;

            //this.ExecuteAfterSeconds(0.1f, () =>
            //{
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            {
                GameObject laser = PhotonNetwork.Instantiate(
                    lasergun.name,
                    transform.position,
                    transform.rotation,
                    0,
                    new object[] { this.gameObject.transform.parent.GetComponent<PhotonView>().ViewID }
                );
                UnityEngine.Debug.Log(this.gameObject.transform.parent.GetComponent<PhotonView>().ViewID);


                //int laserID = laser.GetComponent<PhotonView>().ViewID;
                //int parentID = this.gameObject.transform.parent.GetComponent<PhotonView>().ViewID;
                //UnboundLib.NetworkingManager.RPC(typeof(LaserSpawner), "ParentFromViewID", new object[] { laserID, parentID });
            }
            //});
        }
        [UnboundRPC]
        public static void ParentFromViewID(int childID, int parentID)
        {
            GameObject child = PhotonView.Find(childID).gameObject;
            GameObject parent = PhotonView.Find(parentID).gameObject;
            child.transform.SetParent(parent.transform);
        }
    }
    public class LaserGun : MonoBehaviour, IPunInstantiateMagicCallback
    {

        private readonly float duration = 20f;

        private ProjectileHit projectile;
        private Gun gun;
        private Gun newGun;
        private Player player;
        
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            UnityEngine.Debug.Log("OnPhotonInstantiate");
            object[] instantiationData = info.photonView.InstantiationData;
            int parentViewID = (int)instantiationData[0];
            UnityEngine.Debug.Log(parentViewID);
            GameObject parent = PhotonView.Find(parentViewID).gameObject;
            this.gameObject.transform.SetParent(parent.transform);
            this.projectile = this.gameObject.transform.parent.GetComponent<ProjectileHit>();
            this.player = this.projectile.ownPlayer;
            this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            UnityEngine.Debug.Log("Parenting Success");
        }
        void Start()
        {
            UnityEngine.Debug.Log("START GUN");

            // get the projectile, player, and gun this is attached to
            //if (this.gameObject.transform.parent == null) { return; }
            //this.projectile = this.gameObject.transform.parent.GetComponent<ProjectileHit>();
            //if (this.projectile == null) { return; }
            //this.player = this.projectile.ownPlayer;
            //if (this.player == null) { return; }
            //this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();

            // create a new gun for the spawnbulletseffect
            this.newGun = this.player.gameObject.AddComponent<Gun>();

            SpawnBulletsEffect effect = this.player.gameObject.AddComponent<SpawnBulletsEffect>();
            // set the position and direction to fire
            effect.SetDirection(((Quaternion)typeof(Gun).InvokeMember("getShootRotation",
                                   BindingFlags.Instance | BindingFlags.InvokeMethod |
                                  BindingFlags.NonPublic, null, this.gun, new object[] {0, 0, 0f})) * Vector3.forward);
            effect.SetPosition(this.gun.transform.position);
            effect.SetNumBullets(1);
            effect.SetTimeBetweenShots(0f);
            effect.SetInitialDelay(0f);

            // copy private gun stats over and reset all public stats
            SpawnBulletsEffect.CopyGunStats(this.gun, newGun);

            newGun.isReloading = false;
            newGun.damage = 1f;
            newGun.reloadTime = 1f;
            newGun.reloadTimeAdd = 0f;
            newGun.recoilMuiltiplier = 1f;
            newGun.knockback = 1f;
            newGun.attackSpeed = 0.3f;
            newGun.projectileSpeed = 1f;
            newGun.projectielSimulatonSpeed = 1f;
            newGun.gravity = 1f;
            newGun.damageAfterDistanceMultiplier = 1f;
            newGun.bulletDamageMultiplier = 1f;
            newGun.multiplySpread = 1f;
            newGun.shakeM = 1f;
            newGun.ammo = 0;
            newGun.ammoReg = 0f;
            newGun.size = 0f;
            newGun.overheatMultiplier = 0f;
            newGun.timeToReachFullMovementMultiplier = 0f;
            newGun.numberOfProjectiles = 1;
            newGun.bursts = 0;
            newGun.reflects = 0;
            newGun.smartBounce = 0;
            newGun.bulletPortal = 0;
            newGun.randomBounces = 0;
            newGun.timeBetweenBullets = 0f;
            newGun.projectileSize = 0f;
            newGun.speedMOnBounce = 1f;
            newGun.dmgMOnBounce = 1f;
            newGun.drag = 0f;
            newGun.dragMinSpeed = 1f;
            newGun.spread = 0f;
            newGun.evenSpread = 0f;
            newGun.percentageDamage = 0f;
            newGun.cos = 0f;
            newGun.slow = 0f;
            newGun.chargeNumberOfProjectilesTo = 0f;
            newGun.destroyBulletAfter = 0f;
            newGun.forceSpecificAttackSpeed = 0f;
            newGun.lockGunToDefault = false;
            newGun.unblockable = false;
            newGun.ignoreWalls = false;
            newGun.currentCharge = 0f;
            newGun.useCharge = false;
            newGun.waveMovement = false;
            newGun.teleport = false;
            newGun.spawnSkelletonSquare = false;
            newGun.explodeNearEnemyRange = 0f;
            newGun.explodeNearEnemyDamage = 0f;
            newGun.hitMovementMultiplier = 1f;
            newGun.isProjectileGun = false;
            newGun.defaultCooldown = 1f;
            newGun.attackSpeedMultiplier = 1f;

            newGun.damage = 0f;
            newGun.damageAfterDistanceMultiplier = 1f;
            newGun.reflects = int.MaxValue;
            newGun.bulletDamageMultiplier = 1f;
            newGun.projectileSpeed = 10f;
            newGun.projectielSimulatonSpeed = 1f;
            newGun.projectileSize = 1f;
            newGun.projectileColor = Color.red;
            newGun.spread = 0f;
            newGun.destroyBulletAfter = this.duration;
            newGun.numberOfProjectiles = 1;
            newGun.ignoreWalls = false;
            newGun.gravity = 0f;
            /*
            // get the bullet trail material from the targetbounce card
            CardInfo[] cards = global::CardChoice.instance.cards;
            CardInfo targetBounceCard = (new List<CardInfo>(cards)).Where(card => card.gameObject.name == "TargetBounce").ToList()[0];
            Gun targetBounceGun = targetBounceCard.GetComponent<Gun>();
            ObjectsToSpawn trailToSpawn = (new List<ObjectsToSpawn>(targetBounceGun.objectsToSpawn)).Where(objectToSpawn => objectToSpawn.AddToProjectile.GetComponent<BounceTrigger>() != null).ToList()[0];
            Material material = trailToSpawn.AddToProjectile.GetComponentInChildren<TrailRenderer>().material;
            */
            // make the lasertrail objectToSpawn
            ObjectsToSpawn laserTrail = new ObjectsToSpawn { };
            laserTrail.AddToProjectile = new GameObject("LaserHurtboxSpawner", typeof(LaserHurtboxSpawner));// new GameObject("LaserTrail", typeof(LaserHurtbox), typeof(DestroyOnUnparent));
            LaserHurtboxSpawner laserHurtboxSpawner = laserTrail.AddToProjectile.GetComponent<LaserHurtboxSpawner>();
            laserHurtboxSpawner.player = this.player;
            laserHurtboxSpawner.duration = this.duration / 2f;
            laserHurtboxSpawner.baseDamage = UnityEngine.Mathf.Clamp(this.gun.damage, 1f, float.MaxValue);

            /*
            LaserHurtbox laser = laserTrail.AddToProjectile.gameObject.GetComponent<LaserHurtbox>();
            laser.player = this.player;
            laser.gun = newGun;
            laser.duration = this.duration/2f;
            laser.baseDamage = UnityEngine.Mathf.Clamp(this.gun.damage, 1f, float.MaxValue);
            laser.color = Color.red;
            laser.material = new Material(material); // clone the material
            */
            newGun.objectsToSpawn = new ObjectsToSpawn[] { laserTrail };

            // set the gun of the spawnbulletseffect
            effect.SetGun(newGun);


        }
    }
    // destroy object once its no longer a child
    public class DestroyOnUnparent : MonoBehaviour
    {
        void Update()
        {
            if (this.gameObject.transform.parent == null) { Destroy(this.gameObject); }
        }
    }
    public class LaserHurtboxSpawner : MonoBehaviour
    {

        GameObject laserhurtbox = new GameObject("LaserTrail", typeof(LaserHurtbox), typeof(DestroyOnUnparent), typeof(PhotonView));

        //private static bool Initialized = false;

        public Player player;
        public float duration;
        public float baseDamage;

        void Awake()
        {
            PhotonNetwork.PrefabPool.RegisterPrefab(laserhurtbox.name, laserhurtbox);
        }

        void Start()
        {
            UnityEngine.Debug.Log("START SPAWN HURTBOX");


            /*
            if (!Initialized)
            {
                Initialized = true;
                return;
            }*/

            //Destroy(gameObject, 1f);

            //if (!PhotonNetwork.OfflineMode && !PhotonNetwork.IsMasterClient) return;

            //this.ExecuteAfterSeconds(0.1f, () =>
            //{
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            {
                GameObject laser = PhotonNetwork.Instantiate(
                    laserhurtbox.name,
                    transform.position,
                    transform.rotation,
                    0,
                    new object[] { this.player.GetComponent<PhotonView>().ViewID, this.duration, this.baseDamage }
                );
                UnityEngine.Debug.Log(this.gameObject.transform.parent.GetComponent<PhotonView>().ViewID);


                //int laserID = laser.GetComponent<PhotonView>().ViewID;
                //int parentID = this.gameObject.transform.parent.GetComponent<PhotonView>().ViewID;
                //UnboundLib.NetworkingManager.RPC(typeof(LaserSpawner), "ParentFromViewID", new object[] { laserID, parentID });
            }
            //});
        }
    }
    public static class Vector3Extension
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
        public float baseDamage;

        private readonly int frameInterval = 15;
        private int frames = 0;

        public float duration;
        private readonly float[] minmaxwidth = new float[]{0.1f,0.5f};
        private readonly float baseDamageMultiplier = 2f;

        private TrailRenderer trail;
        private readonly int MAX = 100000;
        private int numPos;
        private Vector3[] positions3d;

        //public Gun gun;
        public Player player;
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            UnityEngine.Debug.Log("HURTBOX OnPhotonInstantiate");
            object[] instantiationData = info.photonView.InstantiationData;
            int playerViewID = (int)instantiationData[0];
            float duration = (float)instantiationData[1];
            float baseDamage = (float)instantiationData[2];

            this.player = PhotonView.Find(playerViewID).gameObject.GetComponent<Player>();
            this.duration = duration;
            this.baseDamage = baseDamage;

            UnityEngine.Debug.Log("HURTBOX Instantiated");

        }
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
                    return UnityEngine.Mathf.Clamp((-1f / this.duration) * (Time.time - this.startTime - this.duration) + 1f, 0f, 1f);
                }
            }
        }
        void Destroy()
        {
            UnityEngine.GameObject.Destroy(this.gameObject.transform.parent.gameObject);
        }

        void Awake()
        {
            // get the bullet trail material from the targetbounce card
            CardInfo[] cards = global::CardChoice.instance.cards;
            CardInfo targetBounceCard = (new List<CardInfo>(cards)).Where(card => card.gameObject.name == "TargetBounce").ToList()[0];
            Gun targetBounceGun = targetBounceCard.GetComponent<Gun>();
            ObjectsToSpawn trailToSpawn = (new List<ObjectsToSpawn>(targetBounceGun.objectsToSpawn)).Where(objectToSpawn => objectToSpawn.AddToProjectile.GetComponent<BounceTrigger>() != null).ToList()[0];
            Material material = trailToSpawn.AddToProjectile.GetComponentInChildren<TrailRenderer>().material;

            this.color = Color.red;

            this.positions3d = new Vector3[MAX];
            this.trail = this.gameObject.GetOrAddComponent<TrailRenderer>();
        }
        void Start()
        {
            this.ResetTimer();
            this.damageMultiplier = this.baseDamageMultiplier;
            this.trail.minVertexDistance = 0.1f;
            this.trail.enabled = true;
            if (this.color.a < 0.1f) { this.color = new Color(this.color.r, this.color.g, this.color.b, 0.1f); }
            this.trail.time = this.duration;
            this.UpdateWidth();
        }
        void DestroyBulletAfter()
        {
            if (Time.time - this.startTime > 2f*this.duration)
            {
                UnityEngine.GameObject.Destroy(this.gameObject.transform.parent.gameObject);
            }
        }
        void Update()
        {
            this.DestroyBulletAfter();
            if (this.frames < this.frameInterval)
            {
                this.frames++;
                return;
            }
            else
            {
                this.frames = 0;
                this.numPos = this.trail.GetPositions(positions3d);
                this.UpdateDamage();
                this.UpdateColor();
                this.HurtBox(positions3d.toVector2Array().ToList<Vector2>().Take(this.numPos).ToArray());
            }

        }
        void UpdateDamage()
        {
            this.damageMultiplier = this.baseDamageMultiplier * this.intensity;
        }
        void UpdateColor()
        {
            this.color = new Color(this.color.r, this.color.g, this.color.b, UnityEngine.Mathf.Clamp(this.color.a * this.intensity, 0.25f, 1f));
        }
        void UpdateWidth()
        {
            this.width = UnityEngine.Mathf.Clamp((this.minmaxwidth[1]-this.minmaxwidth[0])*((this.baseDamage-0.1f)*this.damageMultiplier)/((3f-0.1f)*this.baseDamageMultiplier) + this.minmaxwidth[0], this.minmaxwidth[0], this.minmaxwidth[1]);
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
                        //if ((bool)typeof(Gun).InvokeMember("CheckIsMine",
                         //           BindingFlags.Instance | BindingFlags.InvokeMethod |
                         //           BindingFlags.NonPublic, null, this.gun, new object[] { }))
                        if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
                        {

                            UnityEngine.Debug.Log("baseDamage: " + this.baseDamage.ToString());
                            UnityEngine.Debug.Log("damageMult: " + this.damageMultiplier.ToString());
                            UnityEngine.Debug.Log("Intensity: " + this.intensity.ToString());


                            //componentInParent.CallTakeDamage(base.transform.forward * this.baseDamage * this.damageMultiplier, collider.transform.position, this.gun.gameObject, this.player, true);
                            componentInParent.CallTakeDamage(base.transform.forward * this.baseDamage * this.damageMultiplier, collider.transform.position, null, this.player, true);

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

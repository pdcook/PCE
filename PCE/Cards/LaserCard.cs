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
using Photon;

namespace PCE.Cards
{
    public static class Assets
    {
        public readonly static GameObject laserGun = new GameObject("LaserGun", typeof(LaserGun), typeof(PhotonView));
        public readonly static GameObject laserTrail = new GameObject("LaserTrail", typeof(LaserHurtbox), typeof(PhotonView), typeof(LineRenderer), typeof(NetworkedTrailRenderer));
        public readonly static GameObject laserTrailSpawner = new GameObject("LaserTrailSpawner", typeof(LaserTrailSpawner));

    }
    public class LaserCard : CustomCard
    {

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            List<ObjectsToSpawn> objectsToSpawn = gun.objectsToSpawn.ToList();
            ObjectsToSpawn laserSpawner = new ObjectsToSpawn { };
            ObjectsToSpawn laserGun = new ObjectsToSpawn { };
            laserGun.AddToProjectile = new GameObject("LaserGunSpawner", typeof(LaserGunSpawner));//new GameObject("LaserGun", typeof(LaserGun));
            objectsToSpawn.Add(laserGun);
            
            gun.objectsToSpawn = objectsToSpawn.ToArray();

            gun.reloadTime += 0.5f;
            gun.attackSpeed *= 2f;
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
                positive = false,
                stat = "Reload Time",
                amount = "+50%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Attack Speed",
                    amount = "-50%",
                    simepleAmount = CardInfoStat.SimpleAmount.lower
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
    public class LaserGunSpawner : MonoBehaviour
    {
        private static bool Initialized = false;



        void Awake()
        {
            if (!Initialized)
            {
                PhotonNetwork.PrefabPool.RegisterPrefab(Assets.laserGun.name, Assets.laserGun);
                PhotonNetwork.PrefabPool.RegisterPrefab(Assets.laserTrail.name, Assets.laserTrail);
            }
        }

        void Start()
        {
            if (!Initialized)
            {
                Initialized = true;
                return;
            }

            //Destroy(gameObject, 1f);

            if (!PhotonNetwork.OfflineMode && !this.gameObject.transform.parent.GetComponent<ProjectileHit>().ownPlayer.data.view.IsMine) return;

            //this.ExecuteAfterSeconds(0.5f, () =>
            //{
                PhotonNetwork.Instantiate(
                    Assets.laserGun.name,
                    transform.position,
                    transform.rotation,
                    0,
                    new object[] { this.gameObject.transform.parent.GetComponent<PhotonView>().ViewID }
                );
            //});
        }
    }
    public class LaserGun : MonoBehaviour, IPunInstantiateMagicCallback
    {

        private readonly float duration = 20f;

        private ProjectileHit projectile;
        private Gun gun;
        private Gun newGun;
        private Player player;
        public void OnPhotonInstantiate(Photon.Pun.PhotonMessageInfo info)
        {
            //UnityEngine.Debug.Log("LASERGUN: ONPHOTONINSTANTIATE");

            object[] instantiationData = info.photonView.InstantiationData;

            GameObject parent = PhotonView.Find((int)instantiationData[0]).gameObject;

            this.gameObject.transform.SetParent(parent.transform);

            this.player = parent.GetComponent<ProjectileHit>().ownPlayer;
            this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();

            this.gameObject.AddComponent<DestroyOnUnparent>();

        }
        void Start()
        {

            // get the projectile, player, and gun this is attached to
            //if (this.gameObject.transform.parent == null) { return; }
            this.projectile = this.gameObject.transform.parent.GetComponent<ProjectileHit>();
            //if (this.projectile == null) { return; }
            this.player = this.projectile.ownPlayer;
            //if (this.player == null) { return; }
            this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();

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
            effect.SetInitialDelay(0.05f);

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
            newGun.projectileColor = Color.clear;
            newGun.spread = 0f;
            newGun.destroyBulletAfter = this.duration;
            newGun.numberOfProjectiles = 1;
            newGun.ignoreWalls = false;
            newGun.gravity = 0f;

            // get the bullet trail material from the targetbounce card
            CardInfo[] cards = global::CardChoice.instance.cards;
            CardInfo targetBounceCard = (new List<CardInfo>(cards)).Where(card => card.gameObject.name == "TargetBounce").ToList()[0];
            Gun targetBounceGun = targetBounceCard.GetComponent<Gun>();
            ObjectsToSpawn trailToSpawn = (new List<ObjectsToSpawn>(targetBounceGun.objectsToSpawn)).Where(objectToSpawn => objectToSpawn.AddToProjectile.GetComponent<BounceTrigger>() != null).ToList()[0];
            Material material = trailToSpawn.AddToProjectile.GetComponentInChildren<TrailRenderer>().material;
            
            // make the lasertrail objectToSpawn
            ObjectsToSpawn laserTrail = new ObjectsToSpawn { };
            laserTrail.AddToProjectile = Assets.laserTrailSpawner;//new GameObject("LaserTrail", typeof(LaserHurtbox), typeof(DestroyOnUnparent));
            //LaserHurtbox laser = laserTrail.AddToProjectile.gameObject.GetComponent<LaserHurtbox>();
            /*
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
        void LateUpdate()
        {
            if (this.gameObject.transform.parent == null) { Destroy(this.gameObject); }
        }
    }

    public class LaserTrailSpawner : MonoBehaviour
    {
        private static bool Initialized = false;


        void Awake()
        {
        }

        void Start()
        {
            if (!Initialized)
            {
                Initialized = true;
                return;
            }
            Destroy(gameObject, 1f);

            if (!PhotonNetwork.OfflineMode && !this.transform.parent.GetComponent<PhotonView>().IsMine) return;

            if (this.gameObject.transform.parent == null)
            {
                //UnityEngine.Debug.Log("NO PARENT");
                //if (this.gameObject.GetComponent<ProjectileHit>() == null)
                //{
                //    UnityEngine.Debug.Log("NO PROJECTILE");
                //}
            }    

            //this.ExecuteAfterSeconds(0.1f, () =>
            //{
                PhotonNetwork.Instantiate(
                    Assets.laserTrail.name,
                    transform.position,
                    transform.rotation,
                    0,
                    new object[] {this.gameObject.transform.parent.GetComponent<PhotonView>().ViewID}
                );
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
    public static class Vector2Extension
    {
        public static Vector3[] toVector3Array(this Vector2[] v2)
        {
            return System.Array.ConvertAll<Vector2, Vector3>(v2, getV2fromV3);
        }

        public static Vector3 getV2fromV3(Vector2 v2)
        {
            return new Vector3(v2.x, v2.y, 0);
        }
    }
    class LaserHurtbox : MonoBehaviour, IPunInstantiateMagicCallback
    {
        private float startTime;
        private float damageMultiplier;
        public float baseDamage;

        private readonly int frameInterval = 15;
        private int frames = 0;

        public float duration;
        private readonly float[] minmaxwidth = new float[]{0.1f,0.5f};
        private readonly float baseDamageMultiplier = 1f;

        private LineRenderer trail;
        private readonly int MAX = 100000;
        private int numPos;
        public Gun gun;
        public Player player;

        private bool sync = true;

        private Vector3[] positions3d;

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
        public void OnPhotonInstantiate(Photon.Pun.PhotonMessageInfo info)
        {
            //UnityEngine.Debug.Log("ONPHOTONINSTANTIATE");

            object[] instantiationData = info.photonView.InstantiationData;

            GameObject parent = PhotonView.Find((int)instantiationData[0]).gameObject;

            if (PhotonNetwork.OfflineMode || this.GetComponent<PhotonView>().IsMine)//PhotonNetwork.IsMasterClient)
            {
                this.gameObject.transform.SetParent(parent.transform);

                this.player = parent.GetComponent<ProjectileHit>().ownPlayer;
                this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();
                this.baseDamage = UnityEngine.Mathf.Clamp(this.gun.damage, 1f, float.MaxValue);
            }

            //this.gameObject.AddComponent<DestroyOnUnparent>();

        }
        /*
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                this.numPos = this.trail.GetPositions(positions3d);
                stream.SendNext(this.positions3d.ToList<Vector3>().Take(this.numPos).ToArray());
                stream.SendNext(this.numPos);
            }
            else
            {
                this.positions3d = (Vector3[])stream.ReceiveNext();
                this.numPos = (int)stream.ReceiveNext();
                this.trail.SetPositions(this.positions3d.ToList<Vector3>().Take(this.numPos).ToArray());
            }
        }*/

        void Destroy()
        {
            //UnityEngine.GameObject.Destroy(this.gameObject.transform.parent.gameObject);
            if (PhotonNetwork.OfflineMode || this.gameObject.GetComponent<PhotonView>().IsMine) { PhotonNetwork.Destroy(this.gameObject.GetComponent<PhotonView>()); }
            Destroy(this.gameObject);
        }

        void Awake()
        {
            this.duration = 10f;

            this.positions3d = new Vector3[MAX];
            this.trail = this.gameObject.GetComponent<LineRenderer>();
        }
        void Start()
        {

            this.sync = true;
            
            /*
            if (!PhotonNetwork.OfflineMode && !PhotonNetwork.IsMasterClient)
            {
                this.gameObject.transform.SetParent(null);
            }*/

            CardInfo[] cards = global::CardChoice.instance.cards;
            CardInfo targetBounceCard = (new List<CardInfo>(cards)).Where(card => card.gameObject.name == "TargetBounce").ToList()[0];
            Gun targetBounceGun = targetBounceCard.GetComponent<Gun>();
            ObjectsToSpawn trailToSpawn = (new List<ObjectsToSpawn>(targetBounceGun.objectsToSpawn)).Where(objectToSpawn => objectToSpawn.AddToProjectile.GetComponent<BounceTrigger>() != null).ToList()[0];
            Material material = trailToSpawn.AddToProjectile.GetComponentInChildren<TrailRenderer>().material;
            this.material = material;
            this.color = Color.red;


            this.ResetTimer();
            this.damageMultiplier = this.baseDamageMultiplier;
            //this.trail.minVertexDistance = 0.1f;
            this.trail.enabled = true;
            if (this.color.a < 0f) { this.color = new Color(this.color.r, this.color.g, this.color.b, 0f); }
            //this.trail.time = this.duration;
            this.UpdateWidth();
        }
        void DestroyBulletAfter()
        {
            if (Time.time - this.startTime > 2f*this.duration)
            {
                if (this.gameObject.transform.parent != null) { UnityEngine.GameObject.Destroy(this.gameObject.transform.parent.gameObject); }
                this.Destroy();
            }
        }
        void Update()
        {
            if (this.trail == null) { return; }

            if (this.sync && !this.BulletClose())
            {
                // if the bullet is off screen, unparent this object
                //this.gameObject.transform.SetParent(null);
                //this.trail.emitting = false;
                // also stop syncing the laser
                // TODO
                /*
                this.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off;
                this.GetComponent<PhotonView>().ObservedComponents.Remove(this);
                */
                //this.SyncTrail();
                this.sync = false;
            }

            this.DestroyBulletAfter();
            if (this.frames < this.frameInterval)
            {
                this.frames++;
                return;
            }
            else if (this.gameObject.GetComponent<PhotonView>().IsMine)
            {
                this.frames = 0;
                this.numPos = this.trail.GetPositions(positions3d);
                //this.SyncTrail();
                this.UpdateDamage();
                this.UpdateColor();
                this.UpdateWidth();
                this.HurtBox(positions3d.toVector2Array().ToList<Vector2>().Take(this.numPos).ToArray());
            }

        }
        bool BulletClose()
        {
            if (this.gameObject.transform.parent == null) { return false; }
            Vector3 bulletpos = this.gameObject.transform.parent.transform.position;
            return (UnityEngine.Mathf.Abs(bulletpos.x) < 40f && UnityEngine.Mathf.Abs(bulletpos.y) < 30f);
        }
        void SyncTrail()
        {
            //if (this.sync && !PhotonNetwork.OfflineMode && PhotonNetwork.IsMasterClient)
            //{
             //   this.gameObject.GetComponent<PhotonView>().RPC("RPCA_SyncLaser", RpcTarget.Others, new object[] { this.positions3d.toVector2Array().ToList().Take(this.numPos).ToArray(), this.numPos});
            //}
        }
        [PunRPC]
        void RPCA_SyncLaser(Vector2[] positions, int num)
        {
            //UnityEngine.Debug.Log("SYNC");
            //this.trail.Clear();
            //this.trail.AddPositions(positions.toVector3Array());
        }
        void UpdateDamage()
        {
            this.damageMultiplier = this.baseDamageMultiplier * this.intensity;
        }
        void UpdateColor()
        {
            this.color = new Color(this.color.r, this.color.g, this.color.b, UnityEngine.Mathf.Clamp(this.color.a * this.intensity, 0f, 1f));
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
                        if ((bool)typeof(Gun).InvokeMember("CheckIsMine",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, this.gun, new object[] { }))
                        {

                            //UnityEngine.Debug.Log("baseDamage: " + this.baseDamage.ToString());
                            //UnityEngine.Debug.Log("damageMult: " + this.damageMultiplier.ToString());
                            //UnityEngine.Debug.Log("Intensity: " + this.intensity.ToString());


                            componentInParent.CallTakeDamage(base.transform.forward * this.baseDamage * this.damageMultiplier, collider.transform.position, this.gun.gameObject, this.player, true);

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

    [RequireComponent(typeof(LineRenderer))]
    public class NetworkedTrailRenderer : MonoBehaviour
    {
        private readonly int refreshRate = 0;
        private readonly int initialDelay = 0;
        private readonly int syncRate = 5;
        private const int MAX = 1000;
        private const float tolerance = 0.5f;

        private LineRenderer line;
        private List<Vector3> rawPositions3d = new List<Vector3>() { };
        private Vector3[] positions3d;
        private int numPos;
        private PhotonView view;

        private int frameCount = 0;
        public Material material
        {
            get
            {
                return this.line.material;
            }
            set
            {
                this.line.material = value;
            }
        }
        public Color color
        {
            get { return this.line.startColor; }
            set { this.line.startColor = value; this.line.endColor = value; this.line.material.color = value; }
        }
        private float width
        {
            get { return this.line.startWidth; }
            set { this.line.startWidth = value; this.line.endWidth = value; }
        }
        void Awake()
        {
            this.positions3d = new Vector3[MAX];
        }
        void Start()
        {
            this.line = this.gameObject.GetComponent<LineRenderer>();
            this.view = this.gameObject.GetComponent<PhotonView>();

            if (this.gameObject.transform.parent != null)
            {
                this.rawPositions3d.Add(this.gameObject.transform.position);
                this.line.positionCount = this.rawPositions3d.Count;
                this.line.SetPositions(this.rawPositions3d.ToArray());
                this.numPos = this.line.GetPositions(this.positions3d);
                this.rawPositions3d = this.positions3d.ToList().Take(this.numPos).ToList();
            }
        }
        void Update()
        {
            if (this.line == null) { return; }
            if (this.view.ViewID == 0) { return; }
            if (this.gameObject.transform.parent == null) { return; }

            this.frameCount++;

            if ((this.refreshRate == 0 || this.frameCount % this.refreshRate == 0) && this.BulletClose())
            {
                this.rawPositions3d.Add(this.gameObject.transform.position);
                this.line.positionCount = this.rawPositions3d.Count;
                this.line.SetPositions(this.rawPositions3d.ToArray());
                if (this.line.positionCount > 3) { this.line.Simplify(NetworkedTrailRenderer.tolerance); }
                this.numPos = this.line.GetPositions(this.positions3d);
                this.rawPositions3d = this.positions3d.ToList().Take(this.numPos).ToList();
            }

            if ((this.frameCount > this.initialDelay) && (this.syncRate == 0 || this.frameCount % this.syncRate == 0) && this.BulletClose())
            {
                this.SyncTrail();
            }
            else if ((this.frameCount > this.initialDelay) && (this.syncRate == 0 || this.frameCount % this.syncRate == 0))
            {
                this.SyncAppearance();
            }

        }
        bool BulletClose()
        {
            if (this.gameObject.transform.parent == null) { return false; }
            Vector3 bulletpos = this.gameObject.transform.parent.transform.position;
            return (UnityEngine.Mathf.Abs(bulletpos.x) < 100f && UnityEngine.Mathf.Abs(bulletpos.y) < 100f);
        }
        void SyncTrail()
        {
            if (!PhotonNetwork.OfflineMode && this.gameObject.GetComponent<PhotonView>().IsMine)//PhotonNetwork.IsMasterClient)
            {
                this.view.RPC("RPCA_SyncTrail", RpcTarget.Others, new object[] { this.positions3d.toVector2Array().ToList().Take(this.numPos).ToArray(), this.numPos, this.line.startWidth, this.line.startColor.r, this.line.startColor.g, this.line.startColor.b, this.line.startColor.a });
            }
        }
        [PunRPC]
        void RPCA_SyncTrail(Vector2[] positions, int num, float width, float r, float g, float b, float a)
        {
            // unparent once the host starts syncing the position
            if (this.gameObject.transform.parent != null) { this.gameObject.transform.SetParent(null); }
            //UnityEngine.Debug.Log("SYNC");
            this.line.positionCount = num;
            this.line.SetPositions(positions.toVector3Array());
            this.numPos = this.line.GetPositions(this.positions3d);
            this.rawPositions3d = this.positions3d.ToList().Take(this.numPos).ToList();
            this.width = width;
            this.color = new Color(r, g, b, a);

        }
        void SyncAppearance()
        {
            if (!PhotonNetwork.OfflineMode && this.gameObject.GetComponent<PhotonView>().IsMine)//PhotonNetwork.IsMasterClient)
            {
                this.view.RPC("RPCA_SyncAppearance", RpcTarget.Others, new object[] { this.line.startWidth, this.line.startColor.r, this.line.startColor.g, this.line.startColor.b, this.line.startColor.a });
            }
        }
        [PunRPC]
        void RPCA_SyncAppearance(float width, float r, float g, float b, float a)
        {
            this.width = width;
            this.color = new Color(r, g, b, a);

        }

    }
}

using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PCE.Extensions;
using System.Linq;
using PCE.Utils;
using UnboundLib;

namespace PCE.MonoBehaviours
{
    public class FireworkAssets
    {
        private static GameObject _firework = null;

        internal static GameObject firework
        {
            get
            {
                if (FireworkAssets._firework != null) { return FireworkAssets._firework; }
                else
                {
                    FireworkAssets._firework = new GameObject("Firework", typeof(FireworkEffect), typeof(PhotonView));
                    UnityEngine.GameObject.DontDestroyOnLoad(FireworkAssets._firework);

                    return FireworkAssets._firework;
                }
            }
            set { }
        }
        private static GameObject _setInactiveDelay;
        internal static GameObject setInactiveDelay
        {
            get
            {
                if (FireworkAssets._setInactiveDelay != null) { return FireworkAssets._setInactiveDelay; }
                else
                {
                    FireworkAssets._setInactiveDelay = new GameObject("SetFireworkInactiveDelay",typeof(SetFireworkInactiveDelay));
                    UnityEngine.GameObject.DontDestroyOnLoad(FireworkAssets._setInactiveDelay);

                    return FireworkAssets._setInactiveDelay;
                }
            }
            set { }
        }
        internal static ObjectsToSpawn setInactiveDelayObjectToSpawn
        {
            get
            {
                ObjectsToSpawn obj = new ObjectsToSpawn() { };
                obj.AddToProjectile = FireworkAssets.setInactiveDelay;

                return obj;
            }
            set { }
        }
    }
    public class FireworkSpawner : MonoBehaviour
    {
        private static bool Initialized = false;



        void Awake()
        {
            if (!Initialized)
            {
                PhotonNetwork.PrefabPool.RegisterPrefab(FireworkAssets.firework.name, FireworkAssets.firework);
            }
        }

        void Start()
        {
            if (!Initialized)
            {
                Initialized = true;
                return;
            }

            if (!PhotonNetwork.OfflineMode && !this.gameObject.transform.parent.GetComponent<ProjectileHit>().ownPlayer.data.view.IsMine) return;


            PhotonNetwork.Instantiate(
                FireworkAssets.firework.name,
                transform.position,
                transform.rotation,
                0,
                new object[] { this.gameObject.transform.parent.GetComponent<PhotonView>().ViewID }
            );
        }
    }
    [RequireComponent(typeof(PhotonView))]
    public class FireworkEffect : MonoBehaviour, IPunInstantiateMagicCallback
    {
        private static readonly System.Random rng = new System.Random();

        private readonly float defaultDelay = 0.15f;
        private readonly int bullets = 10;
        private readonly float radius = 2f;

        private const int absMaxBullets = 25;
        private const int numBulletsThreshold = 3;
        private const float minProb = 2f/25f;
        private const float preventLagDelay = 0.5f;
        private const float exclusivityRange = 5f;

        private float delay {
            get
            {
                if (this.numPops > 0)
                {
                    return UnityEngine.Mathf.Clamp(this.defaultDelay * 3f / this.numPops, 0.03f, this.defaultDelay);
                }
                else
                {
                    return this.defaultDelay;
                }
            }
            set { }
        }

        internal int numPops;
        private float time;
        internal int pops = 0;

        private float extraDelay = 0f;

        private readonly float tolerance = 0.2f;

        private PhotonView view;
        private Transform parent;
        private Player player;
        private Gun gun;
        private ProjectileHit projectile;
        private Camera mainCam;
        private readonly Color[] colors = new Color[] { Color.red, Color.white, Color.blue, Color.green, Color.cyan, Color.magenta, Color.yellow };

        public void OnPhotonInstantiate(Photon.Pun.PhotonMessageInfo info)
        {
            object[] instantiationData = info.photonView.InstantiationData;

            GameObject parent = PhotonView.Find((int)instantiationData[0]).gameObject;

            this.gameObject.transform.SetParent(parent.transform);

            this.player = parent.GetComponent<ProjectileHit>().ownPlayer;
            this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();
        }
        void ResetTimer()
        {
            this.time = Time.time;
            this.extraDelay = 0f;
        }
        void Awake()
        {

        }
        void Start()
        {
            this.parent = this.gameObject.transform.parent;
            if (this.parent == null) { return; }
            this.projectile = this.gameObject.transform.parent.GetComponent<ProjectileHit>();
            this.view = this.gameObject.GetComponent<PhotonView>();
            this.numPops = this.gun.GetAdditionalData().fireworkProjectiles;

            this.mainCam = MainCam.instance.transform.GetComponent<Camera>();
            this.ResetTimer();
        }
        void Update()
        {
            if (this.parent == null) { return; }

            if (this.pops >= this.numPops)
            {
                Destroy(this);
                return;
            }
            if (Time.time < this.delay + this.time + this.extraDelay)
            {
                return;
            }
            else
            {
                this.ResetTimer();

                Vector3 pos = this.mainCam.WorldToScreenPoint(this.transform.position);
                pos.x /= (float)Screen.width;
                pos.y /= (float)Screen.height;

                if (pos.y >= 1f - this.tolerance && pos.y < 2f)
                {
                    this.Pop();
                }
            }
        }

        void Pop()
        {
            this.pops++;

            if (PhotonNetwork.OfflineMode || this.view.IsMine)
            {
                // randomly don't spawn fireworks from bullets if more than X number of firework-spawning bullets are on screen
                int numBullets = GetAllBulletsFromThisPlayerInRange();
                if (numBullets > numBulletsThreshold)
                {
                    if (UnityEngine.Random.Range(0f, 1f) >= 2f/numBullets)
                    {
                        this.extraDelay = preventLagDelay;
                        return; 
                    }
                }

                float randomRadiusFactor = UnityEngine.Random.Range(0.5f, 1.5f);
                this.view.RPC("RPCA_ShootFireWorks", RpcTarget.All, new object[] { FireworkEffect.rng.Next(0, this.colors.Length), this.gameObject.transform.position, this.bullets, this.radius * randomRadiusFactor });
            }

        }
        private int GetAllBulletsFromThisPlayerInRange()
        {
            ProjectileHit[] array = UnityEngine.Object.FindObjectsOfType<ProjectileHit>();

            return array.Where(f => IsValidBullet(f)).Count();
        }
        private bool IsValidBullet(ProjectileHit bullet)
        {
            if (bullet.ownPlayer != this.player) { return false; }
            return Vector3.Distance(bullet.transform.position, this.gameObject.transform.position) <= exclusivityRange;
        }

        void OnDisable()
        {
        }

        [PunRPC]
        void RPCA_ShootFireWorks(int randomColorInt, Vector3 position, int numBullets, float radius)
        {
            Color color = this.colors[randomColorInt];

            Gun newGun = this.player.gameObject.GetOrAddComponent<FireworkGun>();

            SpawnBulletsEffect effect = this.player.gameObject.AddComponent<SpawnBulletsEffect>();
            // set the position and direction to fire
            List<Vector3> positions = this.GetPositions(position, radius, numBullets);
            effect.SetPositions(positions);
            effect.SetDirections(this.GetDirections(position, positions));
            effect.SetNumBullets(numBullets);
            effect.SetTimeBetweenShots(0f);
            effect.SetInitialDelay(0.1f);

            // copy private gun stats over and reset a few public stats
            SpawnBulletsEffect.CopyGunStats(this.gun, newGun);

            newGun.spread = 1f;
            newGun.numberOfProjectiles = 1;
            newGun.projectiles = (from e in Enumerable.Range(0, newGun.numberOfProjectiles) from x in newGun.projectiles select x).ToList().Take(newGun.numberOfProjectiles).ToArray();
            newGun.damage = UnityEngine.Mathf.Clamp(newGun.damage/5f, 0.2f, float.MaxValue);
            newGun.projectileSpeed = 0.5f;
            newGun.drag = 20f;
            newGun.dragMinSpeed = 0f;
            newGun.gravity = 1f;
            newGun.reflects = 0;
            newGun.GetAdditionalData().allowStop = true;
            newGun.damageAfterDistanceMultiplier = 1f;
            newGun.projectileColor = color;
            newGun.objectsToSpawn = new ObjectsToSpawn[] { FireworkAssets.setInactiveDelayObjectToSpawn, PreventRecursion.stopRecursionObjectToSpawn };

            // set the gun of the spawnbulletseffect
            effect.SetGun(newGun);

        }
        private Vector2 CosSin(float angle)
        {
            return new Vector2(UnityEngine.Mathf.Cos(angle), UnityEngine.Mathf.Sin(angle));
        }

        private List<Vector3> GetPositions(Vector2 center, float radius, int bullets)
        {
            List<Vector3> res = new List<Vector3>() { };

            for (int i = 0; i < 5; i++)
            {
                res.Add(center + radius * this.CosSin(i * 360f / bullets));
            }

            return res;
        }

        private List<Vector3> GetDirections(Vector2 center, List<Vector3> shootPos)
        {
            List<Vector3> res = new List<Vector3>() { };

            foreach (Vector3 shootposition in shootPos)
            {
                res.Add(((Vector2)shootposition - center).normalized);
            }

            return res;
        }

    }
    public class FireworkGun : Gun
    { }

    public class SetFireworkInactiveDelay : MonoBehaviour
    {
        private const float inactiveDelay = 1f;

        void Start()
        {
            if (this.gameObject.transform.parent == null) { return; }
            else if (this.gameObject.transform.parent.GetComponent<ProjectileHit>()!=null)
            {
                ModdingUtils.Extensions.ProjectileHitExtension.GetAdditionalData(this.gameObject.transform.parent.GetComponent<ProjectileHit>()).inactiveDelay = inactiveDelay;
            }
        }
    }
}

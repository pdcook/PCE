using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;
using Photon.Pun;
using PCE.Extensions;
using System.Linq;
using PCE.Utils;

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
    //[RequireComponent(typeof(OffscreenManager))]
    public class FireworkEffect : MonoBehaviour, IPunInstantiateMagicCallback
    {
        private static readonly System.Random rng = new System.Random();

        private readonly int defaultDelay = 30;
        private readonly int bullets = 10;
        private readonly float radius = 2f;

        private int delay {
            get
            {
                if (this.pops > 0)
                {
                    return (int)UnityEngine.Mathf.RoundToInt(UnityEngine.Mathf.Clamp(this.defaultDelay*3f / this.pops, 1, this.defaultDelay));
                }
                else
                {
                    return this.defaultDelay;
                }
            }
            set { }
        }

        private int numPops;
        private int frames = 0;
        private int pops = 0;

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
        }
        void Update()
        {
            if (this.parent == null) { return; }

            if (this.pops >= this.numPops)
            {
                Destroy(this);
                return;
            }
            if (this.frames < this.delay)
            {
                this.frames++;
                return;
            }
            if (this.frames >= this.delay)
            {
                this.frames = 0;
                this.pops++;

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
                this.view.RPC("RPCA_ShootFireWorks", RpcTarget.All, new object[] { FireworkEffect.rng.Next(0, this.colors.Length), this.gameObject.transform.position, this.bullets });
            }

        }

        void OnDisable()
        {
        }

        [PunRPC]
        void RPCA_ShootFireWorks(int randomColorInt, Vector3 position, int numBullets)
        {
            Color color = this.colors[randomColorInt];

            Gun newGun = this.player.gameObject.AddComponent<FireworkGun>();

            SpawnBulletsEffect effect = this.player.gameObject.AddComponent<SpawnBulletsEffect>();
            // set the position and direction to fire
            List<Vector3> positions = this.GetPositions(position, this.radius, numBullets);
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
            newGun.damage /= 10f;
            newGun.projectileSpeed = 0.5f;
            newGun.drag = 20f;
            newGun.dragMinSpeed = 0f;
            newGun.gravity = 1f;
            newGun.GetAdditionalData().allowStop = true;
            newGun.damageAfterDistanceMultiplier = 1f;
            newGun.projectileColor = color;
            newGun.objectsToSpawn = new ObjectsToSpawn[] { PreventRecursion.stopRecursionObjectToSpawn };

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

    
}

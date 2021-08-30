using UnityEngine;
using UnboundLib;
using Photon.Pun;
using PCE.Extensions;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnboundLib.Utils;
using PCE.Cards;

namespace PCE.MonoBehaviours
{
    public class CombAssets
    {
        private static GameObject _comb = null;

        internal static GameObject comb
        {
            get
            {
                if (CombAssets._comb != null) { return CombAssets._comb; }
                else
                {
                    CombAssets._comb = new GameObject("PCE_Comb", typeof(CombEffect), typeof(PhotonView));
                    UnityEngine.GameObject.DontDestroyOnLoad(CombAssets._comb);

                    return CombAssets._comb;
                }
            }
            set { }
        }
    }
    public class CombSpawner : MonoBehaviour
    {
        private static bool Initialized = false;



        void Awake()
        {
            if (!Initialized)
            {
                PhotonNetwork.PrefabPool.RegisterPrefab(CombAssets.comb.name, CombAssets.comb);
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
                CombAssets.comb.name,
                transform.position,
                transform.rotation,
                0,
                new object[] { this.gameObject.transform.parent.GetComponent<PhotonView>().ViewID }
            );
        }
    }
    [RequireComponent(typeof(PhotonView))]
    public class CombEffect : MonoBehaviour, IPunInstantiateMagicCallback
    {
        private readonly float combDamageMult = 0.5f;

        private Player player;
        private Gun gun;
        private Gun newGun;
        private ProjectileHit projectile;
        private int layersToAdd = 0;

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
            // get the projectile, player, and gun this is attached to
            this.projectile = this.gameObject.transform.parent.GetComponent<ProjectileHit>();
            this.player = this.projectile.ownPlayer;
            this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();

            this.layersToAdd = this.player.data.currentCards.Where(card => card.cardName == "Comb").Count();

            // create a new gun for the spawnbulletseffect
            this.newGun = this.player.gameObject.AddComponent<CombGun>();

            SpawnBulletsEffect effect = this.player.gameObject.AddComponent<SpawnBulletsEffect>();
            // set the position and direction to fire

            effect.SetDirection(((Quaternion)typeof(Gun).InvokeMember("getShootRotation",
                BindingFlags.Instance | BindingFlags.InvokeMethod |
                BindingFlags.NonPublic, null, this.gun, new object[] { 0, 0, 0f })) * Vector3.forward);

            List<Vector3> positions = new List<Vector3>() { };
            for (int b = 1; b < (2*this.layersToAdd) + 1; b++)
            {
                positions.Add(this.projectile.transform.position + (0.25f)*((b % 2 == 0) ? (float) b : -((float) b + 1f)) * this.projectile.transform.right);
            }
            
            effect.SetPositions(positions);
            effect.SetNumBullets(this.layersToAdd*2);
            effect.SetTimeBetweenShots(0f);
            effect.SetInitialDelay(0f);

            // copy gun stats over
            SpawnBulletsEffect.CopyGunStats(this.gun, newGun);
            newGun.objectsToSpawn = newGun.objectsToSpawn.Where(obj => obj.AddToProjectile.GetComponent<CombSpawner>() == null && obj.AddToProjectile.GetComponent<LaserGunSpawner>() == null).ToArray();
            newGun.bursts = 1;
            newGun.numberOfProjectiles = 1;
            newGun.damage *= this.combDamageMult;

            // set the gun of the spawnbulletseffect
            effect.SetGun(newGun);
        }
    }
    class CombGun : Gun
    { }
}

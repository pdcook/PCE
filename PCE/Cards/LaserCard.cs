using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using System.Linq;
using PCE.MonoBehaviours;
using System.Reflection;
using Photon.Pun;
using PCE.Utils;
using System.Collections.ObjectModel;
using UnboundLib.Utils;
using PCE.Extensions;
using HarmonyLib;
using System.Runtime.CompilerServices;
using System;
using InControl;
using ModdingUtils.MonoBehaviours;
using UnboundLib;
using System.Collections;
namespace PCE.Cards
{
    public class LaserCard : CustomCard
    {
        internal static readonly float attackSpeedMult = 6f; // lasers are 6x slower than the normal gun
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.GetAdditionalData().laserGun = UnityEngine.GameObject.Instantiate(LaserAssets.laserGunSpawner, gun.gameObject.transform);
            gun.GetAdditionalData().canBeLaser = true;
            gun.reloadTime += 0.5f;

            LaserEffect laserEffect = player.gameObject.GetOrAddComponent<LaserEffect>();
            laserEffect.SetLivesToEffect(int.MaxValue);
            laserEffect.gunStatModifier.attackSpeed_mult = LaserCard.attackSpeedMult;
            laserEffect.gunStatModifier.bursts_mult = 0;
            laserEffect.gunStatModifier.evenSpread_mult = 0f;
            laserEffect.gunStatModifier.multiplySpread_mult = 0f;
            laserEffect.gunStatModifier.numberOfProjectiles_mult = 0;
            laserEffect.gunStatModifier.timeBetweenBullets_add = 100f;
            laserEffect.applyImmediately = false;
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
            //return "<b>L</b>ight <b>A</b>mplification by <b>S</b>timulated <b>E</b>mission of <b>R</b>adiation";
            return "Switch to a Laser cannon by pressing Middle Mouse or D-Pad Down";
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


        internal static IEnumerator RemoveLasersBetweenBattles()
        {
            foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (gameObject.name == "LaserTrail(Clone)")
                {
                    UnityEngine.GameObject.Destroy(gameObject);
                }
            }
            yield break;
        }
    }
    class LaserEffect : ReversibleEffect
    {
        public override void OnOnDisable()
        {
            base.ClearModifiers(false);
        }
    }
    // postfix PlayerActions constructor to add controls for the crosshair distance
    [HarmonyPatch(typeof(PlayerActions))]
    [HarmonyPatch(MethodType.Constructor)]
    [HarmonyPatch(new Type[] { })]
    class PlayerActionsPatchPlayerActions
    {
        private static void Postfix(PlayerActions __instance)
        {
            __instance.GetAdditionalData().switchWeapon = (PlayerAction)typeof(PlayerActions).InvokeMember("CreatePlayerAction",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, __instance, new object[] { "Switch Weapon" });

        }
    }
    // postfix PlayerActions to add controls for switching weapons
    [HarmonyPatch(typeof(PlayerActions), "CreateWithControllerBindings")]
    class PlayerActionsPatchCreateWithControllerBindings
    {
        private static void Postfix(ref PlayerActions __result)
        {
            __result.GetAdditionalData().switchWeapon.AddDefaultBinding(InputControlType.DPadDown);
        }
    }    
    // postfix PlayerActions to add controls for switching weapons
    [HarmonyPatch(typeof(PlayerActions), "CreateWithKeyboardBindings")]
    class PlayerActionsPatchCreateWithKeyboardBindings
    {
        private static void Postfix(ref PlayerActions __result)
        {
            __result.GetAdditionalData().switchWeapon.AddDefaultBinding(Mouse.MiddleButton);
        }
    }
    // prefix/postfix ApplyStats to disable/enable lasergun if its currently active
    [HarmonyPatch(typeof(ApplyCardStats), "ApplyStats")]
    class ApplyCardStatsPatchApplyStats
    {
        private static void Prefix(ApplyCardStats __instance, out bool __state)
        {
            __state = false;
            Gun gun = ((Player)Traverse.Create(__instance).Field("playerToUpgrade").GetValue()).GetComponent<Holding>().holdable.GetComponent<Gun>();
            if (gun.GetAdditionalData().isLaser)
            {
                gun.GetAdditionalData().isLaser = false;
                gun.player.GetComponent<LaserEffect>().ClearModifiers(false);
                __state = true;
            }
        }
        private static void Postfix(ApplyCardStats __instance, bool __state)
        {
            if (__state)
            {
                Gun gun = ((Player)Traverse.Create(__instance).Field("playerToUpgrade").GetValue()).GetComponent<Holding>().holdable.GetComponent<Gun>();
                gun.GetAdditionalData().isLaser = true;
                gun.player.GetComponent<LaserEffect>().gunStatModifier.attackSpeed_mult = LaserCard.attackSpeedMult;
                gun.player.GetComponent<LaserEffect>().ApplyModifiers();
            }
        }
    }
    // postfix GeneralInput to add controls for switching weapons
    [HarmonyPatch(typeof(GeneralInput), "Update")]
    class GeneralInputPatchUpdate
    {
        private static void Postfix(GeneralInput __instance)
        {
            try
            {
                if (__instance.GetComponent<CharacterData>().playerActions.GetAdditionalData().switchWeapon.WasPressed && __instance.GetComponent<Holding>().holdable.GetComponent<Gun>().GetAdditionalData().canBeLaser)
                {
                    __instance.GetComponent<Holding>().holdable.GetComponent<Gun>().GetAdditionalData().isLaser = !__instance.GetComponent<Holding>().holdable.GetComponent<Gun>().GetAdditionalData().isLaser;
                
                    if (__instance.GetComponent<Holding>().holdable.GetComponent<Gun>().GetAdditionalData().isLaser)
                    {
                        __instance.GetComponent<LaserEffect>().ApplyModifiers();
                    }
                    if (!__instance.GetComponent<Holding>().holdable.GetComponent<Gun>().GetAdditionalData().isLaser)
                    {
                        __instance.GetComponent<LaserEffect>().ClearModifiers(false);
                    }

                }
            }
            catch { }
        }
    }
    // prefix Gun.Attack to skip if firing a laser
    [HarmonyPatch(typeof(Gun), "Attack")]
    class GunPatchAttack
    {
        private static bool Prefix(Gun __instance, ref bool __result, bool forceAttack)
        {

            if (__instance.GetAdditionalData().isLaser && __instance.GetAdditionalData().laserGun != null)
            {
                float usedCooldown = (float)typeof(Gun).GetProperty("usedCooldown", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);

                if (__instance.sinceAttack < usedCooldown && !forceAttack)
                {
                    __result = false;
                    return false;
                }
                if (__instance.isReloading && !forceAttack)
                {
                    __result = false;
                    return false;
                }
                __instance.sinceAttack = 0f;
                
                // fire laser
                __instance.GetAdditionalData().laserGun.GetComponent<LaserGunSpawner>().SpawnLaser();

                __instance.player.GetComponent<LaserEffect>().ApplyModifiers();

                // no recoil
                //Rigidbody2D rig = (Rigidbody2D)Traverse.Create(__instance).Field("rig").GetValue();
                //rig.AddForce(10f * rig.mass * __instance.recoil * Mathf.Clamp(usedCooldown, 0f, 1f) * - __instance.transform.up, ForceMode2D.Impulse);
                
                // use ammo
                ((GunAmmo)Traverse.Create(__instance).Field("gunAmmo").GetValue()).Shoot(__instance.GetAdditionalData().laserGun);

                // audio/visual effects
                GamefeelManager.GameFeel(__instance.transform.up * __instance.shake * 2f);
                //__instance.soundGun.PlayShot(1);

                __result = true;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public class LaserAssets
    {
        private static GameObject _laserGunSpawner;
        internal static GameObject laserGunSpawner
        {
            get
            {
                if (LaserAssets._laserGunSpawner != null) { return LaserAssets._laserGunSpawner; }
                else
                {
                    LaserAssets._laserGunSpawner = new GameObject("LaserGunSpawner", typeof(LaserGunSpawner));
                    UnityEngine.GameObject.DontDestroyOnLoad(LaserAssets._laserGunSpawner);

                    return LaserAssets._laserGunSpawner;
                }
            }
            set { }
        }

        private static GameObject _laserTrail = null;

        internal static GameObject laserTrail
        {
            get
            {
                if (LaserAssets._laserTrail != null) { return LaserAssets._laserTrail; }
                else
                {
                    LaserAssets._laserTrail = new GameObject("LaserTrail", typeof(LaserHurtbox), typeof(PhotonView), typeof(LineRenderer));
                    UnityEngine.GameObject.DontDestroyOnLoad(LaserAssets._laserTrail);

                    return LaserAssets._laserTrail;
                }
            }
            set { }
        }
        private static Material _material = null;
        internal static Material material
        {
            get
            {
                if (LaserAssets._material != null) { return _material; }

                List<CardInfo> activecards = ((ObservableCollection<CardInfo>)typeof(CardManager).GetField("activeCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).ToList();
                List<CardInfo> inactivecards = (List<CardInfo>)typeof(CardManager).GetField("inactiveCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                List<CardInfo> allcards = activecards.Concat(inactivecards).ToList();
                CardInfo targetBounceCard = allcards.Where(card => card.gameObject.name == "TargetBounce").ToList()[0];
                Gun targetBounceGun = targetBounceCard.GetComponent<Gun>();
                ObjectsToSpawn trailToSpawn = (new List<ObjectsToSpawn>(targetBounceGun.objectsToSpawn)).Where(objectToSpawn => objectToSpawn.AddToProjectile.GetComponent<BounceTrigger>() != null).ToList()[0];
                LaserAssets._material = trailToSpawn.AddToProjectile.GetComponentInChildren<TrailRenderer>().material;

                return LaserAssets._material;
            }
            set { }
        }
    }
    public class LaserGunSpawner : MonoBehaviour
    {
        private static bool Initialized = false;



        void Awake()
        {
            if (!Initialized)
            {
                PhotonNetwork.PrefabPool.RegisterPrefab(LaserAssets.laserTrail.name, LaserAssets.laserTrail);
            }
        }

        void Start()
        {
            if (!Initialized)
            {
                Initialized = true;
                return;
            }

            if (!PhotonNetwork.OfflineMode && !this.gameObject.transform.parent.GetComponent<Gun>().player.data.view.IsMine) return;
        }
        internal void SpawnLaser()
        {

            PhotonNetwork.Instantiate(
                LaserAssets.laserTrail.name,
                transform.position,
                transform.rotation,
                0,
                new object[] { this.gameObject.transform.parent.GetComponent<Gun>().player.data.view.ViewID }
            );
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
    [RequireComponent(typeof(PhotonView))]
    class LaserHurtbox : MonoBehaviour, IPunInstantiateMagicCallback
    {
        private float syncTime;
        private readonly float syncDelay = 0.1f;

        private readonly float baseDuration = 6f;
        private float duration;
        private readonly float baseDamageMultiplier = 4f;

        private float startTime;
        private float damageMultiplier;
        public float baseDamage;

        private readonly float[] minmaxwidth = new float[] { 0.1f, 0.5f };

        private readonly float damageTickDelay = 0.1f;
        private float startDamageTime;

        private LineRenderer trail;
        private readonly int MAX = 100000;
        private int numPos;
        public Gun gun;
        public Player player;

        private PhotonView view;

        private Vector3[] positions3d;

        private bool synced = false;

        private int layerMask;

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

            object[] instantiationData = info.photonView.InstantiationData;

            GameObject parentPlayer = PhotonView.Find((int)instantiationData[0]).gameObject;

            GameObject parent = parentPlayer.GetComponent<Player>().GetComponent<Holding>().holdable.GetComponent<Gun>().gameObject;

            this.gameObject.transform.SetParent(parent.transform);

            this.gun = this.gameObject.transform.parent.GetComponent<Gun>();
            this.player = this.gun.player;
            this.baseDamage = UnityEngine.Mathf.Clamp(this.gun.damage, 1f, float.MaxValue);
        }

        void Destroy()
        {
            if (PhotonNetwork.OfflineMode || this.gameObject.GetComponent<PhotonView>().IsMine) { PhotonNetwork.Destroy(this.gameObject.GetComponent<PhotonView>()); }
            Destroy(this.gameObject);
        }

        void Awake()
        {
            this.positions3d = new Vector3[MAX];
            this.trail = this.gameObject.GetComponent<LineRenderer>();
        }
        void Start()
        {
            this.view = this.GetComponent<PhotonView>();

            if (this.view.IsMine)
            {
                // 0.3f is default attack speed, smaller attackSpeed stat means faster shooting
                this.duration = UnityEngine.Mathf.Clamp(this.baseDuration * this.gun.attackSpeed / (LaserCard.attackSpeedMult * 0.3f), 1f, 12f);
            }
            else
            {
                this.duration = UnityEngine.Mathf.Clamp(this.baseDuration * this.gun.attackSpeed / (0.3f), 0.5f, 10f);
            }

            this.layerMask = ~LayerMask.GetMask("BackgroundObject","Player");

            this.material = LaserAssets.material;
            this.color = Color.red;

            this.ResetDamageTimer();
            this.ResetTimer();
            this.damageMultiplier = this.baseDamageMultiplier;
            this.trail.enabled = true;
            if (this.color.a < 0f) { this.color = new Color(this.color.r, this.color.g, this.color.b, 0f); }
            this.UpdateWidth();

            this.SyncAppearance();
            this.SyncStats();
            this.synced = false;

            // create the laser once and only once
            if (this.trail != null && !this.player.data.dead)
            {
                this.positions3d = this.GetPositions();
                this.UpdatePositions(this.positions3d.ToArray());
            }
        }
        void DestroyAfter()
        {
            if (Time.time - this.startTime > 2f * this.duration)
            {
                this.Destroy();
            }
        }
        void Update()
        {
            if (this.trail == null) { return; }

            // sync final position only
            if (!this.synced)
            {
                this.synced = true;
                this.SyncTrail();
                this.ResetSyncTimer();
            }
            else if (this.synced && Time.time >= this.syncTime + this.syncDelay)
            {
                this.SyncTrail();
                this.ResetSyncTimer();
            }

            this.DestroyAfter();
            this.numPos = this.positions3d.Length;
            this.UpdateDamage();
            this.UpdateColor();
            this.UpdateWidth();
            if (this.damageMultiplier > 0f && Time.time > this.startDamageTime + this.damageTickDelay)
            {
                this.ResetDamageTimer();
                this.HurtBox(positions3d.toVector2Array().ToList<Vector2>().Take(this.numPos).ToArray());
            }
            

        }
        /*
        void FixedUpdate()
        {
            if (this.trail != null && !this.player.data.dead)
            {
                this.positions3d = this.GetPositions();
                this.UpdatePositions(this.positions3d.ToArray());
            }
        }*/
        internal void UpdatePositions(Vector3[] positions)
        {
            this.trail.positionCount = positions.Length;
            this.trail.SetPositions(positions.ToArray());
        }
        private readonly int maxReflects = 1000;
        private readonly float maxDistance = 1000f;
        Vector3[] GetPositions()
        {
            if (this.gun == null || this.gun.shootPosition == null)
            {
                return this.positions3d;
            }
            List<Vector3> positions = new List<Vector3>() { };

            positions.Add(this.gun.shootPosition.position);

            Vector3 direction = ((Quaternion)typeof(Gun).InvokeMember("getShootRotation",
                                   BindingFlags.Instance | BindingFlags.InvokeMethod |
                                  BindingFlags.NonPublic, null, this.gun, new object[] { 0, 0, 0f })) * Vector3.forward;

            // REMOVE COMPENSATION FOR BULLET SPEED
            if (!this.player.data.stats.GetAdditionalData().removeSpeedCompensation)
            {
                direction -= Vector3.up * 0.13f / Mathf.Clamp(this.gun.projectileSpeed, 1f, 100f);
            }

            int i = 0;
            RaycastHit2D hit = Physics2D.Raycast(this.gun.shootPosition.position, direction, maxDistance, this.layerMask);

            // if the first hit is nearly the same as the starting point, just return
            if (hit && Vector3.Distance(hit.point, positions[0]) < 0.001f)
            {
                return positions.ToArray();
            }

            while (hit && i < maxReflects)
            {
                i++;
                positions.Add(hit.point);
                direction = Vector2.Reflect(direction, hit.normal);
                hit = Physics2D.Raycast(hit.point + (Vector2)(0.01f * direction), direction, maxDistance, this.layerMask);
            }

            // add the point at "infinity"
            if (i < maxReflects)
            {
                positions.Add(positions[positions.Count - 1] + direction * 100000f);
            }

            return positions.ToArray();
        }
        void UpdateDamage()
        {
            if (this.intensity >= 1f)
            {
                this.damageMultiplier = this.baseDamageMultiplier * this.intensity;
            }
            else
            {
                float damageMult = (-8f / this.duration) * ((this.intensity - 1f) * (-1f * this.duration)) + 1f;
                this.damageMultiplier = this.baseDamageMultiplier * UnityEngine.Mathf.Clamp(damageMult, 0f, 1f);
            }
        }
        void UpdateColor()
        {
            this.color = new Color(this.color.r, this.color.g, this.color.b, UnityEngine.Mathf.Clamp(this.color.a * this.intensity, 0f, 1f));
        }
        void UpdateWidth()
        {
            this.width = UnityEngine.Mathf.Clamp((this.minmaxwidth[1] - this.minmaxwidth[0]) * ((this.baseDamage - 0.1f) * this.damageMultiplier) / ((3f - 0.1f) * this.baseDamageMultiplier) + this.minmaxwidth[0], this.minmaxwidth[0], this.minmaxwidth[1]);
        }
        void HurtBox(Vector2[] vertices)
        {
            List<Collider2D> damaged = new List<Collider2D>() { };

            // take every consecutive pair of vertices, generate a capsule to use Physics.OverlapCapsule to see if it intersects with a damagable
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                Vector2 mid = Vector2.Lerp(vertices[i], vertices[i + 1], 0.5f);
                Vector2 size = new Vector2(this.trail.startWidth, Vector2.Distance(vertices[i], vertices[i + 1]));
                float angle = Vector2.SignedAngle(Vector2.up, vertices[i] - vertices[i + 1]);

                Collider2D[] colliders = Physics2D.OverlapCapsuleAll(mid, size, CapsuleDirection2D.Vertical, angle);
                foreach (Collider2D collider in colliders.Except(damaged))
                {
                    Damagable componentInParent = collider.GetComponentInParent<Damagable>();
                    if (componentInParent)
                    {
                        damaged.Add(collider);
                        if ((bool)typeof(Gun).InvokeMember("CheckIsMine",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, this.gun, new object[] { }))
                        {
                            componentInParent.CallTakeDamage(Vector2.up * this.baseDamage * this.damageMultiplier, collider.transform.position, this.gun.gameObject, this.player, true);
                        }
                    }
                }
            }
        }
        private void ResetSyncTimer()
        {
            this.syncTime = Time.time;
        }
        private void ResetTimer()
        {
            this.startTime = Time.time;
        }
        private void ResetDamageTimer()
        {
            this.startDamageTime = Time.time;
        }
        void SyncTrail()
        {
            if (!PhotonNetwork.OfflineMode && this.gameObject.GetComponent<PhotonView>().IsMine)
            {
                this.view.RPC(nameof(RPCA_SyncTrail), RpcTarget.Others, new object[] { this.positions3d.toVector2Array().ToList().Take(this.numPos).ToArray(), this.numPos, this.trail.startWidth, this.trail.startColor.r, this.trail.startColor.g, this.trail.startColor.b, this.trail.startColor.a });
            }
        }
        void SyncAppearance()
        {
            if (!PhotonNetwork.OfflineMode && this.gameObject.GetComponent<PhotonView>().IsMine)
            {
                this.view.RPC(nameof(RPCA_SyncAppearance), RpcTarget.Others, new object[] { this.trail.startWidth, this.trail.startColor.r, this.trail.startColor.g, this.trail.startColor.b, this.trail.startColor.a });
            }
        }
        void SyncStats()
        {
            if (!PhotonNetwork.OfflineMode && this.gameObject.GetComponent<PhotonView>().IsMine)
            {
                this.view.RPC(nameof(RPCA_SyncStats), RpcTarget.Others, new object[] { this.duration });
            }
        }
        [PunRPC]
        void RPCA_SyncStats(float duration)
        {
            this.duration = duration;
        }
        [PunRPC]
        void RPCA_SyncAppearance(float width, float r, float g, float b, float a)
        {
            this.width = width;
            this.color = new Color(r, g, b, a);
        }
        [PunRPC]
        void RPCA_SyncTrail(Vector2[] positions, int num, float width, float r, float g, float b, float a)
        {
            // unparent once the host starts syncing the position
            if (this.gameObject.transform.parent != null) { this.gameObject.transform.SetParent(null); }
            this.trail.positionCount = num;
            this.trail.SetPositions(positions.toVector3Array());
            this.numPos = this.trail.GetPositions(this.positions3d);
            this.width = width;
            this.color = new Color(r, g, b, a);
        }

    }
}

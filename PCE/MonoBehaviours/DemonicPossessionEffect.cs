using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;
using Photon.Pun;

namespace PCE.MonoBehaviours
{
    /*
        List of effects:
            1. Extra vibrate
 
     */

    public class DemonicPossessionEffect : MonoBehaviour
    {
        private readonly float maxDuration = 10f;
        private readonly float minDuration = 2f;

        private Player player;
        private Gun gun;
        private CharacterData data;
        private HealthHandler health;
        private Gravity gravity;
        private Block block;
        private GunAmmo gunAmmo;
        private CharacterStatModifiers statModifiers;


        private readonly System.Random rng = new System.Random();

        internal float xshakemag = 0.04f;
        internal float yshakemag = 0.02f;

        private List<Func<Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, MonoBehaviour>> effectFuncs = new List<Func<Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, MonoBehaviour>>();
        private MonoBehaviour currentEffect;
        private int effectID;
        private float effectDuration = 0f;
        private float timeOfLastEffect;
        private bool ready = false;
        private readonly int framesToWait = 1;
        private int framesWaited = 0;

        void Awake()
        {
            this.player = gameObject.GetComponent<Player>();
            this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            this.data = this.player.GetComponent<CharacterData>();
            this.health = this.player.GetComponent<HealthHandler>();
            this.gravity = this.player.GetComponent<Gravity>();
            this.block = this.player.GetComponent<Block>();
            this.gunAmmo = this.gun.GetComponentInChildren<GunAmmo>();
            this.statModifiers = this.player.GetComponent<CharacterStatModifiers>();


            this.effectFuncs.Add(this.Effect_NullEffect);
            this.effectFuncs.Add(this.Effect_NoGravityEffect);
            this.effectFuncs.Add(this.Effect_InvisibleEffect);
            this.effectFuncs.Add(this.Effect_ShakeEffect);
            this.effectFuncs.Add(this.Effect_PopEffect);
            this.effectFuncs.Add(this.Effect_NukeEffect);

        }

        void Start()
        {
            this.ready = false;
            this.timeOfLastEffect = -1f;
            this.effectDuration = -1f;
            //this.GetNewEffect();
            //this.GetNewDuration();
            //this.ApplyCurrentEffect();
        }

        void Update()
        {
            if (this.ready)
            {
                this.ready = false;
                this.GetNewEffect();
                this.GetNewDuration();
                this.ApplyCurrentEffect();
            }
            else if (Time.time >= this.timeOfLastEffect + this.effectDuration)
            {
                this.ClearEffects();
            }

            float rx = (float)this.rng.NextGaussianDouble();
            float ry = (float)this.rng.NextGaussianDouble();


            Vector3 position = new Vector3(xshakemag*rx, yshakemag*ry, 0.0f);

            this.player.transform.position += position;

        }
        public void OnDestroy()
        {
            this.ClearEffects();
        }
        public void GetNewDuration()
        {
            float newEffectDuration = (this.maxDuration-this.minDuration)*(float)this.rng.NextDouble() + this.minDuration;

            if (PhotonNetwork.OfflineMode)
            {
                // offline mode
                this.effectDuration = newEffectDuration;
            }
            else if (base.GetComponent<PhotonView>().IsMine)
            {
                base.GetComponent<PhotonView>().RPC("RPCA_SetNewDuration", RpcTarget.All, new object[] { newEffectDuration });
            }
        }
        public void GetNewEffect()
        {
            int newEffectID = this.rng.Next(0, this.effectFuncs.Count);

            if (PhotonNetwork.OfflineMode)
            {
                // offline mode
                this.effectID = newEffectID;
            }
            else if (base.GetComponent<PhotonView>().IsMine)
            {
                base.GetComponent<PhotonView>().RPC("RPCA_SetNewEffectID", RpcTarget.All, new object[] { newEffectID });
            }
        }
        public void ApplyCurrentEffect()
        {
            this.currentEffect = this.effectFuncs[this.effectID](this.player, this.gun, this.gunAmmo, this.data, this.health, this.gravity, this.block, this.statModifiers);
            ColorFlash thisColorFlash = this.player.gameObject.GetOrAddComponent<ColorFlash>();
            thisColorFlash.SetNumberOfFlashes(3);
            thisColorFlash.SetDuration(0.25f);
            thisColorFlash.SetDelayBetweenFlashes(0.25f);
            this.ResetEffectTimer();
        }
        public void ResetEffectTimer()
        {
            this.timeOfLastEffect = Time.time;
        }
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }
        public void ClearEffects()
        {
            if (this.currentEffect != null)
            {
                Destroy(this.currentEffect);
            }

            if (!this.ready && this.framesWaited < this.framesToWait)
            {
                this.framesWaited++;
            }
            else if (!this.ready)
            {
                this.framesWaited = 0;
                this.ready = true;
            }

        }
        public MonoBehaviour Effect_NoGravityEffect(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            GravityEffect effect = player.gameObject.GetOrAddComponent<GravityEffect>();
            effect.SetGravityForceMultiplier(0f);
            effect.SetDuration(this.effectDuration);
            return effect;
        }
        public MonoBehaviour Effect_InvisibleEffect(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            ColorEffect effect = player.gameObject.GetOrAddComponent<ColorEffect>();
            effect.SetColorMax(Color.clear);
            effect.SetColorMin(Color.clear);
            return effect;
        }
        public MonoBehaviour Effect_ShakeEffect(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            DemonicPossessionShakeEffect effect = player.gameObject.GetOrAddComponent<DemonicPossessionShakeEffect>();
            effect.SetXMagMult(10f);
            effect.SetYMagMult(10f);
            return effect;
        }
        public MonoBehaviour Effect_NullEffect(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            return null;
        }
        public MonoBehaviour Effect_PopEffect(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            PopEffect effect = player.gameObject.GetOrAddComponent<PopEffect>();
            effect.SetPeriod(3f);
            effect.SetSpacing(5f);
            return effect;
        }
        public MonoBehaviour Effect_NukeEffect(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Gun newGun = this.gameObject.AddComponent<Gun>();

            SpawnBulletsEffect effect = player.gameObject.GetOrAddComponent<SpawnBulletsEffect>();
            effect.SetDirection(new Vector3(0f, 1f, 0f));
            effect.SetPosition(new Vector3(0f, 100f, 0f));
            effect.SetNumBullets(1);

            SpawnBulletsEffect.CopyGunStats(gun, newGun);

            newGun.damage = 1000f;
            newGun.reloadTime = float.MaxValue;
            newGun.ammoReg = 1;
            newGun.ammo = 1;
            newGun.projectileSpeed = 2f;
            newGun.projectielSimulatonSpeed = 2f;
            newGun.explodeNearEnemyRange = 5f;
            newGun.explodeNearEnemyDamage = 1000f;
            newGun.projectileSize = 100f;
            newGun.projectileColor = Color.white;

            effect.SetGun(newGun);

            ColorFlash thisColorFlash = this.player.gameObject.GetOrAddComponent<ColorFlash>();
            thisColorFlash.SetNumberOfFlashes(10);
            thisColorFlash.SetDuration(0.15f);
            thisColorFlash.SetDelayBetweenFlashes(0.15f);
            thisColorFlash.SetColorMax(Color.red);
            thisColorFlash.SetColorMin(Color.red);

            return effect;
        }

        [PunRPC]
        public void RPCA_SetNewEffectID(int effectID)
        {
            this.effectID = effectID;
        }
        [PunRPC]
        public void RPCA_SetNewDuration(float duration)
        {
            this.effectDuration = duration;
        }

    }
}

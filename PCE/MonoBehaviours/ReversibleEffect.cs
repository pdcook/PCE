using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;
using PCE.Extensions;

namespace PCE.MonoBehaviours
{
    public class ReversibleEffect : MonoBehaviour
    {

		internal Player player;
        internal CharacterStatModifiers characterStatModifiers;
        internal Gun gun;
        internal GunAmmo gunAmmo;
        internal Gravity gravity;
        internal HealthHandler health;
        internal CharacterData data;
        internal Block block;

        internal int livesToEffect = 1;
        private int livesEffected = 0;

        public GunStatModifier gunStatModifier = new GunStatModifier();
        public GunAmmoStatModifier gunAmmoStatModifier = new GunAmmoStatModifier();
        public CharacterStatModifiersModifier characterStatModifiersModifier = new CharacterStatModifiersModifier();
        public GravityModifier gravityModifier = new GravityModifier();
        public BlockModifier blockModifier = new BlockModifier();

        public bool applyImmediately = true;
        private bool modifiersActive = false;
        private bool wasActiveLastFrame = true;

        internal int numEnemyPlayers
        {
            get
            {
                if (this.player == null) { return -1; }
                int num = PlayerStatus.GetNumberOfEnemyPlayers(this.player);
                if (num > 0)
                {
                    return num;
                }
                else
                {
                    return 1;
                }
            }
            set
            { }
        }

        public void Awake()
        {
            this.player = gameObject.GetComponent<Player>();
            this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            this.data = this.player.GetComponent<CharacterData>();
            this.health = this.player.GetComponent<HealthHandler>();
            this.gravity = this.player.GetComponent<Gravity>();
            this.block = this.player.GetComponent<Block>();
            this.gunAmmo = this.gun.GetComponentInChildren<GunAmmo>();
            this.characterStatModifiers = this.player.GetComponent<CharacterStatModifiers>();
            this.OnAwake();
        }
        public virtual void OnAwake()
        {

        }

        public void OnEnable()
        {
            if (this.livesEffected >= this.livesToEffect)
            {
                Destroy(this);
            }
            this.OnOnEnable();
        }

        public virtual void OnOnEnable()
        {

        }

        public void Start()
        {

            this.wasActiveLastFrame = PlayerStatus.PlayerAliveAndSimulated(this.player);

            this.OnStart();

            if (this.applyImmediately)
            {
                this.ApplyModifiers();
            }


        }
        public virtual void OnStart()
        {
            // this is where derived effects should modify each of the following:
            /* base.gunStatModifier
             * base.gunAmmoStatModifier
             * base.playerColorModifier
             * base.characterStatModifiersModifier
             * base.blockModifier
             * 
             * and optionally, if the effect should not be applied until later, base.applyImmediately
             */
        }

        void FixedUpdate()
        {
            this.OnFixedUpdate();
        }
        public virtual void OnFixedUpdate()
        {

        }
        void Update()
        {
            if (this.wasActiveLastFrame && !PlayerStatus.PlayerAliveAndSimulated(this.player))
            {
                this.livesEffected++;
            }


            if (this.livesEffected >= this.livesToEffect)
            {
                Destroy(this);
            }

            this.OnUpdate();
        }
        public virtual void OnUpdate()
        {

        }
        public void LateUpdate()
        {
            this.wasActiveLastFrame = PlayerStatus.PlayerAliveAndSimulated(this.player);

            this.OnLateUpdate();
        }
        public virtual void OnLateUpdate()
        {
        }
        public void OnDisable()
        {
            
            this.livesEffected++;

            if (this.livesEffected >= this.livesToEffect)
            {
                Destroy(this);
            }

        }
        public virtual void OnOnDisable()
        {

        }
        public void OnDestroy()
        {
            this.ClearModifiers();
            this.OnOnDestroy();
		}

        public virtual void OnOnDestroy()
        {
            // derived effects should put any necessary cleanup here
        }
        internal void ApplyModifiers()
        {
            if (this.modifiersActive) { return; }
            this.gunStatModifier.ApplyGunStatModifier(this.gun);
            this.gunAmmoStatModifier.ApplyGunAmmoStatModifier(this.gunAmmo);
            this.characterStatModifiersModifier.ApplyCharacterStatModifiersModifier(this.characterStatModifiers);
            this.gravityModifier.ApplyGravityModifier(this.gravity);
            this.blockModifier.ApplyBlockModifier(this.block);
            this.modifiersActive = true;
        }
        internal void ClearModifiers(bool clear = true)
        {
            if (!this.modifiersActive) { return; }
            this.gunStatModifier.RemoveGunStatModifier(this.gun, clear);
            this.gunAmmoStatModifier.RemoveGunAmmoStatModifier(this.gunAmmo, clear);
            this.characterStatModifiersModifier.RemoveCharacterStatModifiersModifier(this.characterStatModifiers, clear);
            this.gravityModifier.RemoveGravityModifier(this.gravity, clear);
            this.blockModifier.RemoveBlockModifier(this.block, clear);
            this.modifiersActive = false;

        }
        public void Destroy()
        {
            this.ClearModifiers();
            UnityEngine.Object.Destroy(this);
        }

        public void SetLivesToEffect(int lives = 1)
        {
            this.livesToEffect = lives;
        }

    }
}

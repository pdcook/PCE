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

		private Player player;
        private CharacterStatModifiers characterStatModifiers;
        private Gun gun;
        private GunAmmo gunAmmo;
        private Gravity gravity;

        public GunStatModifier gunStatModifier = new GunStatModifier();
        public GunAmmoStatModifier gunAmmoStatModifier = new GunAmmoStatModifier();
        //public PlayerColorModifier playerColorModifier = new PlayerColorModifier();
        public CharacterStatModifiersModifier characterStatModifiersModifier = new CharacterStatModifiersModifier();
        public GravityModifier gravityModifier = new GravityModifier();

        public void Awake()
        {
            this.player = this.gameObject.GetComponent<Player>();
            this.characterStatModifiers = this.player.data.stats;
            this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            this.gunAmmo = this.gun.GetComponentInChildren<GunAmmo>();
            this.gravity = this.player.GetComponent<Gravity>();
            this.OnAwake();
        }
        public virtual void OnAwake()
        {

        }

        public void Start()
        {

            this.OnStart();

            this.gunStatModifier.ApplyGunStatModifier(this.gun);
            this.gunAmmoStatModifier.ApplyGunAmmoStatModifier(this.gunAmmo);
            this.characterStatModifiersModifier.ApplyCharacterStatModifiersModifier(this.characterStatModifiers);
            this.gravityModifier.ApplyGravityModifier(this.gravity);

        }
        public virtual void OnStart()
        {
            // this is where derived effects should modify each of the following:
            /* base.gunStatModifier
             * base.gunAmmoStatModifier
             * base.playerColorModifier
             * base.characterStatModifiersModifier
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
            this.OnUpdate();
        }
        public virtual void OnUpdate()
        {

        }
        public void OnDestroy()
        {
            this.gunStatModifier.RemoveGunStatModifier(this.gun);
            this.gunAmmoStatModifier.RemoveGunAmmoStatModifier(this.gunAmmo);
            this.characterStatModifiersModifier.RemoveCharacterStatModifiersModifier(this.characterStatModifiers);
            this.gravityModifier.RemoveGravityModifier(this.gravity);
            this.OnOnDestroy();
		}
        public virtual void OnOnDestroy()
        {
            // derived effects should put any necessary cleanup here
        }
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }

    }
}

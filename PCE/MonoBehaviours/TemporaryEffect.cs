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
    public class TemporaryEffect : MonoBehaviour
    {

		private Player player;
        private CharacterStatModifiers characterStats;

        private bool storingOriginal = false;

        public void Awake()
        {
            this.player = this.gameObject.GetComponent<Player>();
            this.characterStats = this.player.data.stats;
            this.OnAwake();
        }
        public virtual void OnAwake()
        {

        }

        public void Start()
        {
            this.storingOriginal = !this.characterStats.GetAdditionalData().isEffected;
            this.characterStats.GetAdditionalData().isEffected = true;
            this.OnStart();
		}
        public virtual void OnStart()
        {

        }

        void FixedUpdate()
        {
            if (!this.characterStats.GetAdditionalData().isEffected)
            {
                this.player = this.gameObject.GetComponent<Player>();
                this.characterStats = this.player.data.stats;
                this.characterStats.GetAdditionalData().isEffected = true;
                this.storingOriginal = true;
            }

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
            if (this.storingOriginal)
            {
                this.OnOriginalDestroy();
                this.characterStats.GetAdditionalData().isEffected = false;
            }
            else
            {
                this.OnAncillaryDestroy();
            }
		}
        public virtual void OnOriginalDestroy()
        {

        }
        public virtual void OnAncillaryDestroy()
        {

        }
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }

    }
}

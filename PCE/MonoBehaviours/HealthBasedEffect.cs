using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;
using PCE.Extensions;

namespace PCE.MonoBehaviours
{
    public class HealthBasedEffect : ReversibleEffect
    {
        private bool active = false;
        private float percThreshMax = 0f;
        private float percThreshMin = 0f;
        private ColorEffect colorEffect = null;
        private Color color = Color.clear;

        public override void OnAwake()
        {
            base.SetLivesToEffect(int.MaxValue);
        }
        public override void OnStart()
        {
            base.applyImmediately = false;
            this.active = false;
        }
        public override void OnUpdate()
        {
            if (!this.active && this.HealthInRange())
            {
                this.ApplyColorEffect();
                base.ApplyModifiers();
                this.active = true;
            }
            else if (this.active && !this.HealthInRange())
            {
                if (this.colorEffect != null)
                {
                    UnityEngine.Object.Destroy(this.colorEffect);
                }
                base.ClearModifiers(false);
                this.active = false;
            }
        }
        private void ApplyColorEffect()
        {
            if (this.color != Color.clear)
            {
                this.colorEffect = base.player.gameObject.AddComponent<ColorEffect>();
                this.colorEffect.SetColor(this.color);
            }
        }
        private bool HealthInRange()
        {
            return (base.data.health <= base.data.maxHealth * this.percThreshMax && base.data.health >= base.data.maxHealth * this.percThreshMin);
        }
        public override void OnOnDisable()
        {
            // if the player is dead, clear the modifiers
            base.ClearModifiers(false);
            this.active = false;
        }
        public override void OnOnDestroy()
        {
        }
        public void SetPercThresholdMax(float perc)
        {
            this.percThreshMax = perc;
        }
        public void SetPercThresholdMin(float perc)
        {
            this.percThreshMin = perc;
        }
        public void SetColor(Color color)
        {
            this.color = color;
        }
    }

}

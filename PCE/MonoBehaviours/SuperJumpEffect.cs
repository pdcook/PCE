using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;
using PCE.Extensions;
using UnboundLib;

namespace PCE.MonoBehaviours
{
    public class SuperJumpEffect : ReversibleEffect // using ReversibleEffect just to handle cleanup/resetting
    {
        private bool active = false;
        ColorFlash colorFlash = null;
        private float multiplier = 1f;
        private float timeMultiplier = 0f;
        private readonly float chargeTime = 5f;
        private readonly float maxMultiplier = 5f;
        private readonly float minChargeTime = 0.5f;
        private float startTime = -1f;
        public override void OnAwake()
        {
            base.SetLivesToEffect(int.MaxValue);
        }
        public override void OnStart()
        {
            base.applyImmediately = false;
            this.active = false;
        }
        public override void OnOnEnable()
        {
            this.active = false;
        }
        public override void OnUpdate()
        {
            if (!base.data.isGrounded)
            {
                return;
            }

            if (!this.active && base.data.isGrounded && base.data.playerActions.Down.IsPressed)
            {
                this.active = true;
                this.ResetTimer();
            }

            if (this.active && base.data.isGrounded && base.data.playerActions.Down.IsPressed && this.HeldTime() >= this.minChargeTime)
            {
                this.multiplier = UnityEngine.Mathf.Clamp(((this.maxMultiplier - 1f) / (this.chargeTime / this.timeMultiplier - this.minChargeTime)) * (this.HeldTime() - this.minChargeTime) + 1f, 1f, this.maxMultiplier);
            }
            

            if (this.active && base.data.isGrounded && !base.data.playerActions.Down.IsPressed && this.HeldTime()>=this.minChargeTime)
            {
                base.data.jump.Jump(true, this.multiplier);
                base.data.currentJumps++;
                this.multiplier = 1f;
                this.active = false;
                this.ResetTimer();
            }
            else if (this.active && base.data.isGrounded && !base.data.playerActions.Down.IsPressed && this.HeldTime() < this.minChargeTime)
            {
                this.active = false;
                this.multiplier = 1f;
                this.ResetTimer();
            }

            if (this.active && base.data.isGrounded && base.data.playerActions.Down.IsPressed && this.HeldTime()>=this.minChargeTime)
            {
                this.colorFlash = base.player.gameObject.GetOrAddComponent<ColorFlash>();
                this.colorFlash.SetColor(Color.white);
                float perc = 1f - this.PercCharged();
                this.colorFlash.SetDelayBetweenFlashes(perc);
                this.colorFlash.SetDuration(perc);
                this.colorFlash.SetNumberOfFlashes(int.MaxValue);
            }
            else
            {
                if (this.colorFlash != null) { this.colorFlash.Destroy(); }
            }
        }
        public override void OnOnDisable()
        {
            // if the player is dead, this is no longer active
            this.active = false;
        }
        public override void OnOnDestroy()
        {
        }
        private void ResetTimer()
        {
            this.startTime = Time.time;
        }
        public void AddToTimeMultiplier(float add)
        {
            this.timeMultiplier += add;
            this.timeMultiplier = UnityEngine.Mathf.Clamp(this.timeMultiplier, 1f, this.chargeTime);
        }
        private float PercCharged()
        {
            return this.multiplier / this.maxMultiplier;
        }
        private void ResetMultiplier()
        {
            this.multiplier = 1f;
        }
        private float HeldTime()
        {
            return (Time.time - this.startTime);
        }
    }

}

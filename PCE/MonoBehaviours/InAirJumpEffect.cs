using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;
using PCE.Extensions;

namespace PCE.MonoBehaviours
{
    public class InAirJumpEffect : ReversibleEffect
    {
        private float
            interval = 0f,
            jump_mult = 1f;
        private float jumps = 0f;
        private float costPerJump = 0f;
        private float currentjumps = 0f;
        private bool continuous_trigger = false;
        private bool resetOnWallGrab = true;

        private readonly float minTimeFromGround = 0.1f; // minimum amount of time off the ground before this will engage

        public override void OnStart()
        {
            base.SetLivesToEffect(int.MaxValue);
        }
        public override void OnUpdate()
        {
            // reset if the player is on the ground
            if (base.data.isGrounded)
            {
                this.currentjumps = this.jumps;
                return;
            }
            // reset on wallgrab if desired
            else if (base.data.isWallGrab && this.resetOnWallGrab)
            {
                this.currentjumps = this.jumps;
                return;
            }
            // do not engage unless the player is out of normal jumps, and a bunch of other conditions are met
            else if (base.data.currentJumps <= 0 && this.currentjumps > 0f && base.data.sinceJump >= this.interval && base.data.sinceGrounded > this.minTimeFromGround && (base.data.playerActions.Jump.WasPressed || (this.continuous_trigger && base.data.playerActions.Jump.IsPressed)))
            {
                base.data.jump.Jump(true, this.jump_mult);
                this.currentjumps -= this.costPerJump;
            }
        }
        public override void OnOnDestroy()
        {
        }
        public void SetInterval(float interval)
        {
            this.interval = interval;
        }
        public void AddJumps(float add)
        {
            this.jumps += add;
        }
        public void SetJumpMult(float mult)
        {
            this.jump_mult = mult;
        }
        public float GetJumpMult()
        {
            return this.jump_mult;
        }
        public void SetContinuousTrigger(bool enabled)
        {
            this.continuous_trigger = enabled;
        }
        public bool GetContinuousTrigger()
        {
            return this.continuous_trigger;
        }
        public void SetResetOnWallGrab(bool enabled)
        {
            this.resetOnWallGrab = enabled;
        }
        public void SetCostPerJump(float cost)
        {
            this.costPerJump = cost;
        }
        public float GetCostPerJump()
        {
            return this.costPerJump;
        }
    }

}

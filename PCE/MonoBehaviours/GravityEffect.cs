using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;
using PCE.Extensions;

namespace PCE.MonoBehaviours
{
    public class GravityEffect : ReversibleEffect
    {
        private float
          startTime,
          duration = float.MaxValue,
          gravityForceMultiplier = 1f;
        public override void OnStart()
        {
            base.gravityModifier.gravityForce_mult = this.gravityForceMultiplier;
        }
        public override void OnUpdate()
        {
            // destroy this effect when time is up, the base class (ReversibleEffect) will handle reseting stats
            if (Time.time - this.startTime >= this.duration)
            {
                this.Destroy();
            }
            // destroy this if the effected player hits the damagebox
            if ((bool)Traverse.Create(base.player.data.GetAdditionalData().outOfBoundsHandler).Field("outOfBounds").GetValue())
            {
                this.Destroy();
            }
        }
        public override void OnOnDestroy()
        {
            base.player.data.sinceGrounded = 0f;
            base.player.data.sinceWallGrab = 0f;
        }
        public void ResetTimer()
        {
            this.startTime = Time.time;
        }
        public void SetDuration(float duration)
        {
            this.duration = duration;
        }
        public void SetGravityForceMultiplier(float mult)
        {
            this.gravityForceMultiplier = mult;
        }
    }

}

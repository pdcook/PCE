using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PCE.MonoBehaviours
{
    public class GravityEffect : ReversibleEffect
    {
        private Player playerToModify;
        private float
          startTime,
          duration = float.MaxValue,
          gravityForceMultiplier = 1f;
        public override void OnAwake()
        {
            this.playerToModify = gameObject.GetComponent<Player>();
        }
        public override void OnStart()
        {
            base.gravityModifier.gravityForce_mult = this.gravityForceMultiplier;
        }
        public override void OnUpdate()
        {
            // destroy this effect when time is up, the base class (ReversibleEffect) will handle reseting stats
            if (Time.time - this.startTime >= this.duration)
            {
                UnityEngine.Object.Destroy(this);
            }
        }
        public override void OnOnDestroy()
        {
            this.playerToModify.data.sinceGrounded = 0f;
            this.playerToModify.data.sinceWallGrab = 0f;
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

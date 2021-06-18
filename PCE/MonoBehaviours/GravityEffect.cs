using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PCE.MonoBehaviours
{
    public class GravityEffect : MonoBehaviour
    {
        private Player playerToModify;
        private Player playerWhoModifies = null;
        private Gravity gravityToModify;
        private float
          startTime,
          duration,
          gravityForceMultiplier = 1f,
          directGravityForce,
          origGravityForce;
        bool direct = false;


        void Awake()
        {
            this.playerToModify = gameObject.GetComponent<Player>();
            this.gravityToModify = gameObject.GetComponent<Gravity>();
            ResetTimer();
        }

        void Start()
        {
            this.origGravityForce = this.gravityToModify.gravityForce;
            if (direct)
            {
                this.gravityToModify.gravityForce = this.directGravityForce;
            }
            else
            {
                this.gravityToModify.gravityForce *= this.gravityForceMultiplier;
            }
        }

        void Update()
        {
            if (Time.time - this.startTime >= this.duration)
            {
                UnityEngine.Object.Destroy(this);
            }
        }
        public void OnDestroy()
        {
            this.playerToModify.data.sinceGrounded = 0f;
            this.playerToModify.data.sinceWallGrab = 0f;
            this.gravityToModify.gravityForce = this.origGravityForce;
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }
        public void SetPlayerWhoModifies(Player owner)
        {
            this.playerWhoModifies = owner;
        }
        public void ResetTimer()
        {
            startTime = Time.time;
        }
        public void SetDuration(float duration)
        {
            this.duration = duration;
        }
        public void SetGravityForceMultiplier(float mult)
        {
            this.gravityForceMultiplier = mult;
        }

        public void SetDirectGravityForce(float directGravityForce)
        {
            this.directGravityForce = directGravityForce;
            this.direct = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PCE.MonoBehaviours
{
    public class DiscombobulateEffect : MonoBehaviour
    {
        private Player playerToModify;
        private Player playerWhoModifies = null;
        private CharacterStatModifiers charStatsToModify;
        private float
          startTime,
          duration,
          movementspeedMultiplier = 1f,
          origMovementSpeed;

        void Awake()
        {
            this.playerToModify = gameObject.GetComponent<Player>();
            this.charStatsToModify = gameObject.GetComponent<CharacterStatModifiers>();
            ResetTimer();
        }

        void Start()
        {
            this.origMovementSpeed = this.charStatsToModify.movementSpeed;
            this.charStatsToModify.movementSpeed *= this.movementspeedMultiplier;

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
            this.charStatsToModify.movementSpeed = this.origMovementSpeed;
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
        public void SetMovementSpeedMultiplier(float mult)
        {
            this.movementspeedMultiplier = mult;
        }
    }
}

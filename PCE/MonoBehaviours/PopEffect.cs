using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;

namespace PCE.MonoBehaviours
{

    public class PopEffect : MonoBehaviour
    {
        private float period;
        private float spacing;
		private Player player;
        private float startTime;
        private float currentDuration;

        private readonly int maxAttemps = 100;

        private readonly System.Random rng = new System.Random();

        void Awake()
        {
            this.player = this.gameObject.GetComponent<Player>();
        }

        void Start()
        {
            this.ResetTimer();
            this.GetNewDuration();

        }

        void Update()
        {
            
            if (Time.time >= this.startTime + this.currentDuration)
            {
                int i = 0;
                Player otherPlayer = PlayerManager.instance.players[rng.Next(0, PlayerManager.instance.players.Count)];

                while (otherPlayer.playerID == this.player.playerID && i < this.maxAttemps)
                {
                    otherPlayer = PlayerManager.instance.players[rng.Next(0, PlayerManager.instance.players.Count)];
                    i++;
                }

                float rangle = (float)(rng.NextDouble() * 2 * Math.PI);
                float rradius = spacing * (float)rng.NextGaussianDouble();

                this.player.transform.position = otherPlayer.transform.position + new Vector3(rradius * (float)Math.Cos(rangle), rradius * (float)Math.Sin(rangle), 0f);

                this.ResetTimer();
                this.GetNewDuration();
            }



        }
        public void OnDestroy()
        {
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }
        public void ResetTimer()
        {
            this.startTime = Time.time;
        }
        public void GetNewDuration()
        {
            this.currentDuration = this.period*(float)rng.NextDouble()+this.period/2;
        }

        public void SetSpacing(float spacing)
        {
            this.spacing = spacing;
        }
        public void SetPeriod(float period)
        {
            this.period = period;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HarmonyLib;
using PCE.Extensions;

namespace PCE.MonoBehaviours
{
    public class AntSquishEffect : MonoBehaviour
    {
        private Player playerToModify;
        private CharacterStatModifiers charStatsToModify;


        void Awake()
        {
            this.playerToModify = gameObject.GetComponent<Player>();
            this.charStatsToModify = gameObject.GetComponent<CharacterStatModifiers>();
        }

        void Start()
        {

        }

        void Update()
        {
            // if the Ant player hasn't been squished in the minimum amount of time, and is currently alive, check for squish
            if (PlayerStatus.PlayerAliveAndSimulated(this.playerToModify) && Time.time >= this.timeOfLastSquish + this.minTimeBetweenSquishes)
            {
                List<Player> enemyPlayers = PlayerManager.instance.players.Where(player => PlayerStatus.PlayerAliveAndSimulated(player) && (player.teamID != this.playerToModify.teamID)).ToList();

                Vector2 displacement;

                foreach (Player enemyPlayer in enemyPlayers)
                {
                    // get the displacement vector from the Ant player to the enemy player, only if the enemy is at least 1.1x the mass of the Ant player
                    float mass = (float)Traverse.Create(this.playerToModify.data.playerVel).Field("mass").GetValue();
                    float enemy_mass = (float)Traverse.Create(enemyPlayer.data.playerVel).Field("mass").GetValue();

                    if ( enemy_mass >= this.minMassFactor * mass)
                    {
                        displacement = enemyPlayer.transform.position - this.playerToModify.transform.position;
                        if (displacement.magnitude <= this.range && Vector2.Angle(Vector2.up, displacement) <= Math.Abs(this.angleThreshold / 2))
                        {
                            // if the enemy player is both within range and within the specified angle above the player, then squish the Ant player
                            //float damage = this.damagePerc * (enemy_mass / mass) * this.playerToModify.data.maxHealth;
                            float damage = this.playerToModify.data.maxHealth * 2f; // instakill player

                            this.playerToModify.data.healthHandler.TakeDamage(new Vector2(0, -1*damage), this.playerToModify.transform.position, Color.red, null, enemyPlayer, true, false);
                            // reset the time since last squish and return
                            this.ResetTimer();
                            return;
                        }
                    }

                }
            }

        }
        public void OnDestroy()
        {
        }
        public void ResetTimer()
        {
            this.timeOfLastSquish = Time.time;
        }
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }
        public void SetDamagePerc(float perc)
        {
            this.damagePerc = perc;
        }
        public void IncreaseDamagePerc(float inc)
        {
            this.damagePerc += inc;
            this.damagePerc = Math.Min(this.damagePerc, 0.75f);
        }

        private float
          damagePerc = 0.0f,
          timeOfLastSquish = -1f;
        private readonly float range = 1.5f;
        private readonly float angleThreshold = 30f;
        private readonly float minTimeBetweenSquishes = 0.5f;
        private readonly float minMassFactor = 1.1f;

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using PCE.Extensions;
using PCE.MonoBehaviours;
using UnboundLib;

namespace PCE.RoundsEffects
{
    // heal a percent of the damage done by the bullet of a friendly player, however, status effects still apply
    public class FriendlyBulletsDealtDamageEffect : DealtDamageEffect
    {
        public float multiplier = 1f;
        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer = null)
        {
            
            if (selfDamage || (damagedPlayer != null && damagedPlayer.teamID == this.gameObject.GetComponent<Player>().teamID))
            {
                damagedPlayer.data.healthHandler.Heal(damage.magnitude * (1f - this.multiplier));
            }
        }


    }
}

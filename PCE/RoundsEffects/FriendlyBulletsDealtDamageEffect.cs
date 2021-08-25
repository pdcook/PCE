using UnityEngine;
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
                if (damagedPlayer.data.health - damage.magnitude <= 0f)
                {
                    damagedPlayer.data.healthHandler.Heal(damage.magnitude * (1f - this.multiplier));
                }
                else
                {
                    Unbound.Instance.ExecuteAfterFrames(2, () => damagedPlayer.data.healthHandler.Heal(damage.magnitude * (1f - this.multiplier)));
                }
            }
        }


    }
}

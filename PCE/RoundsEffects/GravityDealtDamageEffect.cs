using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using PCE.Extensions;
using PCE.MonoBehaviours;
using UnboundLib;
using System.Reflection;

namespace PCE.RoundsEffects
{
    public class GravityDealtDamageEffect : DealtDamageEffect
    {
        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer = null)
        {
            GravityEffect thisGravityEffect = damagedPlayer.gameObject.GetOrAddComponent<GravityEffect>();
            thisGravityEffect.SetDuration(this.GetComponent<CharacterStatModifiers>().GetAdditionalData().gravityDurationOnDoDamage);
            thisGravityEffect.SetGravityForceMultiplier(this.GetComponent<CharacterStatModifiers>().GetAdditionalData().gravityMultiplierOnDoDamage);
            thisGravityEffect.ResetTimer();

            // if this inflicts negative gravity, kick the player off the ground
            if (damagedPlayer.data.playerVel != null && this.GetComponent<CharacterStatModifiers>().GetAdditionalData().gravityMultiplierOnDoDamage < 0f)
            {
                typeof(PlayerVelocity).InvokeMember("AddForce",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, damagedPlayer.data.playerVel, new object[] { new Vector2(0f, 100f) });
                
            }
        }
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }

    }
}

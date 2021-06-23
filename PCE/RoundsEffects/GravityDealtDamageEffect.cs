using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using PCE.Extensions;
using PCE.MonoBehaviours;
using UnboundLib;

namespace PCE.RoundsEffects
{
    public class GravityDealtDamageEffect : DealtDamageEffect
    {
        private Player player;
        private CharacterStatModifiers characterStat;
        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer = null)
        {
            GravityEffect thisGravityEffect = damagedPlayer.gameObject.AddComponent<GravityEffect>();
            thisGravityEffect.SetDuration(this.GetComponent<CharacterStatModifiers>().GetAdditionalData().gravityDurationOnDoDamage);
            thisGravityEffect.SetGravityForceMultiplier(this.GetComponent<CharacterStatModifiers>().GetAdditionalData().gravityMultiplierOnDoDamage);
            thisGravityEffect.ResetTimer();
        }
        public void SetPlayer(Player player)
        {
            this.player = player;
            this.characterStat = player.GetComponent<CharacterStatModifiers>();
        }
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }

    }
}

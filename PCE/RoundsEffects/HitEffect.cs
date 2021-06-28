using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using PCE.Extensions;
using PCE.MonoBehaviours;
using UnboundLib;

namespace PCE.RoundsEffects
{
    // this is distinct from WasDealtDamageEffect since it will not trigger on DamageOverTime
    public abstract class HitEffect : MonoBehaviour
    {
        public abstract void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer = null);

        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }

    }
}

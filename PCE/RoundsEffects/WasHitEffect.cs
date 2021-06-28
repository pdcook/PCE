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
    public abstract class WasHitEffect : MonoBehaviour
    {
        public abstract void WasDealtDamage(Vector2 damage, bool selfDamage);

        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }

    }
}

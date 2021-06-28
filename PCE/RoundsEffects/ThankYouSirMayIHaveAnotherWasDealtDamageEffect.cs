using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using PCE.Extensions;
using PCE.MonoBehaviours;
using UnboundLib;

namespace PCE.RoundsEffects
{
    public class ThankYouSirMayIHaveAnotherWasDealtDamageEffect : WasHitEffect // do not trigger on DamageOverTime
    {
        public override void WasDealtDamage(Vector2 damage, bool selfDamage)
        {
            if (!selfDamage && this.gameObject.GetComponent<Player>().data.lastSourceOfDamage != null)
            {
                ReversibleEffect reversibleEffect = this.gameObject.GetComponent<Player>().gameObject.AddComponent<ReversibleEffect>();
                reversibleEffect.gunAmmoStatModifier.maxAmmo_add = this.gameObject.GetComponent<Player>().data.stats.GetAdditionalData().thankyousirmayihaveanother;
            }
        }
    }
}

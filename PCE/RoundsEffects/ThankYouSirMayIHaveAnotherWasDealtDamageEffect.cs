using ModdingUtils.MonoBehaviours;
using ModdingUtils.RoundsEffects;
using UnityEngine;
using PCE.Extensions;

namespace PCE.RoundsEffects
{
    public class ThankYouSirMayIHaveAnotherWasDealtDamageEffect : WasHitEffect // do not trigger on DamageOverTime
    {
        public override void WasDealtDamage(Vector2 damage, bool selfDamage)
        {
            if (!selfDamage && this.gameObject.GetComponent<Player>().data.lastSourceOfDamage != null && this.gameObject.GetComponent<Player>().GetComponent<Holding>().holdable.GetComponent<Gun>().GetComponentInChildren<GunAmmo>().maxAmmo < 99)
            {
                ReversibleEffect reversibleEffect = this.gameObject.GetComponent<Player>().gameObject.AddComponent<ReversibleEffect>();
                reversibleEffect.gunAmmoStatModifier.maxAmmo_add = this.gameObject.GetComponent<Player>().data.stats.GetAdditionalData().thankyousirmayihaveanother;
            }
        }
    }
}

using UnityEngine;
using HarmonyLib;
using ModdingUtils.MonoBehaviours;
using PCE.Extensions;
using UnboundLib;

namespace PCE.MonoBehaviours
{
    public class MulliganEffect : ReversibleEffect
    {
        ColorFlash colorFlash = null;
        public override void OnStart()
        {
            base.SetLivesToEffect(int.MaxValue);
        }
        public void UseMulligan()
        {
            // if there are no mulligans left, just return
            if (base.characterStatModifiers.GetAdditionalData().remainingMulligans <= 0)
            {
                return;
            }

            // force the player to block (for free)
            base.block.CallDoBlock(true, true, BlockTrigger.BlockTriggerType.Default);

            // stop DoT effects
            ((DamageOverTime)Traverse.Create(base.health).Field("dot").GetValue()).StopAllCoroutines();
            this.colorFlash = base.player.gameObject.GetOrAddComponent<ColorFlash>();
            this.colorFlash.SetNumberOfFlashes(1);
            this.colorFlash.SetDuration(0.25f);
            this.colorFlash.SetDelayBetweenFlashes(0.25f);
            this.colorFlash.SetColorMax(Color.white);
            this.colorFlash.SetColorMin(Color.white);

            // use up a single mulligan
            base.characterStatModifiers.GetAdditionalData().remainingMulligans--;
        }
        public override void OnOnDestroy()
        {
        }
    }

}

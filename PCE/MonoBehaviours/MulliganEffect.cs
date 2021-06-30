using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;
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
            base.characterStatModifiers.GetAdditionalData().mulligan = true;
        }
        public override void OnOnEnable()
        {
            base.characterStatModifiers.GetAdditionalData().mulligan = true;
        }
        public override void OnUpdate()
        {
            // if the player has 1f health left, remove the mulligan
            if (base.characterStatModifiers.GetAdditionalData().mulligan && base.data.health == 1f)
            {
                base.characterStatModifiers.GetAdditionalData().mulligan = false;
                this.colorFlash = base.player.gameObject.GetOrAddComponent<ColorFlash>();
                this.colorFlash.SetNumberOfFlashes(1);
                this.colorFlash.SetDuration(0.25f);
                this.colorFlash.SetDelayBetweenFlashes(0.25f);
                this.colorFlash.SetColorMax(Color.white);
                this.colorFlash.SetColorMin(Color.white);
            }
            else if (base.data.dead)
            {
                base.characterStatModifiers.GetAdditionalData().mulligan = true;
            }
        }
        public override void OnOnDisable()
        {
            base.characterStatModifiers.GetAdditionalData().mulligan = true;
        }
        public override void OnOnDestroy()
        {
        }
    }

}

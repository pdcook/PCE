using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HarmonyLib;
using PCE.MonoBehaviours;
using PCE.RoundsEffects;
using UnityEngine;
using PCE.Extensions;
using UnboundLib;


namespace PCE.Patches
{
    // patch to reset cards and effects
    [Serializable]
    [HarmonyPatch(typeof(CardToggleMenuHandler), "AddCardToggle")]
    class UnboundPatchAddCardToggle
    {
        private static bool Prefix(CardInfo info, bool isModded)
        {
            if (info.GetAdditionalData().hide)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

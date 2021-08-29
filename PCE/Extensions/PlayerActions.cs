using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using UnboundLib;
using InControl;

namespace PCE.Extensions
{
    [Serializable]
    public class PlayerActionsAdditionalData
    {
        public PlayerAction switchWeapon;


        public PlayerActionsAdditionalData()
        {
            switchWeapon = null;
        }
    }
    public static class PlayerActionsExtension
    {
        public static readonly ConditionalWeakTable<PlayerActions, PlayerActionsAdditionalData> data =
            new ConditionalWeakTable<PlayerActions, PlayerActionsAdditionalData>();

        public static PlayerActionsAdditionalData GetAdditionalData(this PlayerActions playerActions)
        {
            return data.GetOrCreateValue(playerActions);
        }

        public static void AddData(this PlayerActions playerActions, PlayerActionsAdditionalData value)
        {
            try
            {
                data.Add(playerActions, value);
            }
            catch (Exception) { }
        }
    }
}

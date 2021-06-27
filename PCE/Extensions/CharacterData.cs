using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using UnboundLib;

namespace PCE.Extensions
{
    // ADD FIELDS TO CHARACTERDATA
    [Serializable]
    public class CharacterDataAdditionalData
    {
        public OutOfBoundsHandler outOfBoundsHandler;

        public CharacterDataAdditionalData()
        {
            outOfBoundsHandler = null;
        }
    }
    public static class CharacterDataExtension
    {
        public static readonly ConditionalWeakTable<CharacterData, CharacterDataAdditionalData> data =
            new ConditionalWeakTable<CharacterData, CharacterDataAdditionalData>();

        public static CharacterDataAdditionalData GetAdditionalData(this CharacterData characterData)
        {
            return data.GetOrCreateValue(characterData);
        }

        public static void AddData(this CharacterData characterData, CharacterDataAdditionalData value)
        {
            try
            {
                data.Add(characterData, value);
            }
            catch (Exception) { }
        }
    }
    // get outOfBounds handler assigned to this player
    [HarmonyPatch(typeof(OutOfBoundsHandler), "Start")]
    class OutOfBoundsHandlerPatchStart
    {
        private static void Postfix(OutOfBoundsHandler __instance)
        {
            if (((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).GetAdditionalData().outOfBoundsHandler == null)
            {
                OutOfBoundsHandler[] ooBs = UnityEngine.GameObject.FindObjectsOfType<OutOfBoundsHandler>();
                foreach (OutOfBoundsHandler ooB in ooBs)
                {
                    if (((CharacterData)Traverse.Create(ooB).Field("data").GetValue()).player.playerID == ((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).player.playerID)
                    {
                        ((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).GetAdditionalData().outOfBoundsHandler = ooB;
                        return;
                    }
                }
            }
        }
    }
    
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace PCE.Extensions
{
    // ADD FIELDS TO BLOCK
    [Serializable]
    public class BlockAdditionalData
    {
        public float discombobulateRange;
        public float discombobulateDuration;
        public float timeOfLastSuccessfulBlock;


        public BlockAdditionalData()
        {
            discombobulateRange = 0f;
            discombobulateDuration = 0f;
            timeOfLastSuccessfulBlock = -100f;
        }
    }
    public static class BlockExtension
    {
        public static readonly ConditionalWeakTable<Block, BlockAdditionalData> data =
            new ConditionalWeakTable<Block, BlockAdditionalData>();

        public static BlockAdditionalData GetAdditionalData(this Block block)
        {
            return data.GetOrCreateValue(block);
        }

        public static void AddData(this Block block, BlockAdditionalData value)
        {
            try
            {
                data.Add(block, value);
            }
            catch (Exception) { }
        }
    }
    // reset additional block fields when ResetStats is called
    [HarmonyPatch(typeof(Block), "ResetStats")]
    class BlockPatchResetStats
    {
        private static void Prefix(Block __instance)
        {

            __instance.GetAdditionalData().discombobulateRange = 0f;
            __instance.GetAdditionalData().discombobulateDuration = 0f;
            __instance.GetAdditionalData().timeOfLastSuccessfulBlock = -100f;


        }
    }
}

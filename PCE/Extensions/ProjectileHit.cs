using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using UnboundLib;

namespace PCE.Extensions
{
    // ADD FIELDS TO PROJECTILEHIT
    [Serializable]
    public class ProjectileHitAdditionalData
    {

        public float startTime;
        public float inactiveDelay;

        public ProjectileHitAdditionalData()
        {
            startTime = -1f;
            inactiveDelay = 0f;
        }
    }
    public static class ProjectileHitExtension
    {
        public static readonly ConditionalWeakTable<ProjectileHit, ProjectileHitAdditionalData> data =
            new ConditionalWeakTable<ProjectileHit, ProjectileHitAdditionalData>();

        public static ProjectileHitAdditionalData GetAdditionalData(this ProjectileHit gun)
        {
            return data.GetOrCreateValue(gun);
        }

        public static void AddData(this ProjectileHit gun, ProjectileHitAdditionalData value)
        {
            try
            {
                data.Add(gun, value);
            }
            catch (Exception) { }
        }
    }
}

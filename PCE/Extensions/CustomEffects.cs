using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using System.Reflection;
using HarmonyLib;
using PCE.MonoBehaviours;
using PCE.RoundsEffects;

namespace PCE.Extensions
{
    public static class CustomEffects
    {
        public static void ResetAllTimers(GameObject gameObject)
        {
            CounterReversibleEffect[] counterReversibleEffects = gameObject.GetComponents<CounterReversibleEffect>();
            foreach (CounterReversibleEffect counterReversibleEffect in counterReversibleEffects) { if (counterReversibleEffect != null) { counterReversibleEffect.Reset(); } }
        }
        public static void ClearAllReversibleEffects(GameObject gameObject)
        {
            ReversibleEffect[] reversibleEffects = gameObject.GetComponents<ReversibleEffect>();
            foreach (ReversibleEffect reversibleEffect in reversibleEffects) { if (reversibleEffect != null) { reversibleEffect.ClearModifiers(); } }
        }
        public static void DestroyAllEffects(GameObject gameObject)
        {
            CustomEffects.DestroyAllAppliedEffects(gameObject);
            CustomEffects.DestroyAllDamageEfects(gameObject);
            CustomEffects.DestroyAllColorEffects(gameObject);
        }
        public static void DestroyAllColorEffects(GameObject gameObject)
        {
            CustomEffects.DestroyEffects<ColorEffect>(gameObject);
            CustomEffects.DestroyEffects<ColorFlash>(gameObject);
            CustomEffects.DestroyEffects<GunColorEffect>(gameObject);
            CustomEffects.DestroyEffects<ColorEffectBase>(gameObject);
            CustomEffects.DestroyEffects<GunColorEffectBase>(gameObject);
        }
        public static void DestroyAllReversibleEffects(GameObject gameObject)
        {
            ReversibleEffect[] reversibleEffects = gameObject.GetComponents<ReversibleEffect>();
            foreach (ReversibleEffect reversibleEffect in reversibleEffects) { if (reversibleEffect != null) { reversibleEffect.Destroy(); } }
        }
        public static void DestroyAllAppliedEffects(GameObject gameObject)
        {
            GravityEffect[] gravityEffects = gameObject.GetComponents<GravityEffect>();
            foreach (GravityEffect gravityEffect in gravityEffects) { if (gravityEffect != null) { gravityEffect.Destroy(); } }
            AntSquishEffect[] antSquishEffects = gameObject.GetComponents<AntSquishEffect>();
            foreach (AntSquishEffect antSquishEffect in antSquishEffects) { if (antSquishEffect != null) { antSquishEffect.Destroy(); } }
            DiscombobulateEffect[] discombobulateEffects = gameObject.GetComponents<DiscombobulateEffect>();
            foreach (DiscombobulateEffect discombobulateEffect in discombobulateEffects) { if (discombobulateEffect != null) { discombobulateEffect.Destroy(); } }
            DemonicPossessionEffect[] demonicPossessionEffects = gameObject.GetComponents<DemonicPossessionEffect>();
            foreach (DemonicPossessionEffect demonicPossessionEffect in demonicPossessionEffects) { if (demonicPossessionEffect != null) { demonicPossessionEffect.Destroy(); } }
            InConeEffect[] inConeEffects = gameObject.GetComponents<InConeEffect>();
            foreach (InConeEffect inConeEffect in inConeEffects) { if (inConeEffect != null) { inConeEffect.Destroy(); } }
            ColorFlash[] colorFlashs = gameObject.GetComponents<ColorFlash>();
            foreach (ColorFlash colorFlash in colorFlashs) { if (colorFlash != null) { colorFlash.Destroy(); } }
            ColorEffect[] colorEffects = gameObject.GetComponents<ColorEffect>();
            foreach (ColorEffect colorEffect in colorEffects) { if (colorEffect != null) { colorEffect.Destroy(); } }
            ColorEffectBase[] colorEffectBases = gameObject.GetComponents<ColorEffectBase>();
            foreach (ColorEffectBase colorEffectBase in colorEffectBases) { if (colorEffectBase != null) { colorEffectBase.Destroy(); } }
            GunColorEffect[] gunColorEffects = gameObject.GetComponents<GunColorEffect>();
            foreach (GunColorEffect gunColorEffect in gunColorEffects) { if (gunColorEffect != null) { gunColorEffect.Destroy(); } }
            GunColorEffectBase[] gunColorEffectBases = gameObject.GetComponents<GunColorEffectBase>();
            foreach (GunColorEffectBase gunColorEffectBase in gunColorEffectBases) { if (gunColorEffectBase != null) { gunColorEffectBase.Destroy(); } }
            ReversibleEffect[] reversibleEffects = gameObject.GetComponents<ReversibleEffect>();
            foreach (ReversibleEffect reversibleEffect in reversibleEffects) { if (reversibleEffect != null) { reversibleEffect.Destroy(); } }
            SpawnBulletsEffect[] spawnBulletsEffects = gameObject.GetComponents<SpawnBulletsEffect>();
            foreach (SpawnBulletsEffect spawnBulletsEffect in spawnBulletsEffects) { if (spawnBulletsEffect != null) { spawnBulletsEffect.Destroy(); } }
        }
        public static void ClearReversibleEffects(GameObject gameObject)
        {
            ReversibleEffect[] reversibleEffects = gameObject.GetComponents<ReversibleEffect>();
            foreach (ReversibleEffect reversibleEffect in reversibleEffects) { if (reversibleEffect != null) { reversibleEffect.Destroy(); } }
        }
        public static void DestroyEffects<T>(GameObject gameObject) where T : MonoBehaviour
        {
            T[] effects = gameObject.GetComponents<T>();
            foreach (T effect in effects) { if (effect != null) { UnityEngine.GameObject.Destroy((MonoBehaviour)(object)effect); } }
        }

        public static void DestroyAllDamageEfects(GameObject gameObject)
        {
            GravityDealtDamageEffect[] gravityDealtDamageEffects = gameObject.GetComponents<GravityDealtDamageEffect>();
            foreach (GravityDealtDamageEffect gravityDealtDamageEffect in gravityDealtDamageEffects) { if (gravityDealtDamageEffect != null) { gravityDealtDamageEffect.Destroy(); } }
            ThankYouSirMayIHaveAnotherWasDealtDamageEffect[] thankYouSirMayIHaveAnotherWasDealtDamageEffects = gameObject.GetComponents<ThankYouSirMayIHaveAnotherWasDealtDamageEffect>();
            foreach (ThankYouSirMayIHaveAnotherWasDealtDamageEffect thankYouSirMayIHaveAnotherWasDealtDamageEffect in thankYouSirMayIHaveAnotherWasDealtDamageEffects) { if (thankYouSirMayIHaveAnotherWasDealtDamageEffect != null) { thankYouSirMayIHaveAnotherWasDealtDamageEffect.Destroy(); } }
        }
    }
}

using PCE.Cards;
using PCE.MonoBehaviours;
using UnityEngine;

namespace PCE.Extensions
{
    public static class CustomEffects
    {
        public static void DestroyAllEffects(GameObject gameObject)
        {
            DestroyAllAppliedEffects(gameObject);
        }
        public static void DestroyAllAppliedEffects(GameObject gameObject)
        {
            AntSquishEffect[] antSquishEffects = gameObject.GetComponents<AntSquishEffect>();
            foreach (AntSquishEffect antSquishEffect in antSquishEffects) { if (antSquishEffect != null) { antSquishEffect.Destroy(); } }
            DiscombobulateEffect[] discombobulateEffects = gameObject.GetComponents<DiscombobulateEffect>();
            foreach (DiscombobulateEffect discombobulateEffect in discombobulateEffects) { if (discombobulateEffect != null) { discombobulateEffect.Destroy(); } }
            DemonicPossessionEffect[] demonicPossessionEffects = gameObject.GetComponents<DemonicPossessionEffect>();
            foreach (DemonicPossessionEffect demonicPossessionEffect in demonicPossessionEffects) { if (demonicPossessionEffect != null) { demonicPossessionEffect.Destroy(); } }
            SpawnBulletsEffect[] spawnBulletsEffects = gameObject.GetComponents<SpawnBulletsEffect>();
            foreach (SpawnBulletsEffect spawnBulletsEffect in spawnBulletsEffects) { if (spawnBulletsEffect != null) { spawnBulletsEffect.Destroy(); } }
            KingMidasEffect[] kingMidasEffects = gameObject.GetComponents<KingMidasEffect>();
            foreach (KingMidasEffect kingMidasEffect in kingMidasEffects) { if (kingMidasEffect != null) { kingMidasEffect.Destroy(); } }
        }
        public static void DestroyAllRandomCardEffects(GameObject gameObject)
        {
            RandomCardEffect[] randomCardEffects = gameObject.GetComponents<RandomCardEffect>();
            foreach (RandomCardEffect randomCardEffect in randomCardEffects) { if (randomCardEffect != null) { UnityEngine.GameObject.Destroy(randomCardEffect); } }
        }
    }
}
using PCE.Cards;
using UnityEngine;

namespace PCE.Extensions
{
    public static class CustomEffects
    {
        public static void DestroyAllRandomCardEffects(GameObject gameObject)
        {
            RandomCardEffect[] randomCardEffects = gameObject.GetComponents<RandomCardEffect>();
            foreach (RandomCardEffect randomCardEffect in randomCardEffects) { if (randomCardEffect != null) { UnityEngine.GameObject.Destroy(randomCardEffect); } }
        }
    }
}
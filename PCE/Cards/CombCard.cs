using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using PCE.MonoBehaviours;
using System.Collections.Generic;
using System.Linq;

namespace PCE.Cards
{
    public class CombCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            List<ObjectsToSpawn> objectsToSpawn = gun.objectsToSpawn.ToList();
            ObjectsToSpawn comb = new ObjectsToSpawn { };
            comb.AddToProjectile = new GameObject("CombSpawner", typeof(CombSpawner));
            objectsToSpawn.Add(comb);

            gun.objectsToSpawn = objectsToSpawn.ToArray();

            gun.bulletDamageMultiplier *= 0.7f;
            gunAmmo.maxAmmo = UnityEngine.Mathf.RoundToInt(gunAmmo.maxAmmo / 3f);
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Comb";
        }
        protected override string GetDescription()
        {
            return "Fire multiple bullets side-by-side";
        }

        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bullets",
                    amount = "+2",
                    simepleAmount = CardInfoStat.SimpleAmount.Some
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Ammo",
                    amount = "1/3×",
                    simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Damage",
                    amount = "-30%",
                    simepleAmount = CardInfoStat.SimpleAmount.slightlyLower
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.FirepowerYellow;
        }

        public override string GetModName()
        {
            return "PCE";
        }

    }
}

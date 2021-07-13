using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using PCE.MonoBehaviours;

namespace PCE.Cards
{
    public class FragmentationCard : CustomCard
    {
        // not yet implemented
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            GameObject effect = new UnityEngine.GameObject("fragmentation");
            SpawnBulletsEffect spawnBullets = effect.AddComponent<SpawnBulletsEffect>();

            ObjectsToSpawn objectsToSpawn = new ObjectsToSpawn();
            objectsToSpawn.effect = effect;
            objectsToSpawn.spawnOn = ObjectsToSpawn.SpawnOn.notPlayer;
            objectsToSpawn.AddToProjectile = effect; // ???


        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Fragmentation";
        }
        protected override string GetDescription()
        {
            return "Bullets split into multiple bullets after hitting the ground.";
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
            return null;
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

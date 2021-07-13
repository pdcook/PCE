using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;

namespace PCE.Cards
{
    public class GhostBulletsCard : CustomCard
    {
        /*
         * bullets are invisible and go through walls
         */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.ignoreWalls = true;
            gun.bulletDamageMultiplier = 0.25f;
            gun.projectileColor = Color.clear;
            if (gun.destroyBulletAfter == 0f) { gun.destroyBulletAfter = 5f; }
            gun.unblockable = true;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Ghost Bullets";
        }
        protected override string GetDescription()
        {
            return "Bullets are invisible, go through walls, and penetrate shields";
        }

        protected override GameObject GetCardArt()
        {
            return PCE.ArtAssets.LoadAsset<GameObject>("C_GhostGun");
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                positive = true,
                stat = "Bullets",
                amount = "Invisible",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Damage",
                amount = "-75%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

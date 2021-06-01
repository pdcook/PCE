using System;
using BepInEx;
using UnboundLib.Cards;
using UnityEngine;
using PCE.Cards;

namespace PCE
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("pykess.rounds.plugins.pykesscardexpansion", "Pykess's Card Expansion (PCE)", "0.1.0.0")]
    [BepInProcess("Rounds.exe")]
    public class PCE : BaseUnityPlugin
    {
        private void Start()
        {
            CustomCard.BuildCard<LaserCard>();
            CustomCard.BuildCard<GhostGunCard>();
            CustomCard.BuildCard<TractorBeamCard>();
        }
        private const string ModId = "pykess.rounds.plugins.pykesscardexpansion";

        private const string ModName = "Pykess's Card Expansion (PCE)";
    }
}

namespace PCE.Cards
{
    public class LaserCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.soundDisableRayHitBulletSound = true;
            gun.forceSpecificShake = false;
            gun.recoilMuiltiplier = 0f;
            gun.spread = 0f;
            gun.multiplySpread = 0f;
            gun.size = 0f;
            gun.bursts = 50;
            gun.knockback *= 0.02f;
            gun.projectileSpeed *= 100f;
            gun.drag = 0f;
            gun.gravity = 0f;
            gun.timeBetweenBullets = 0.001f;
            if (gun.projectileColor.a > 0f)
            {
                gun.projectileColor = new Color(1f, 0f, 0f, 0.1f);
            }
            gun.multiplySpread = 0f;
            gun.shakeM = 0f;
            gun.bulletDamageMultiplier = 0.025f;
            gunAmmo.reloadTimeMultiplier *= 1.5f;
            gunAmmo.maxAmmo = 1;
            gun.destroyBulletAfter = 1f;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Laser";
        }
        protected override string GetDescription()
        {
            return "Light Amplification by Stimulated Emission of Radiation";
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
                amount = "Light",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Bullet Speed",
                amount = "+10000%",
                simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Damage",
                amount = "-97.5%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Reload Time",
                amount = "+50%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Ammo",
                amount = "1",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
    }
    public class GhostGunCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.ignoreWalls = true;
            gun.unblockable = true;
            gun.projectileColor = Color.clear;
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
            return "Bullets are invisible and go through walls and shields";
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
                amount = "Invisible",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
    }

    public class TractorBeamCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.knockback = -2f * Math.Abs(gun.knockback);

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Tractor Beam";
        }
        protected override string GetDescription()
        {
            return "Bullets pulls opponents toward you on contact";
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
                stat = "Knockback",
                amount = "Reverse",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
    }

}
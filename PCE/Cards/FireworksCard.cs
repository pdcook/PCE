using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using PCE.MonoBehaviours;
using PCE.Extensions;
using System.Linq;

namespace PCE.Cards
{
    public class FireworksCard : CustomCard
    {
        private static GameObject _FireworkSpawner = null;
        internal static GameObject FireworkSpawner
        {
            get 
            {
                if (_FireworkSpawner != null) { return FireworksCard._FireworkSpawner; }

                _FireworkSpawner = new GameObject("FireworkSpawner", typeof(FireworkSpawner));
                UnityEngine.GameObject.DontDestroyOnLoad(_FireworkSpawner);
                return _FireworkSpawner;
            }
            set { }
        }
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (gun.GetAdditionalData().fireworkProjectiles == 0)
            {
                ObjectsToSpawn fireworkObj = new ObjectsToSpawn() { };
                fireworkObj.AddToProjectile = FireworkSpawner;
                List<ObjectsToSpawn> objectsToSpawn = gun.objectsToSpawn.ToList();
                objectsToSpawn.Add(fireworkObj);
                gun.objectsToSpawn = objectsToSpawn.ToArray();
            }
            gun.GetAdditionalData().fireworkProjectiles += 3;

            gun.damage *= 1.15f;
            gun.projectileSpeed *= 1.15f;

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Fireworks";
        }
        protected override string GetDescription()
        {
            return "Bullets pop like fireworks when above the battlefield.";
        }

        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    positive = true,
                    stat = "Fireworks",
                    amount = "+3",
                    simepleAmount = CardInfoStat.SimpleAmount.Some
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Damage",
                    amount = "+15%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bullet Speed",
                    amount = "+15%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },

            };
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

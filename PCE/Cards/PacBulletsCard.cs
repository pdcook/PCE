using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using PCE.MonoBehaviours;
using Photon.Pun;
using System.Reflection;
using PCE.Extensions;
using System.Linq;
using PCE.RoundsEffects;
using PCE.Utils;

namespace PCE.Cards
{
    public class PacBulletsCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (gun.GetAdditionalData().wraps == 0)
            {
                ObjectsToSpawn fireworkObj = new ObjectsToSpawn() { };
                fireworkObj.AddToProjectile = new GameObject("PacBulletsSpawner", typeof(PacBulletSpawner));
                List<ObjectsToSpawn> objectsToSpawn = gun.objectsToSpawn.ToList();
                objectsToSpawn.Add(fireworkObj);
                gun.objectsToSpawn = objectsToSpawn.ToArray();
            }
            gun.GetAdditionalData().wraps += 3;
            gun.gravity *= 0.5f;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Pac-Bullets";
        }
        protected override string GetDescription()
        {
            return "Bullets wrap around from the edge of the screen.";
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
                    stat = "Bullet Warps",
                    amount = "+3",
                    simepleAmount = CardInfoStat.SimpleAmount.Some
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Bullet Gravity",
                    amount = "-50%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                }

            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }



}

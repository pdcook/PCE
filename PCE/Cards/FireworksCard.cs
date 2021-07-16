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
    public class FireworksCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (gun.GetAdditionalData().fireworkProjectiles == 0)
            {
                ObjectsToSpawn fireworkObj = new ObjectsToSpawn() { };
                fireworkObj.AddToProjectile = new GameObject("FireworkSpawner", typeof(FireworkSpawner));
                List<ObjectsToSpawn> objectsToSpawn = gun.objectsToSpawn.ToList();
                objectsToSpawn.Add(fireworkObj);
                gun.objectsToSpawn = objectsToSpawn.ToArray();
            }
            gun.GetAdditionalData().fireworkProjectiles += 3;

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
            return "Bullets that hit the top of the battlefield pop like fireworks.";
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

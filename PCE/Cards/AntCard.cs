using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using PCE.MonoBehaviours;

namespace PCE.Cards
{
    public class AntCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            data.maxHealth /= 2f;
            characterStats.sizeMultiplier /= 1.5f;
            gun.bulletDamageMultiplier *= 2f;
            gunAmmo.maxAmmo -= 2;

            if (Utils.Cards.instance.CountPlayerCardsWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, (card, p, g, ga, d, h, gr, b, c) => card.name == this.GetTitle()) == 0)
            {
                // only apply movementspeed buff and jump debuff if the player doesn't have any ant cards yet
                characterStats.movementSpeed *= 1.25f;
                characterStats.jump *= 0.85f;
            }

            AntSquishEffect thisAntSquishEffect = player.gameObject.GetOrAddComponent<AntSquishEffect>();

            //thisAntSquishEffect.IncreaseDamagePerc(0.25f);

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Ant";
        }
        protected override string GetDescription()
        {
            return "Halve in size; double in strength. Be careful not to get stepped on.";
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
                    stat = "Damage",
                    amount = "Double",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = true,
                    stat = "Size",
                    amount = "Half",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "HP",
                    amount = "Half",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    positive = false,
                    stat = "Ammo",
                    amount = "-2",
                    simepleAmount = CardInfoStat.SimpleAmount.lower
                }

            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.NatureBrown;
        }

        public override string GetModName()
        {
            return "PCE";
        }

    }
}

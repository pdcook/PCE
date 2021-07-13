using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;
using PCE;
using UnboundLib;
using PCE.Extensions;
using PCE.RoundsEffects;
using PCE.MonoBehaviours;


namespace PCE.Cards
{
    public class LowGroundCard : CustomCard
    {
        /*
        *  Get boosted stats when below an enemy player
        */

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            InConeEffect newEffect = player.gameObject.AddComponent<InConeEffect>();

            newEffect.SetCenterRay(new Vector2(0f, 1f));
            newEffect.SetColor(Color.green);
            newEffect.SetEffectFunc(this.statboost);
            newEffect.SetAngle(90f);

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Low Ground";
        }
        protected override string GetDescription()
        {
            return "Get increased stats when below an enemy player.\nWhen active:";
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
                amount = "+50%",
                simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Bullet Speed",
                amount = "+100%",
                simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Attack Speed",
                amount = "+50%",
                simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Ammo",
                amount = "+3",
                simepleAmount = CardInfoStat.SimpleAmount.Some
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Reload Speed",
                amount = "+25%",
                simepleAmount = CardInfoStat.SimpleAmount.Some
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Movement Speed",
                amount = "+50%",
                simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },

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
        public List<MonoBehaviour> statboost(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            List<MonoBehaviour> effects = new List<MonoBehaviour>();

            ReversibleEffect effect = player.gameObject.AddComponent<ReversibleEffect>();
            effect.gunStatModifier.projectielSimulatonSpeed_mult = 2f;
            effect.gunStatModifier.projectileSpeed_mult = 2f;
            effect.gunStatModifier.damage_mult = 1.5f;
            effect.gunStatModifier.projectileColor = Color.red;
            effect.gunStatModifier.attackSpeed_mult = 0.75f;

            effect.gunAmmoStatModifier.maxAmmo_add = 3;
            effect.gunAmmoStatModifier.reloadTimeMultiplier_mult = 0.75f;

            effect.characterStatModifiersModifier.movementSpeed_mult = 1.5f;

            effects.Add(effect);

            return effects;

        }

    }
}

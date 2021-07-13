using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using PCE.MonoBehaviours;

namespace PCE.Cards
{
    public class JetpackCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.movementSpeed *= 0.85f;
            InAirJumpEffect jumpEffect = player.gameObject.GetOrAddComponent<InAirJumpEffect>();
            jumpEffect.SetJumpMult(0.1f);
            jumpEffect.AddJumps(100);
            jumpEffect.SetCostPerJump(5);
            jumpEffect.SetContinuousTrigger(true);
            jumpEffect.SetResetOnWallGrab(false);
            jumpEffect.SetInterval(0.03f);
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Jetpack";
        }
        protected override string GetDescription()
        {
            return "A shiny new jetpack that actually works";
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
                stat = "New Jetpack",
                amount = "A",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Jetpack Fuel",
                amount = "+10",
                simepleAmount = CardInfoStat.SimpleAmount.Some
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Movement Speed",
                amount = "-15%",
                simepleAmount = CardInfoStat.SimpleAmount.lower
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

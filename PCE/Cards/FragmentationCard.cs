using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using PCE.Extensions;
using PCE.RoundsEffects;

namespace PCE.Cards
{
    public class FragmentationCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.GetAdditionalData().fragmentationProjectiles += 5;
            player.gameObject.GetOrAddComponent<FragmentationHitSurfaceEffect>();

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
            return "Bullets split into fragments on impact.";
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
                    stat = "Bullet Fragments",
                    amount = "+5",
                    simepleAmount = CardInfoStat.SimpleAmount.Some
                }
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

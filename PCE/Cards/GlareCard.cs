using System.Collections.Generic;
using ModdingUtils.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;
using PCE.Extensions;
using PCE.MonoBehaviours;


namespace PCE.Cards
{
    public class GlareCard : CustomCard
    {
        /*
        *  Slow enemies when you can see them
        */

        private Player player;
        private CharacterStatModifiers characterStats;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.GetAdditionalData().glare += 1f;

            InConeEffect newEffect = player.gameObject.AddComponent<InConeEffect>();

            newEffect.SetCenterRay(new Vector2(1f, 0f));
            newEffect.SetOtherColor(Color.black);
            newEffect.SetNeedsLineOfSight(true);
            newEffect.SetApplyToSelf(false);
            newEffect.SetApplyToOthers(true);
            newEffect.SetCheckEnemiesOnly(true);
            newEffect.SetOtherEffectFunc(this.glare);

            this.player = player;
            this.characterStats = characterStats;

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Glare";
        }
        protected override string GetDescription()
        {
            return "Enemies tremble in fear when you can see them\nWhen active:";
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
                stat = "Enemy Movement Speed",
                amount = "-15%",
                simepleAmount = CardInfoStat.SimpleAmount.slightlyLower
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Enemy Jump Height",
                amount = "-25%",
                simepleAmount = CardInfoStat.SimpleAmount.slightlyLower
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.EvilPurple;
        }
        public List<MonoBehaviour> glare(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            List<MonoBehaviour> effects = new List<MonoBehaviour>();

            ReversibleEffect effect = player.gameObject.AddComponent<ReversibleEffect>();

            float movementspeedReduction = 0.15f;
            float jumpheightReduction = 0.25f;

            effect.characterStatModifiersModifier.movementSpeed_mult = UnityEngine.Mathf.Pow(1f - movementspeedReduction, this.characterStats.GetAdditionalData().glare);
            effect.characterStatModifiersModifier.jump_mult = UnityEngine.Mathf.Pow(1f - jumpheightReduction, this.characterStats.GetAdditionalData().glare);

            effects.Add(effect);

            ShakeEffect shakeeffect = player.gameObject.AddComponent<ShakeEffect>();

            effects.Add(shakeeffect);

            return effects;

        }
        public override string GetModName()
        {
            return "PCE";
        }

    }
}

using UnboundLib.Cards;
using UnityEngine;
using ModdingUtils.Extensions;

namespace PCE.Cards
{
    public class RiskyGambleCard : CustomCard
    {
        /*
        *  An Uncommon card which gives the player two random Common cards
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.GetAdditionalData().canBeReassigned = false;
            cardInfo.categories = new CardCategory[] { CardChoiceSpawnUniqueCardPatch.CustomCategories.CustomCardCategories.instance.CardCategory("CardManipulation") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            CardInfo randomCard1 = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, this.condition);

            ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard1);
            ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard1);

            CardInfo randomCard2 = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, (card, p, g, ga, d, h, gr, b, s) => this.condition(card, p, g, ga, d, h, gr, b, s) && ModdingUtils.Utils.Cards.instance.CardDoesNotConflictWithCards(card, new CardInfo[] { randomCard1 }));

            ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard2);
            ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard2);

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Risky Gamble";
        }
        protected override string GetDescription()
        {
            return "Get <b>two</b> random <color=#ffffffff>Common</color> cards";
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
            return null;
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
        public bool condition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // do not allow duplicates of cards with allowMultiple == false
            // card rarity must be as desired
            // card cannot be another Gamble / Jackpot card
            // card cannot be from a blacklisted catagory of any other card

            return (card.rarity == CardInfo.Rarity.Common) && !card.cardName.Contains("Jackpot") && !card.cardName.Contains("Gamble");

        }
        public override string GetModName()
        {
            return "PCE";
        }

    }
}

using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;
using System.Linq;
using PCE.RoundsEffects;
using PCE.Extensions;

namespace PCE.Cards
{
    /*
     * Any bullets that hit you are added to your max ammo
     */
    public class ThankYouSirMayIHaveAnotherCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.GetAdditionalData().thankyousirmayihaveanother += 1;
            player.gameObject.GetOrAddComponent<ThankYouSirMayIHaveAnotherWasDealtDamageEffect>();

            data.maxHealth *= 1.5f;

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Thank You Sir, May I Have Another?";
        }
        protected override string GetDescription()
        {
            return "Pick up the bullets that have hit you";
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
                    stat = "HP",
                    amount = "+50%",
                    simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
        }
        public List<Player> GetAllEnemyPlayers(Player player)
        {
            return PlayerManager.instance.players.Where(enemyPlayer => enemyPlayer.teamID != player.teamID).ToList();
        }
        public override string GetModName()
        {
            return "PCE";
        }
    }
}

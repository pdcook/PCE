using UnboundLib.Cards;
using UnityEngine;
using Photon.Pun;
using System.Reflection;
using HarmonyLib;
using System.Linq;
using PCE.Extensions;
using System.Collections;
using UnboundLib.Networking;
using ModdingUtils.Extensions;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using UnboundLib.GameModes;

namespace PCE.Cards
{
    public class ShuffleCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            ModdingUtils.Extensions.CardInfoExtension.GetAdditionalData(cardInfo).canBeReassigned = false;
            cardInfo.categories = new CardCategory[] { CustomCardCategories.instance.CardCategory("NoPreGamePick") };
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            Extensions.CharacterStatModifiersExtension.GetAdditionalData(characterStats).shuffles += 1;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Shuffle";
        }
        protected override string GetDescription()
        {
            return "Draw new cards to choose from.";
        }

        protected override GameObject GetCardArt()
        {
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Shuffle");
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
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
        public override string GetModName()
        {
            return "PCE";
        }

        internal static IEnumerator ExtraPicks()
        {
            foreach (Player player in PlayerManager.instance.players.ToArray())
            {
                while (Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).shuffles > 0)
                {
                    Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).shuffles -= 1;
                    yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);
                    CardChoiceVisuals.instance.Show(Enumerable.Range(0, PlayerManager.instance.players.Count).Where(i => PlayerManager.instance.players[i].playerID == player.playerID).First(), true);
                    yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
                    yield return new WaitForSecondsRealtime(0.1f);
                    yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);
                    yield return new WaitForSecondsRealtime(0.1f);
                }
            }
            yield break;
        }
    }
}

using UnboundLib.Cards;
using UnboundLib;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System.Collections;
using Photon.Pun;

namespace PCE.Cards
{
    public class MurderCard : CustomCard
    {
        /*
        *  A rare card which kills ALL opposing players immediately at the start of the next round
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            ModdingUtils.Extensions.CardInfoExtension.GetAdditionalData(cardInfo).canBeReassigned = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

            if (GM_Test.instance != null && GM_Test.instance.gameObject.activeInHierarchy)
            {
                // are we in sandbox mode? if so, just kill the other players
                foreach (Player oppPlayer in PlayerManager.instance.players.Where(oP => oP.teamID != player.teamID))
                {
                    Unbound.Instance.ExecuteAfterSeconds(2f, delegate
                    {
                        typeof(HealthHandler).InvokeMember("RPCA_Die",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, oppPlayer.data.healthHandler,
                                    new object[] { new Vector2(0, 1) });

                    });
                }
            }
            else
            {
                // otherwise, let the onbattlestart hook handle it
                ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(characterStats).murder += 1;
            }

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Murder";
        }
        protected override string GetDescription()
        {
            return "Kill your opponents";
        }

        protected override GameObject GetCardArt()
        {
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Murder");
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override CardInfoStat[] GetStats()
        {
            return null;
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.EvilPurple;
        }
        public override string GetModName()
        {
            return "PCE";
        }

        internal static IEnumerator CommitMurders()
        {
            Player[] players = PlayerManager.instance.players.ToArray();
            for (int j = 0; j < players.Length; j++)
            {
                // commit any pending murders
                if (ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(players[j].data.stats).murder >= 1)
                {
                    ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(players[j].data.stats).murder--;

                    // kill ALL opposing players
                    foreach (Player oppPlayer in PlayerManager.instance.players.Where(player => player.teamID != players[j].teamID))
                    {
                        Unbound.Instance.ExecuteAfterSeconds(2f, delegate
                        {
                            oppPlayer.data.view.RPC("RPCA_Die", RpcTarget.All, new object[]
                            {
                                    new Vector2(0, 1)
                            });

                        });
                    }


                }
            }
            yield break;
        }
    }
}

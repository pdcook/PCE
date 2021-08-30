using System;
using PCE.Extensions;
using PCE.MonoBehaviours;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using System.Reflection;
using UnboundLib.Networking;

namespace PCE.Cards
{
    public class DiscombobulateCard : CustomCard
    {
        /*
        *  Blocking temporarily inverts nearby players' controls
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.cdAdd += 0.25f;
            block.GetAdditionalData().discombobulateRange += 5f;
            block.GetAdditionalData().discombobulateDuration += 2f;

            block.BlockAction = (Action<BlockTrigger.BlockTriggerType>)Delegate.Combine(block.BlockAction, new Action<BlockTrigger.BlockTriggerType>(this.GetDoBlockAction(player, block)));

        }
        public Action<BlockTrigger.BlockTriggerType> GetDoBlockAction(Player player, Block block)
        {
            return delegate (BlockTrigger.BlockTriggerType trigger)
            {
                if (trigger != BlockTrigger.BlockTriggerType.None)
                {
                    Vector2 pos = block.transform.position;
                    Player[] players = PlayerManager.instance.players.ToArray();

                    for (int i = 0; i < players.Length; i++)
                    {
                        // don't apply the effect to the player who activated it...
                        if (players[i].playerID == player.playerID) { continue; }

                        // apply to players within range, that are within line-of-sight
                        if (Vector2.Distance(pos, players[i].transform.position) < block.GetAdditionalData().discombobulateRange && PlayerManager.instance.CanSeePlayer(player.transform.position, players[i]).canSee)
                        {
                            NetworkingManager.RPC(typeof(DiscombobulateCard), "OnDiscombobulateActivate", new object[] { players[i].playerID, block.GetAdditionalData().discombobulateDuration });
                        }
                    }


                }
            };
        }
        
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Discombobulate";
        }
        protected override string GetDescription()
        {
            return "Blocking temporarily reverses nearby players' controls";
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
                positive = false,
                stat = "Block Cooldown",
                amount = "+0.25s",
                simepleAmount = CardInfoStat.SimpleAmount.aLittleBitOf
                },

            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
        public override string GetModName()
        {
            return "PCE";
        }
        [UnboundRPC]
        public static void OnDiscombobulateActivate(int playerID, float duration)
        {
            Player player = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithID",
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic,
                null, PlayerManager.instance, new object[] { playerID });

            DiscombobulateEffect thisDiscombobulateEffect = player.gameObject.AddComponent<DiscombobulateEffect>();
            thisDiscombobulateEffect.SetDuration(duration);
            thisDiscombobulateEffect.SetMovementSpeedMultiplier(-1f);
            Color yellow = Color.yellow;
            float brightness = 0.8f;
            yellow.r *= brightness;
            yellow.g *= brightness;
            yellow.b *= brightness;
            thisDiscombobulateEffect.SetColor(new Color(yellow.r, yellow.g, yellow.b, 1f));
            thisDiscombobulateEffect.ResetTimer();

        }

    }
}

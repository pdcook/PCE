using System;
using PCE.Extensions;
using PCE.MonoBehaviours;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using System.Reflection;
using UnboundLib.Networking;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnboundLib.Utils;

namespace PCE.Cards
{
    public class DiscombobulateCard : CustomCard
    {
        private static float rangePerCard = 5f;
        private static float durationPerCard = 2f;

        private static void makeHierarchyHidden(GameObject obj)
        {
            obj.gameObject.hideFlags = HideFlags.HideAndDontSave;
            foreach (Transform child in obj.transform)
            {
                makeHierarchyHidden(child.gameObject);
            }
        }
        private class DiscombobSpawner : MonoBehaviour
        {
            void Start()
            {
                if (!(this.gameObject.GetComponent<SpawnedAttack>().spawner != null))
                {
                    return;
                }

                this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * (this.gameObject.GetComponent<SpawnedAttack>().spawner.GetComponent<Block>().GetAdditionalData().discombobulateRange / DiscombobulateCard.rangePerCard);

                this.gameObject.AddComponent<RemoveAfterSeconds>().seconds = 5f;
                this.gameObject.transform.GetChild(1).GetComponent<LineEffect>().SetFieldValue("inited", false);
                typeof(LineEffect).InvokeMember("Init",
                    BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic,
                    null, this.gameObject.transform.GetChild(1).GetComponent<LineEffect>(), new object[] { });
                this.gameObject.transform.GetChild(1).GetComponent<LineEffect>().radius = (DiscombobulateCard.rangePerCard-1.4f);
                this.gameObject.transform.GetChild(1).GetComponent<LineEffect>().SetFieldValue("startWidth", 0.5f);
                this.gameObject.transform.GetChild(1).GetComponent<LineEffect>().Play();

            }
        }

        private static GameObject discombobVisual_ = null;
        private static GameObject discombobVisual
        {
            get
            {
                if (discombobVisual_ != null) { return discombobVisual_; }
                else
                {
                    List<CardInfo> activecards = ((ObservableCollection<CardInfo>)typeof(CardManager).GetField("activeCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).ToList();
                    List<CardInfo> inactivecards = (List<CardInfo>)typeof(CardManager).GetField("inactiveCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                    List<CardInfo> allcards = activecards.Concat(inactivecards).ToList();
                    GameObject E_Overpower = allcards.Where(card => card.cardName.ToLower() == "overpower").First().GetComponent<CharacterStatModifiers>().AddObjectToPlayer.GetComponent<SpawnObjects>().objectToSpawn[0];
                    discombobVisual_ = UnityEngine.GameObject.Instantiate(E_Overpower, new Vector3(0,100000f, 0f), Quaternion.identity);
                    discombobVisual_.name = "E_Discombobulate";
                    DontDestroyOnLoad(discombobVisual_);
                    foreach (ParticleSystem parts in discombobVisual_.GetComponentsInChildren<ParticleSystem>())
                    {
                        parts.startColor = Color.green;
                    }
                    discombobVisual_.transform.GetChild(1).GetComponent<LineEffect>().colorOverTime.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.green, 0f) };
                    UnityEngine.GameObject.Destroy(discombobVisual_.transform.GetChild(2).gameObject);
                    discombobVisual_.transform.GetChild(1).GetComponent<LineEffect>().offsetMultiplier = 0f;
                    discombobVisual_.transform.GetChild(1).GetComponent<LineEffect>().playOnAwake = true;
                    UnityEngine.GameObject.Destroy(discombobVisual_.GetComponent<FollowPlayer>());
                    discombobVisual_.GetComponent<DelayEvent>().time = 0f;
                    UnityEngine.GameObject.Destroy(discombobVisual_.GetComponent<SoundImplementation.SoundUnityEventPlayer>());
                    UnityEngine.GameObject.Destroy(discombobVisual_.GetComponent<Explosion>());
                    UnityEngine.GameObject.Destroy(discombobVisual_.GetComponent<Explosion_Overpower>());
                    //UnityEngine.GameObject.Destroy(discombobVisual_.GetComponent<SpawnedAttack>());
                    UnityEngine.GameObject.Destroy(discombobVisual_.GetComponent<RemoveAfterSeconds>());
                    discombobVisual_.AddComponent<DiscombobSpawner>();
                    return discombobVisual_;
                }
            }
            set { }
        }
        /*
        *  Blocking temporarily inverts nearby players' controls
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (block.GetAdditionalData().discombobulateRange == 0f)
            {
                block.BlockAction = (Action<BlockTrigger.BlockTriggerType>)Delegate.Combine(block.BlockAction, new Action<BlockTrigger.BlockTriggerType>(this.GetDoBlockAction(player, block)));
                block.objectsToSpawn.Add(discombobVisual);
            }

            block.cdAdd += 0.25f;
            block.GetAdditionalData().discombobulateRange += DiscombobulateCard.rangePerCard;
            block.GetAdditionalData().discombobulateDuration += DiscombobulateCard.durationPerCard;
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

            DiscombobulateEffect thisDiscombobulateEffect = player.gameObject.GetOrAddComponent<DiscombobulateEffect>();
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

using BepInEx; // requires BepInEx.dll and BepInEx.Harmony.dll
using UnboundLib; // requires UnboundLib.dll
using UnboundLib.Cards; // " "
using UnityEngine; // requires UnityEngine.dll, UnityEngine.CoreModule.dll, and UnityEngine.AssetBundleModule.dll
using HarmonyLib; // requires 0Harmony.dll
using System.Collections;
using Photon.Pun;
using Jotunn.Utils;
using UnboundLib.GameModes;
using PCE.Cards;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnboundLib.Networking;
using UnboundLib.Utils;
using CustomEffects = PCE.Extensions.CustomEffects;

// requires Assembly-CSharp.dll
// requires MMHOOK-Assembly-CSharp.dll

namespace PCE
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)] // necessary for most modding stuff here
    [BepInDependency("pykess.rounds.plugins.playerjumppatch", BepInDependency.DependencyFlags.HardDependency)] // fixes multiple jumps
    [BepInDependency("pykess.rounds.plugins.legraycasterspatch", BepInDependency.DependencyFlags.HardDependency)] // fixes physics for small players
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)] // fixes allowMultiple and blacklistedCategories
    [BepInDependency("pykess.rounds.plugins.gununblockablepatch", BepInDependency.DependencyFlags.HardDependency)] // fixes gun.unblockable
    [BepInDependency("pykess.rounds.plugins.temporarystatspatch", BepInDependency.DependencyFlags.HardDependency)] // fixes Taste Of Blood, Pristine Perserverence, and Chase when combined with cards from PCE
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)] // utilities for cards and cardbars
    [BepInPlugin(ModId, ModName, "0.2.5.0")]
    [BepInProcess("Rounds.exe")]
    public class PCE : BaseUnityPlugin
    {
        private void Awake()
        {
            new Harmony(ModId).PatchAll();
        }
        private void Start()
        {
            // register credits with unbound
            Unbound.RegisterCredits(ModName, new string[] { "Pykess" }, new string[] { "github", "Buy me a coffee" }, new string[] { "https://github.com/pdcook/PCE", "https://www.buymeacoffee.com/Pykess" });

            PCE.ArtAssets = AssetUtils.LoadAssetBundleFromResources("pceassetbundle", typeof(PCE).Assembly);
            if (PCE.ArtAssets == null)
            {
                UnityEngine.Debug.Log("Failed to load PCE art asset bundle");
            }

            // build all cards

            CustomCard.BuildCard<LaserCard>();
            CustomCard.BuildCard<GhostBulletsCard>();
            CustomCard.BuildCard<TractorBeamCard>();
            CustomCard.BuildCard<MoonShoesCard>();
            CustomCard.BuildCard<OldJetpackCard>(); // requires pykess.rounds.plugins.playerjumppatch
            CustomCard.BuildCard<FlipCard>();
            CustomCard.BuildCard<GroundedCard>();
            CustomCard.BuildCard<AntCard>(); // requires pykess.rounds.plugins.legraycasterspatch
            CustomCard.BuildCard<MurderCard>();
            CustomCard.BuildCard<JackpotCard>();
            CustomCard.BuildCard<SmallJackpotCard>();
            CustomCard.BuildCard<GambleCard>();
            CustomCard.BuildCard<RiskyGambleCard>();
            CustomCard.BuildCard<CloseQuartersCard>();
            CustomCard.BuildCard<DiscombobulateCard>();
            CustomCard.BuildCard<DemonicPossessionCard>();
            CustomCard.BuildCard<LowGroundCard>();
            CustomCard.BuildCard<ThankYouSirMayIHaveAnotherCard>();
            CustomCard.BuildCard<GlareCard>();
            CustomCard.BuildCard<JetpackCard>(); // requires pykess.rounds.plugins.playerjumppatch
            CustomCard.BuildCard<LastStandCard>();
            CustomCard.BuildCard<MulliganCard>();
            CustomCard.BuildCard<SuperJumpCard>();
            CustomCard.BuildCard<FriendlyBulletsCard>();
            CustomCard.BuildCard<KingMidasCard>();
            CustomCard.BuildCard<StraightShotCard>();
            CustomCard.BuildCard<SuperBallCard>();
            CustomCard.BuildCard<RetreatCard>();
            CustomCard.BuildCard<FragmentationCard>();
            CustomCard.BuildCard<FireworksCard>();
            CustomCard.BuildCard<ShuffleCard>();
            CustomCard.BuildCard<PacBulletsCard>();
            CustomCard.BuildCard<PacPlayerCard>();
            CustomCard.BuildCard<PiercingBulletsCard>();
            CustomCard.BuildCard<PunchingBulletsCard>();

            CustomCard.BuildCard<SurvivalistICard>();
            CustomCard.BuildCard<SurvivalistIICard>();
            CustomCard.BuildCard<SurvivalistIIICard>();
            CustomCard.BuildCard<SurvivalistIVCard>();
            CustomCard.BuildCard<SurvivalistVCard>(card => { SurvivalistVCard.self = card; ModdingUtils.Utils.Cards.instance.AddHiddenCard(SurvivalistVCard.self); });

            CustomCard.BuildCard<PacifistICard>();
            CustomCard.BuildCard<PacifistIICard>();
            CustomCard.BuildCard<PacifistIIICard>();
            CustomCard.BuildCard<PacifistIVCard>();
            CustomCard.BuildCard<PacifistVCard>(card => { PacifistVCard.self = card; ModdingUtils.Utils.Cards.instance.AddHiddenCard(PacifistVCard.self); });

            CustomCard.BuildCard<WildcardICard>();
            CustomCard.BuildCard<WildcardIICard>();
            CustomCard.BuildCard<WildcardIIICard>();
            CustomCard.BuildCard<WildcardIVCard>();
            CustomCard.BuildCard<WildcardVCard>(card => { WildcardVCard.self = card; ModdingUtils.Utils.Cards.instance.AddHiddenCard(WildcardVCard.self); });

            CustomCard.BuildCard<MasochistICard>();
            CustomCard.BuildCard<MasochistIICard>();
            CustomCard.BuildCard<MasochistIIICard>();
            CustomCard.BuildCard<MasochistIVCard>();
            CustomCard.BuildCard<MasochistVCard>(card => { MasochistVCard.self = card; ModdingUtils.Utils.Cards.instance.AddHiddenCard(MasochistVCard.self); });

            CustomCard.BuildCard<RandomCommonCard>(Cards.RandomCard.callback);
            CustomCard.BuildCard<RandomUncommonCard>(Cards.RandomCard.callback);
            CustomCard.BuildCard<RandomRareCard>(Cards.RandomCard.callback);

            GameModeManager.AddHook(GameModeHooks.HookBattleStart, (gm) => this.CommitMurders());
            GameModeManager.AddHook(GameModeHooks.HookBattleStart, (gm) => this.ResetEffectsBetweenBattles());

            GameModeManager.AddHook(GameModeHooks.HookPointEnd, (gm) => this.ResetEffectsBetweenBattles());

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, (gm) => this.ExtraPicks());

            GameModeManager.AddHook(GameModeHooks.HookPointStart, (gm) => this.RandomCard());

            GameModeManager.AddHook(GameModeHooks.HookGameStart, (gm) => this.ClearRandomCards());
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, (gm) => this.ClearRandomCards());
        }

        private IEnumerator CommitMurders()
        {
            Player[] players = PlayerManager.instance.players.ToArray();
            for (int j = 0; j < players.Length; j++)
            {
                // commit any pending murders
                if (ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(players[j].data.stats).murder >= 1)
                {
                    ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(players[j].data.stats).murder--;

                    int i = 0;
                    Player oppPlayer = PlayerManager.instance.players[MurderCard.rng.Next(0, PlayerManager.instance.players.Count)];

                    // while the other player is on the same team as the current player
                    while ((oppPlayer.teamID == players[j].teamID) && i < 1000)
                    {
                        oppPlayer = PlayerManager.instance.players[MurderCard.rng.Next(0, PlayerManager.instance.players.Count)];
                        i++;
                    }
                    Unbound.Instance.ExecuteAfterSeconds(2f, delegate
                    {
                        oppPlayer.data.view.RPC("RPCA_Die", RpcTarget.All, new object[]
                        {
                                    new Vector2(0, 1)
                        });

                    });
                }
            }
            yield break;
        }
        private IEnumerator ResetEffectsBetweenBattles()
        {
            foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (gameObject.name == "LaserTrail(Clone)")
                {
                    UnityEngine.GameObject.Destroy(gameObject);
                }
            }
            yield break;
        }

        private IEnumerator ExtraPicks()
        {
            foreach(Player player in PlayerManager.instance.players.ToArray())
            {
                while (Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).shuffles > 0)
                {
                    Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).shuffles -= 1;
                    yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);
                    CardChoiceVisuals.instance.Show(Enumerable.Range(0,PlayerManager.instance.players.Count).Where(i => PlayerManager.instance.players[i].playerID == player.playerID).First(), true);
                    yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
                    yield return new WaitForSecondsRealtime(0.1f);
                    yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);
                    yield return new WaitForSecondsRealtime(0.1f);
                }
            }
            yield break;
        }

        private IEnumerator RandomCard()
        {
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(PCE), nameof(RPCA_RandomInProgress), new object[] { true });
            }
            yield return new WaitForSecondsRealtime(0.5f);
            if (!PhotonNetwork.OfflineMode && !PhotonNetwork.IsMasterClient)
            {
                while (PCE.randomInProgress)
                {
                    yield return null;
                }
                yield break;
            }
            //if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            //{
            //    NetworkingManager.RPC(typeof(PCE), nameof(RPCA_DisablePlayers), new object[] { });
            //}

            Dictionary<Player, List<CardInfo>> cardsToShow = new Dictionary<Player, List<CardInfo>>();
            
            foreach (Player player in PlayerManager.instance.players.ToArray())
            {

                if (player.GetComponent<RandomCardEffect>() != null && player.GetComponent<RandomCardEffect>().indeces.Count > 0)
                {
                    List<int> indeces = new List<int>(player.GetComponent<RandomCardEffect>().indeces);
                    List<int> invalidInd = new List<int>() { };
                    List<string> twoLetterCodes = new List<string>() { };
                    List<CardInfo> newCards = new List<CardInfo>() { };
                    foreach (int idx in indeces)
                    {
                        string twoLetterCode = player.GetComponent<RandomCardEffect>().twoLetterCode;
                        CardInfo card = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, null, null, null, null, null, null, null, (card, player, g, ga, d, h, gr, b, s) => ModdingUtils.Utils.Cards.instance.CardDoesNotConflictWithCards(card, newCards.ToArray()) && card.rarity == player.data.currentCards[idx].rarity && ModdingUtils.Extensions.CardInfoExtension.GetAdditionalData(card).canBeReassigned && !Extensions.CardInfoExtension.GetAdditionalData(card).isRandom && ModdingUtils.Utils.Cards.instance.CardIsNotBlacklisted(card, new CardCategory[] { CardChoiceSpawnUniqueCardPatch.CustomCategories.CustomCardCategories.instance.CardCategory("CardManipulation") }));
                        if (card == null)
                        {
                            // if there is no valid card, then try drawing from the list of all cards (inactive + active) but still make sure it is compatible
                            CardInfo[] allCards = ((ObservableCollection<CardInfo>)typeof(CardManager).GetField("activeCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).ToList().Concat((List<CardInfo>)typeof(CardManager).GetField("inactiveCards", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)).ToArray();
                            card = ModdingUtils.Utils.Cards.instance.DrawRandomCardWithCondition(allCards, player, null, null, null, null, null, null, null, (card, player, g, ga, d, h, gr, b, s) => ModdingUtils.Utils.Cards.instance.CardDoesNotConflictWithCards(card, newCards.ToArray()) && card.rarity == player.data.currentCards[idx].rarity && ModdingUtils.Extensions.CardInfoExtension.GetAdditionalData(card).canBeReassigned && !Extensions.CardInfoExtension.GetAdditionalData(card).isRandom && ModdingUtils.Utils.Cards.instance.CardIsNotBlacklisted(card, new CardCategory[] { CardChoiceSpawnUniqueCardPatch.CustomCategories.CustomCardCategories.instance.CardCategory("CardManipulation") }));

                            if (card == null)
                            {
                                // if there is STILL no valid card, then this index is invalid
                                invalidInd.Add(idx);
                                continue;
                            }
                        }
                        twoLetterCodes.Add(twoLetterCode);
                        newCards.Add(card);
                    }
                    indeces = indeces.Except(invalidInd).ToList();
                    cardsToShow[player] = newCards;
                    if (indeces.Count == 0)
                    {
                        continue;
                    }
                    yield return ModdingUtils.Utils.Cards.instance.ReplaceCards(player, indeces.ToArray(), newCards.ToArray(), twoLetterCodes.ToArray());
                }
            }
            yield return new WaitForSecondsRealtime(0.5f);
            float numCardsToShow = 0f;
            foreach (Player player in cardsToShow.Keys)
            {
                numCardsToShow += cardsToShow[player].Count;
            }
            numCardsToShow = UnityEngine.Mathf.Clamp(numCardsToShow, 2f, float.MaxValue);
            foreach (Player player in cardsToShow.Keys)
            {
                if (cardsToShow[player].Count == 0) { continue; }
                yield return ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, cardsToShow[player].ToArray(), 2f/numCardsToShow);
            }
            //if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            //{
            //    NetworkingManager.RPC(typeof(PCE), nameof(RPCA_EnablePlayers), new object[] { });
            //}
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC(typeof(PCE), nameof(RPCA_RandomInProgress), new object[] { false });
            }
            yield return new WaitForSecondsRealtime(0.1f);
            yield break;
        }
        [UnboundRPC]
        private static void RPCA_RandomInProgress(bool prog)
        {
            PCE.randomInProgress = prog;
        }
        [UnboundRPC]
        private static void RPCA_DisablePlayers()
        {
            foreach (Player player in PlayerManager.instance.players.ToArray())
            {
                if (player.GetComponent<GeneralInput>() != null) { player.GetComponent<GeneralInput>().enabled = false; }
            }
        }
        [UnboundRPC]
        private static void RPCA_EnablePlayers()
        {
            foreach (Player player in PlayerManager.instance.players.ToArray())
            {
                if (player.GetComponent<GeneralInput>() != null) { player.GetComponent<GeneralInput>().enabled = true; }
            }
        }

        private IEnumerator ClearRandomCards()
        {
            foreach (Player player in PlayerManager.instance.players)
            {
                CustomEffects.DestroyAllRandomCardEffects(player.gameObject);
            }
            yield break;
        }

        private static bool randomInProgress = false;

        private const string ModId = "pykess.rounds.plugins.pykesscardexpansion";

        private const string ModName = "Pykess's Card Expansion (PCE)";
        internal static AssetBundle ArtAssets;
    }
}

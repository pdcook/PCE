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
using PCE.Extensions;
using PCE.MonoBehaviours;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
// requires Assembly-CSharp.dll
// requires MMHOOK-Assembly-CSharp.dll

namespace PCE
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)] // necessary for most modding stuff here
    [BepInDependency("pykess.rounds.plugins.playerjumppatch", BepInDependency.DependencyFlags.HardDependency)] // fixes multiple jumps
    [BepInDependency("pykess.rounds.plugins.legraycasterspatch", BepInDependency.DependencyFlags.HardDependency)] // fixes physics for small players
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)] // fixes allowMultiple and blacklistedCategories
    [BepInDependency("pykess.rounds.plugins.gununblockablepatch", BepInDependency.DependencyFlags.HardDependency)] // fixes gun.unblockable
    [BepInPlugin(ModId, ModName, "0.2.1.2")]
    [BepInProcess("Rounds.exe")]
    public class PCE : BaseUnityPlugin
    {
        private void Awake()
        {
            new Harmony(ModId).PatchAll();
        }
        private void Start()
        {

            PCE.ArtAssets = AssetUtils.LoadAssetBundleFromResources("pceassetbundle", typeof(PCE).Assembly);
            if (PCE.ArtAssets == null)
            {
                global::Debug.Log("Failed to load PCE art asset bundle");
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

            CustomCard.BuildCard<SurvivalistICard>();
            CustomCard.BuildCard<SurvivalistIICard>();
            CustomCard.BuildCard<SurvivalistIIICard>();
            CustomCard.BuildCard<SurvivalistIVCard>();

            CustomCard.BuildCard<PacifistICard>();
            CustomCard.BuildCard<PacifistIICard>();
            CustomCard.BuildCard<PacifistIIICard>();
            CustomCard.BuildCard<PacifistIVCard>();

            CustomCard.BuildCard<WildcardICard>();
            CustomCard.BuildCard<WildcardIICard>();
            CustomCard.BuildCard<WildcardIIICard>();
            CustomCard.BuildCard<WildcardIVCard>();


            GameModeManager.AddHook(GameModeHooks.HookBattleStart, (gm) => this.CommitMurders());
            GameModeManager.AddHook(GameModeHooks.HookBattleStart, (gm) => this.ResetEffectsBetweenBattles());
            GameModeManager.AddHook(GameModeHooks.HookBattleStart, (gm) => this.ResetTimers()); // I sure hope this doesn't have unintended side effects...

            GameModeManager.AddHook(GameModeHooks.HookPointEnd, (gm) => this.ResetEffectsBetweenBattles());
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, (gm) => this.ResetTimers());

            GameModeManager.AddHook(GameModeHooks.HookPickEnd, (gm) => this.ExtraPicks());

        }

        private IEnumerator CommitMurders()
        {
            Player[] players = PlayerManager.instance.players.ToArray();
            for (int j = 0; j < players.Length; j++)
            {
                // commit any pending murders
                if (players[j].data.stats.GetAdditionalData().murder >= 1)
                {
                    players[j].data.stats.GetAdditionalData().murder--;
                    Player oppPlayer = PlayerManager.instance.GetOtherPlayer(players[j]);
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
            Player[] players = PlayerManager.instance.players.ToArray();
            for (int j = 0; j < players.Length; j++)
            {
                CustomEffects.ClearAllReversibleEffects(players[j].gameObject);
            }
            foreach (GameObject gameObject in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (gameObject.name == "LaserTrail(Clone)")
                {
                    UnityEngine.GameObject.Destroy(gameObject);
                }
            }
            yield break;
        }

        private IEnumerator ResetTimers()
        {
            Player[] players = PlayerManager.instance.players.ToArray();
            for (int j = 0; j < players.Length; j++)
            {
                CustomEffects.ResetAllTimers(players[j].gameObject);
            }
            yield break;
        }

        private IEnumerator ExtraPicks()
        {
            foreach(Player player in PlayerManager.instance.players.ToArray())
            {
                if (player.GetComponent<Shuffle>() != null)
                {
                    yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);
                    yield return player.GetComponent<Shuffle>().WaitForSyncUp();
                    CardChoiceVisuals.instance.Show(Enumerable.Range(0,PlayerManager.instance.players.Count).Where(i => PlayerManager.instance.players[i].playerID == player.playerID).First(), true);
                    yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
                    Destroy(player.GetComponent<Shuffle>());
                    Extensions.Cards.instance.RemoveCardsFromPlayer(player, Extensions.Cards.instance.GetPlayerCardsWithCondition(player, null,null,null,null,null,null,null,(card,player,g,ga,d,h,gr,b,s)=>card.name=="Shuffle"), Extensions.Cards.SelectionType.All);
                    yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);
                    yield return new WaitForSecondsRealtime(0.1f);

                }
            }
            yield break;
        }

        private const string ModId = "pykess.rounds.plugins.pykesscardexpansion";

        private const string ModName = "Pykess's Card Expansion (PCE)";
        internal static AssetBundle ArtAssets;
    }
}

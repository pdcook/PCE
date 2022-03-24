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
    [BepInDependency("com.dk.rounds.plugins.zerogpatch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, "2.7.8")]
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
            CustomCard.BuildCard<CombCard>();

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

            GameModeManager.AddHook(GameModeHooks.HookBattleStart, (gm) => MurderCard.CommitMurders());
            GameModeManager.AddHook(GameModeHooks.HookBattleStart, (gm) => LaserCard.RemoveLasersBetweenBattles());

            GameModeManager.AddHook(GameModeHooks.HookPointEnd, (gm) => LaserCard.RemoveLasersBetweenBattles());

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, (gm) => ShuffleCard.ExtraPicks());

            GameModeManager.AddHook(GameModeHooks.HookPointStart, (gm) => RandomCard.Go());

            GameModeManager.AddHook(GameModeHooks.HookGameStart, (gm) => RandomCard.ClearRandomCards());
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, (gm) => RandomCard.ClearRandomCards());
        }

        private const string ModId = "pykess.rounds.plugins.pykesscardexpansion";

        private const string ModName = "Pykess's Card Expansion (PCE)";
        internal static AssetBundle ArtAssets;
    }
}

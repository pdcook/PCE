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
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.playerjumppatch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.legraycasterspatch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, "0.1.8.2")]
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
            CustomCard.BuildCard<GhostGunCard>();
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

            GameModeManager.AddHook(GameModeHooks.HookBattleStart, (gm) => this.CommitMurders());
            GameModeManager.AddHook(GameModeHooks.HookBattleStart, (gm) => this.ResetEffectsBetweenBattles());
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
            yield break;
        }

        private const string ModId = "pykess.rounds.plugins.pykesscardexpansion";

        private const string ModName = "Pykess's Card Expansion (PCE)";
        internal static AssetBundle ArtAssets;
    }
}
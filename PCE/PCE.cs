using System;
using BepInEx; // requires BepInEx.dll and BepInEx.Harmony.dll
using UnboundLib; // requires UnboundLib.dll
using UnboundLib.Cards; // " "
using UnityEngine; // requires UnityEngine.dll, UnityEngine.CoreModule.dll, and UnityEngine.AssetBundleModule.dll
using PCE.Cards;
using System.IO;
using HarmonyLib; // requires 0Harmony.dll
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Collections;
// requires Assembly-CSharp.dll
// requires MMHOOK-Assembly-CSharp.dll

namespace PCE
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("pykess.rounds.plugins.pykesscardexpansion", "Pykess's Card Expansion (PCE)", "0.1.1.0")]
    [BepInProcess("Rounds.exe")]
    public class PCE : BaseUnityPlugin
    {
        private void Awake()
        {
            new Harmony("pykess.rounds.plugins.pykesscardexpansion").PatchAll();
        }
        private void Start()
        {

            PCE.ArtAssets = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "pceAssetBundle"));
            if (PCE.ArtAssets == null)
            {
                global::Debug.Log("Failed to load PCE art asset bundle");
            }
            CustomCard.BuildCard<LaserCard>();
            CustomCard.BuildCard<GhostGunCard>();
            CustomCard.BuildCard<TractorBeamCard>();
            CustomCard.BuildCard<MoonShoesCard>();
            //CustomCard.BuildCard<JetpackCard>(); // cannot set number of jumps greater than 1 but less than infinity
            CustomCard.BuildCard<FlipCard>();
            CustomCard.BuildCard<GroundedCard>();
            //CustomCard.BuildCard<AntCard>(); // small players have physics issues
            CustomCard.BuildCard<MurderCard>();
            CustomCard.BuildCard<JackpotCard>();
            CustomCard.BuildCard<SmallJackpotCard>();
            CustomCard.BuildCard<GambleCard>();
            CustomCard.BuildCard<RiskyGambleCard>();
            CustomCard.BuildCard<CloseQuartersCard>();


        }
        private const string ModId = "pykess.rounds.plugins.pykesscardexpansion";

        private const string ModName = "Pykess's Card Expansion (PCE)";
        internal static AssetBundle ArtAssets;
    }


    // ADD FIELDS TO CHARACTERSTATMODIFIERS
    [Serializable]
    public class CharacterStatModifiersAdditionalData
    {
        public float gravityMultiplierOnDoDamage;
        public float gravityDurationOnDoDamage;
        public float timeOfLastWasDealtDamage;
        public bool defaultGravity;
        public float defaultGravityForce;
        public float defaultGravityExponent;
        public int murder;


        public CharacterStatModifiersAdditionalData()
        {
            gravityMultiplierOnDoDamage = 1f;
            gravityDurationOnDoDamage = 0f;
            timeOfLastWasDealtDamage = -1f;
            defaultGravity = true;
            murder = 0;
        }
    }
    public static class CharacterStatModifiersExtension
    {
        public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData> data =
            new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData>();

        public static CharacterStatModifiersAdditionalData GetAdditionalData(this CharacterStatModifiers characterstats)
        {
            return data.GetOrCreateValue(characterstats);
        }

        public static void AddData(this CharacterStatModifiers characterstats, CharacterStatModifiersAdditionalData value)
        {
            try
            {
                data.Add(characterstats, value);
            }
            catch (Exception) { }
        }
    }
    [HarmonyPatch(typeof(CharacterStatModifiers), "Start")]
    class CharacterStatModifiersPatchStart
    {
        private static void Postfix(CharacterStatModifiers __instance)
        {
            __instance.GetAdditionalData().defaultGravityExponent = __instance.GetComponent<Player>().GetComponent<Gravity>().exponent;
            __instance.GetAdditionalData().defaultGravityForce = __instance.GetComponent<Player>().GetComponent<Gravity>().gravityForce;
            
        }
    }
    [HarmonyPatch(typeof(CharacterStatModifiers), "DealtDamage")]
    class CharacterStatModifiersPatchDealtDamage
    {
        private static void Prefix(CharacterStatModifiers __instance, Vector2 damage, bool selfDamage, Player damagedPlayer = null)
        {

            if (__instance.GetAdditionalData().gravityMultiplierOnDoDamage != 1f)
            {
                
                
                CharacterStatModifiers opstats = damagedPlayer.GetComponent<CharacterStatModifiers>();
                if (opstats.GetAdditionalData().defaultGravity)
                {
                    Gravity opgrav = damagedPlayer.GetComponent<Gravity>();
                    float orig_gravityForce = opgrav.gravityForce;
                    opstats.GetAdditionalData().defaultGravity = false;
                    opgrav.gravityForce *= __instance.GetAdditionalData().gravityMultiplierOnDoDamage;

                    Unbound.Instance.ExecuteAfterSeconds(__instance.GetAdditionalData().gravityDurationOnDoDamage, delegate
                    {
                        if (!opstats.GetAdditionalData().defaultGravity && Time.realtimeSinceStartup > __instance.GetAdditionalData().timeOfLastWasDealtDamage)
                        {
                            opgrav.gravityForce = orig_gravityForce;
                            opstats.GetAdditionalData().defaultGravity = true;
                        }
                    });
                }

            }
        }
    }
    [HarmonyPatch(typeof(CharacterStatModifiers), "WasDealtDamage")]
    class CharacterStatModifiersPatchWasDealtDamage
    {
        private static void Prefix(CharacterStatModifiers __instance, Vector2 damage, bool selfDamage)
        {
            float curTime = Time.realtimeSinceStartup;
            if (curTime > __instance.GetAdditionalData().timeOfLastWasDealtDamage)
            {
                __instance.GetAdditionalData().timeOfLastWasDealtDamage = Time.realtimeSinceStartup;
            }
        }
    }
    // reset player gravity effects when ResetStats is called
    [HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
    class CharacterStatModifiersPatchResetStats
    {
        private static void Prefix(CharacterStatModifiers __instance)
        {

            __instance.GetAdditionalData().gravityMultiplierOnDoDamage = 1f;
            __instance.GetAdditionalData().gravityDurationOnDoDamage = 0f;
            __instance.GetAdditionalData().timeOfLastWasDealtDamage = -1f;
            __instance.GetAdditionalData().defaultGravity = true;
            __instance.GetAdditionalData().murder = 0;
            Gravity gravity = __instance.GetComponent<Gravity>();
            gravity.gravityForce = __instance.GetAdditionalData().defaultGravityForce;
            gravity.exponent = __instance.GetAdditionalData().defaultGravityExponent;


        }
    }

    // ADD FIELDS TO GUN
    public class GunAdditionalData
    {
        public float minDistanceMultiplier;



        public GunAdditionalData()
        {
            minDistanceMultiplier = 1f;
        }
    }
    public static class GunExtension
    {
        public static readonly ConditionalWeakTable<Gun, GunAdditionalData> data =
            new ConditionalWeakTable<Gun, GunAdditionalData>();

        public static GunAdditionalData GetAdditionalData(this Gun gun)
        {
            return data.GetOrCreateValue(gun);
        }

        public static void AddData(this Gun gun, GunAdditionalData value)
        {
            try
            {
                data.Add(gun, value);
            }
            catch (Exception) { }
        }
    }
    [HarmonyPatch(typeof(Gun), "ApplyProjectileStats")]
    class GunPatchApplyProjectileStats
    {
        private static void Prefix(Gun __instance, GameObject obj, int numOfProj = 1, float damageM = 1f, float randomSeed = 0f)
        {
            if (__instance.GetAdditionalData().minDistanceMultiplier != 1f)
            {
                obj.AddComponent<ChangeDamageMultiplierAfterDistanceTravelled>().distance *= __instance.GetAdditionalData().minDistanceMultiplier;
            }
        }
    }

    // patch for murder card
    [HarmonyPatch(typeof(GM_ArmsRace), "RoundTransition")]
    class GM_ArmsRacePatchRoundTransition : MonoBehaviour
    {
        private static bool Prefix(GM_ArmsRace __instance, int winningTeamID, int killedTeamID)
        {
            __instance.StartCoroutine(murderOnRoundStart(PlayerManager.instance.players.ToArray()));

            return true;

        }
        private static IEnumerator murderOnRoundStart(Player[] players)
        {
            while(!GameManager.instance.battleOngoing) { yield return null; }


            for (int j = 0; j < players.Length; j++)
            {
                if (players[j].data.stats.GetAdditionalData().murder >= 1)
                {
                    players[j].data.stats.GetAdditionalData().murder--;
                    Player oppPlayer = PlayerManager.instance.GetOtherPlayer(players[j]);
                    Unbound.Instance.ExecuteAfterSeconds(2f, delegate
                    {
                        typeof(HealthHandler).InvokeMember("RPCA_Die",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, oppPlayer.data.healthHandler,
                                    new object[] { new Vector2(0, 1) });
                    });
                }
            }
            yield break;
        }
    }
}

namespace PCE.Cards
{
    public class LaserCard : CustomCard
    {
        /*
         * fire a laser instead of a gun
         */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.soundDisableRayHitBulletSound = true;
            gun.forceSpecificShake = false;
            gun.recoilMuiltiplier = 0f;
            gun.spread = 0f;
            gun.multiplySpread = 0f;
            gun.size = 0f;
            gun.bursts = 50;
            gun.knockback *= 0.005f;
            gun.projectileSpeed = Math.Min(Math.Max(gun.projectileSpeed*100f, 0f), 100f); // why the hell doesn't Clamp work?
            gun.drag = 0f;
            gun.gravity = 0f;
            gun.timeBetweenBullets = 0.001f;
            if (gun.projectileColor.a > 0f)
            {
                gun.projectileColor = new Color(1f, 0f, 0f, 0.1f);
            }
            gun.multiplySpread = 0f;
            gun.shakeM = 0f;
            gun.bulletDamageMultiplier = 0.01f;
            gunAmmo.reloadTimeMultiplier *= 1.5f;
            gunAmmo.maxAmmo = 1;
            gun.destroyBulletAfter = 1f;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Laser";
        }
        protected override string GetDescription()
        {
            return "Light Amplification by Stimulated Emission of Radiation";
        }
        protected override GameObject GetCardArt()
        {
            return PCE.ArtAssets.LoadAsset<GameObject>("C_Laser");
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                positive = true,
                stat = "Bullets",
                amount = "Light",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Bullet Speed",
                amount = "+10000%",
                simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Damage",
                amount = "-99%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Reload Time",
                amount = "+50%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Ammo",
                amount = "1",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }
    }
    public class GhostGunCard : CustomCard
    {
        /*
         * bullets are invisible and go through walls
         */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.ignoreWalls = true;
            gun.bulletDamageMultiplier = 0.75f;
            gun.projectileColor = Color.clear;
            if (gun.destroyBulletAfter == 0f) { gun.destroyBulletAfter = 5f; }
            gun.unblockable = true;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Ghost Bullets";
        }
        protected override string GetDescription()
        {
            return "Bullets are invisible, go through walls, and penetrate shields";
        }

        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                positive = true,
                stat = "Bullets",
                amount = "Invisible",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Damage",
                amount = "-25%",
                simepleAmount = CardInfoStat.SimpleAmount.slightlyLower
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
    }

    public class TractorBeamCard : CustomCard
    {
        /*
         *  Bullets do double reverse knockback
         */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.knockback = -2f * Math.Abs(gun.knockback);

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Tractor Beam";
        }
        protected override string GetDescription()
        {
            return "Bullets pull opponents toward you on contact";
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
                stat = "Knockback",
                amount = "-2×",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
    }

    public class MoonShoesCard : CustomCard
    {
        /*
        *  player gravity reduced to 1/6th normal
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gravity.gravityForce /= 6f;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Moon Shoes";
        }
        protected override string GetDescription()
        {
            return null;
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
                stat = "Gravity",
                amount = "-83%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotLower
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
    }
    public class JetpackCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // TODO: this doesn't work as intended... this gives infinite jumps instead of 10

            data.jumps += 10;
            characterStats.jump = 0.5f;
            characterStats.movementSpeed *= 0.75f;
            data.maxHealth *= 0.75f;

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Jetpack";
        }
        protected override string GetDescription()
        {
            return "Fly around for a bit";
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
                stat = "Jetpack",
                amount = "A",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Jetpack Fuel",
                amount = "+10",
                simepleAmount = CardInfoStat.SimpleAmount.Some
                },
                new CardInfoStat
                {
                positive = false,
                stat = "HP",
                amount = "-25%",
                simepleAmount = CardInfoStat.SimpleAmount.lower
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Movement Speed",
                amount = "-25%",
                simepleAmount = CardInfoStat.SimpleAmount.lower
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
    }
    public class FlipCard : CustomCard
    {
        /*
        *  Bullets temporarily invert victim's gravity
        */

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

            characterStats.GetAdditionalData().gravityMultiplierOnDoDamage = -1f * Math.Abs(characterStats.GetAdditionalData().gravityMultiplierOnDoDamage);
            if (characterStats.GetAdditionalData().gravityDurationOnDoDamage < 4f)
            {
                characterStats.GetAdditionalData().gravityDurationOnDoDamage += 1.5f;
            }

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Flip";
        }
        protected override string GetDescription()
        {
            return "Bullets temporarily flip victim's direction of gravity";
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
                stat = "Bullets",
                amount = "Antigravity",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
    }
    public class GroundedCard : CustomCard
    {
        /*
         *  Bullets temporarily increase victim's gravity
         */

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

            characterStats.GetAdditionalData().gravityMultiplierOnDoDamage *= 2f;
            if (characterStats.GetAdditionalData().gravityDurationOnDoDamage < 4f)
            {
                characterStats.GetAdditionalData().gravityDurationOnDoDamage += 1.5f;
            }


        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Grounded";
        }
        protected override string GetDescription()
        {
            return "Bullets temporarily increase victim's gravity";
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
                stat = "Bullets",
                amount = "Grounding",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
    }
    public class AntCard : CustomCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

            //TODO: this card doesn't work unless the small-player-size gravity glitch is fixed

            data.maxHealth /= 2f;
            characterStats.sizeMultiplier /= 2f;
            gun.bulletDamageMultiplier *= 2f;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Ant";
        }
        protected override string GetDescription()
        {
            return "Halve in size; double in strength";
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
                stat = "Damage",
                amount = "Double",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                positive = true,
                stat = "Size",
                amount = "Half",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                positive = false,
                stat = "HP",
                amount = "Half",
                simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },

            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.MagicPink;
        }
    }
    public class MurderCard : CustomCard
    {
        /*
        *  A rare card which kills the opposing player immediately at the start of the next round
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.GetAdditionalData().murder += 1;

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
            return "Kill your opponent";
        }

        protected override GameObject GetCardArt()
        {
            return null;
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
    }
    public class JackpotCard : CustomCard
    {
        /*
        *  An Uncommon card which gives the player a random Rare card
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // get array of all cards
            CardInfo[] cards = global::CardChoice.instance.cards;

            // pseudorandom number generator
            System.Random r = new System.Random();

            int rID = r.Next(0, cards.Length); // random card index

            int i = 0;
            int maxAttempts = 1000;

            // draw a random card until it's a rare or the maximum number of attempts was reached
            while (cards[rID].rarity != CardInfo.Rarity.Rare && i < maxAttempts)
            {
                rID = r.Next(0, cards.Length);
                i++;
            }

            // add the card to the player's deck
            ApplyCardStats cardStats = cards[rID].gameObject.GetComponentInChildren<ApplyCardStats>();
            cardStats.GetComponent<CardInfo>().sourceCard = cards[rID];
            cardStats.Pick(player.playerID, true, PickerType.Player);
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Jackpot";
        }
        protected override string GetDescription()
        {
            return "Get a random <color=#ff00ffff>Rare</color> card";
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
    }
    public class SmallJackpotCard : CustomCard
    {
        /*
         *  A Common card which gives the player a random Uncommon card
         */

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // get array of all cards
            CardInfo[] cards = global::CardChoice.instance.cards;

            // pseudorandom number generator
            System.Random r = new System.Random();

            int rID = r.Next(0, cards.Length); // random card index

            int i = 0;
            int maxAttempts = 1000;

            // draw a random card until it's an uncommon or the maximum number of attempts was reached
            while (cards[rID].rarity != CardInfo.Rarity.Uncommon && i < maxAttempts)
            {
                rID = r.Next(0, cards.Length);
                i++;
            }

            // add the card to the player's deck
            ApplyCardStats cardStats = cards[rID].gameObject.GetComponentInChildren<ApplyCardStats>();
            cardStats.GetComponent<CardInfo>().sourceCard = cards[rID];
            cardStats.Pick(player.playerID, true, PickerType.Player);
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Small Jackpot";
        }
        protected override string GetDescription()
        {
            return "Get a random <color=#00ffffff>Uncommon</color> card";
        }

        protected override GameObject GetCardArt()
        {
            return null;
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
    }
    public class GambleCard : CustomCard
    {
        /*
        *  A Rare card which gives the player two random Uncommon cards
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // get array of all cards
            CardInfo[] cards = global::CardChoice.instance.cards;

            // pseudorandom number generator
            System.Random r = new System.Random();

            int rID1 = r.Next(0, cards.Length); // random card index
            int rID2 = r.Next(0, cards.Length); // random card index


            int i = 0;
            int maxAttempts = 1000;

            // draw a random card until it's a uncommon or the maximum number of attempts was reached
            while (cards[rID1].rarity != CardInfo.Rarity.Uncommon && i < maxAttempts)
            {
                rID1 = r.Next(0, cards.Length);
                i++;
            }
            // draw a random card until it's a uncommon or the maximum number of attempts was reached
            while (cards[rID2].rarity != CardInfo.Rarity.Uncommon && i < maxAttempts)
            {
                rID2 = r.Next(0, cards.Length);
                i++;
            }

            // add the cards to the player's deck
            ApplyCardStats cardStats1 = cards[rID1].gameObject.GetComponentInChildren<ApplyCardStats>();
            cardStats1.GetComponent<CardInfo>().sourceCard = cards[rID1];
            cardStats1.Pick(player.playerID, true, PickerType.Player);
            ApplyCardStats cardStats2 = cards[rID2].gameObject.GetComponentInChildren<ApplyCardStats>();
            cardStats2.GetComponent<CardInfo>().sourceCard = cards[rID2];
            cardStats2.Pick(player.playerID, true, PickerType.Player);
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Gamble";
        }
        protected override string GetDescription()
        {
            return "Get <b>two</b> random <color=#00ffffff>Uncommon</color> cards";
        }

        protected override GameObject GetCardArt()
        {
            return null;
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
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
    }
    public class RiskyGambleCard : CustomCard
    {
        /*
        *  An Uncommon card which gives the player two random Common cards
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // get array of all cards
            CardInfo[] cards = global::CardChoice.instance.cards;

            // pseudorandom number generator
            System.Random r = new System.Random();

            int rID1 = r.Next(0, cards.Length); // random card index
            int rID2 = r.Next(0, cards.Length); // random card index


            int i = 0;
            int maxAttempts = 1000;

            // draw a random card until it's a common or the maximum number of attempts was reached
            while (cards[rID1].rarity != CardInfo.Rarity.Common && i < maxAttempts)
            {
                rID1 = r.Next(0, cards.Length);
                i++;
            }
            // draw a random card until it's a common or the maximum number of attempts was reached
            while (cards[rID2].rarity != CardInfo.Rarity.Common && i < maxAttempts)
            {
                rID2 = r.Next(0, cards.Length);
                i++;
            }

            // add the cards to the player's deck
            ApplyCardStats cardStats1 = cards[rID1].gameObject.GetComponentInChildren<ApplyCardStats>();
            cardStats1.GetComponent<CardInfo>().sourceCard = cards[rID1];
            cardStats1.Pick(player.playerID, true, PickerType.Player);
            ApplyCardStats cardStats2 = cards[rID2].gameObject.GetComponentInChildren<ApplyCardStats>();
            cardStats2.GetComponent<CardInfo>().sourceCard = cards[rID2];
            cardStats2.Pick(player.playerID, true, PickerType.Player);
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
    }
    public class CloseQuartersCard : CustomCard
    {
        /*
        *  More damage, but damage falloff is significantly increased
        */
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.damageAfterDistanceMultiplier = 0.1f;
            gun.GetAdditionalData().minDistanceMultiplier = 0f;
        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Close Quarters";
        }
        protected override string GetDescription()
        {
            return "Do significantly more damage up close";
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
                stat = "Damage",
                amount = "+100%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                },
                new CardInfoStat
                {
                positive = false,
                stat = "Damage Falloff",
                amount = "+100%",
                simepleAmount = CardInfoStat.SimpleAmount.aLotOf
                },

};
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.FirepowerYellow;
        }
    }
}
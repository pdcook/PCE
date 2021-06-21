using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;
using UnityEngine;
using PCE;
using UnboundLib;
using PCE.Extensions;
using PCE.RoundsEffects;
using PCE.MonoBehaviours;


namespace PCE.Cards
{
    public class LowGroundCard : CustomCard
    {
        /*
        *  Get boosted stats when below an enemy player
        */

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {

        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            InConeEffect newEffect = player.gameObject.GetOrAddComponent<InConeEffect>();

            newEffect.SetCenterRay(new Vector2(0f, 1f));
            newEffect.SetColor(Color.green);
            newEffect.SetEffectFunc(this.statboost);
            newEffect.SetAngle(90f);

        }
        public override void OnRemoveCard()
        {
        }

        protected override string GetTitle()
        {
            return "Low Ground";
        }
        protected override string GetDescription()
        {
            return "Increased stats when below an enemy player";
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
            return CardThemeColor.CardThemeColorType.FirepowerYellow;
        }
        public List<MonoBehaviour> statboost(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            List<MonoBehaviour> effects = new List<MonoBehaviour>();

            Gun newGun = player.gameObject.AddComponent<Gun>();
            //GunAmmo newGunAmmo = player.gameObject.AddComponent<GunAmmo>();
            GunEffect gunEffect = player.gameObject.GetOrAddComponent<GunEffect>();
            GunEffect.CopyGunStats(gun, newGun);
            GunAmmoStats newGunAmmoStats = GunEffect.GetGunAmmoStats(gunAmmo);

            newGun.projectileSpeed *= 2f;
            newGun.projectielSimulatonSpeed *= 2f;
            newGun.damage *= 2f;
            newGun.projectileColor = Color.red;
            newGun.attackSpeed *= 0.5f;

            newGunAmmoStats.maxAmmo += 3;
            newGunAmmoStats.reloadTime *= 0.75f;

            //gunEffect.SetGun(newGun);
            //gunEffect.SetGunAmmo(newGunAmmo);

            gunEffect.SetGunAndGunAmmoStats(newGun, newGunAmmoStats);

            effects.Add(gunEffect);

            CharacterStatModifiers newStats = player.gameObject.AddComponent<CharacterStatModifiers>();
            CharacterStatModifiersEffect statsEffect = player.gameObject.GetOrAddComponent<CharacterStatModifiersEffect>();
            CharacterStatModifiersEffect.CopyStats(characterStats, newStats);

            newStats.movementSpeed *= 2f;

            statsEffect.SetStats(newStats);

            effects.Add(statsEffect);

            return effects;
        }

    }
}

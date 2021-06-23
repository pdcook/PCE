using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using UnityEngine;
using System.Linq;
using HarmonyLib;

namespace PCE.MonoBehaviours
{
    public class InConeEffect : MonoBehaviour
    {
        // internal variables
        private Player player;
        private Gun gun;
        private CharacterData data;
        private HealthHandler health;
        private Gravity gravity;
        private Block block;
        private GunAmmo gunAmmo;
        private CharacterStatModifiers statModifiers;
        private List<ColorEffect> colorEffects = new List<ColorEffect>();
        private List<MonoBehaviour> effects = new List<MonoBehaviour>();
        private float timeOfLastApply; // last time the effect was applied
        private bool effectApplied = false;


        // variables set by card

        private Vector2 centerRay = Vector2.zero; // center ray of the cone
        private float range = float.MaxValue; // maximum range from the player for the effect to work
        private float angle = 361f; // arc angle (in degrees) of the cone
        private float period = 0f; // how often to reapply the effect while conditions are met, 0f for once only. will be unapplied when conditions are no longer met and reapplied when they are

        // the function that adds. applies, sets, and returns a list of monobehaivors to apply to the player
        private Func<Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, List<MonoBehaviour>> effectFunc = null;
        // the function that adds. applies, sets, and returns a list of monobehaivors to apply to the other player(s)
        private Func<Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, List<MonoBehaviour>> otherPlayerEffectFunc = null;

        private Color colorMaxWhileActive = Color.clear; // colorMax of the player while conditions are met, clear for none
        private Color colorMinWhileActive = Color.clear; // colorMin of the player while conditions are met, clear for none
        private Color otherPlayerColorMaxWhileActive = Color.clear; // colorMax of the other players while conditions are met, clear for none
        private Color otherPlayerColorMinWhileActive = Color.clear; // colorMin of the other players while conditions are met, clear for none
        private bool needsLineOfSight = false; // does the effect need line of sight?
        private bool checkEnemiesOnly = true; // should only enemies be able to activate the effect?
        private bool applyToOthers = false; // should the effect apply to other players in the cone (true)?
        private bool applyToSelf = true; //  should the effect apply to the player with this component (true)? 


        void Awake()
        {
            this.player = gameObject.GetComponent<Player>();
            this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            this.data = this.player.GetComponent<CharacterData>();
            this.health = this.player.GetComponent<HealthHandler>();
            this.gravity = this.player.GetComponent<Gravity>();
            this.block = this.player.GetComponent<Block>();
            this.gunAmmo = this.gun.GetComponentInChildren<GunAmmo>();
            this.statModifiers = this.player.GetComponent<CharacterStatModifiers>();
        }

        void Start()
        {
            // if the centerRay has magnitude 0 (i.e. has not been set or hasn't been set properly), just destroy this component
            if (centerRay.magnitude == 0f)
            {
                this.Destroy();
            }
            else
            {
                // otherwise, normalize it
                centerRay = centerRay.normalized;
            }
            // if the effectFunc has not been set, just destroy this component
            if (this.effectFunc == null)
            {
                this.Destroy();
            }

            this.ResetTimer();

        }

        void Update()
        {
            if (this.ConditionsMet())
            {
                if (this.period == 0f && !this.effectApplied)
                {
                    this.ApplyEffectToAllPrescribedPlayers();
                    this.ResetTimer();
                    this.effectApplied = true;
                }
                // if the period is 0, then it only needed to be applied once, so just return
                else if (this.period == 0f && this.effectApplied)
                {
                    return;
                }
                // otherwise, it may need to be applied multiple times
                else if (Time.time >= this.timeOfLastApply + this.period)
                {
                    this.ApplyEffectToAllPrescribedPlayers();
                    this.ResetTimer();
                }
            }
            // if the conditions are not met, then remove all effects that may be present
            else
            {
                this.RemoveAllEffects();
                this.effectApplied = false;
            }

        }
        public bool ConditionsMet()
        {
            return (this.GetValidPlayersInCone().Count > 0);
        }
        public void ApplyEffectToAllPrescribedPlayers()
        {
            List<Player> validPlayers = this.GetValidPlayersInCone();
            if (this.applyToOthers)
            {
                this.ApplyEffectToOthers(validPlayers);
            }
            if (this.applyToSelf)
            {
                this.ApplyEffectToSelf();
            }
        }
        public List<Player> GetValidPlayersInCone()
        {
            if (this.checkEnemiesOnly)
            {
                return this.GetAllEnemyPlayers().Where(player => this.OtherPlayerInCone(player)).ToList();
            }
            else
            {
                return this.GetAllOtherPlayers().Where(player => this.OtherPlayerInCone(player)).ToList();
            }
        }
        public List<Player> GetAllEnemyPlayers()
        {
            return PlayerManager.instance.players.Where(player => player.teamID != this.player.teamID).ToList();
        }
        public List<Player> GetAllOtherPlayers()
        {
            return PlayerManager.instance.players.Where(player => player.playerID != this.player.playerID).ToList();
        }
        public bool OtherPlayerInCone(Player otherPlayer)
        {
            bool lineOfSight = true;
            // if the effect needs line-of-sight, then check for it
            if (this.needsLineOfSight)
            {
                CanSeeInfo canSeeInfo = PlayerManager.instance.CanSeePlayer(this.player.transform.position, otherPlayer);
                lineOfSight = canSeeInfo.canSee;
            }

            Vector2 displacement = otherPlayer.transform.position - this.player.transform.position;
            return (lineOfSight && displacement.magnitude <= this.range && Vector2.Angle(this.centerRay, displacement) <= Math.Abs(this.angle / 2));
        }
        public void OnDestroy()
        {
            this.RemoveAllEffects();
        }
        public void ApplyEffectToSelf()
        {
            List<MonoBehaviour> newEffects = this.effectFunc(this.player, this.gun, this.gunAmmo, this.data, this.health, this.gravity, this.block, this.statModifiers);
            foreach (MonoBehaviour newEffect in newEffects)
            {
                this.effects.Add(newEffect);
            }
            this.ApplyColorEffectToSelf();
        }
        public void ApplyEffectToOthers(List<Player> otherPlayers)
        {
            Gun otherGun;
            CharacterData otherData;
            HealthHandler otherHealth;
            Gravity otherGravity;
            Block otherBlock;
            GunAmmo otherGunAmmo;
            CharacterStatModifiers otherStatModifiers;

            foreach (Player otherPlayer in otherPlayers)
            {
                otherGun = otherPlayer.GetComponent<Holding>().holdable.GetComponent<Gun>();
                otherData = otherPlayer.GetComponent<CharacterData>();
                otherHealth = otherPlayer.GetComponent<HealthHandler>();
                otherGravity = otherPlayer.GetComponent<Gravity>();
                otherBlock = otherPlayer.GetComponent<Block>();
                otherGunAmmo = otherGun.GetComponentInChildren<GunAmmo>();
                otherStatModifiers = otherPlayer.GetComponent<CharacterStatModifiers>();

                List<MonoBehaviour> newEffects = this.otherPlayerEffectFunc(otherPlayer, otherGun, otherGunAmmo, otherData, otherHealth, otherGravity, otherBlock, otherStatModifiers);
                foreach (MonoBehaviour newEffect in newEffects)
                {
                    this.effects.Add(newEffect);
                }
                this.ApplyColorEffectToOtherPlayer(otherPlayer);
            }
        }
        public void ApplyColorEffectToSelf()
        {
            ColorEffect newColorEffect = this.player.gameObject.AddComponent<ColorEffect>();
            newColorEffect.SetColorMax(this.colorMaxWhileActive);
            newColorEffect.SetColorMin(this.colorMinWhileActive);

            this.colorEffects.Add(newColorEffect);
        }
        public void ApplyColorEffectToOtherPlayer(Player otherPlayer)
        {
            ColorEffect newColorEffect = otherPlayer.gameObject.AddComponent<ColorEffect>();
            newColorEffect.SetColorMax(this.otherPlayerColorMaxWhileActive);
            newColorEffect.SetColorMin(this.otherPlayerColorMinWhileActive);

            this.colorEffects.Add(newColorEffect);
        }
        public void RemoveEffects()
        {
            foreach (MonoBehaviour effect in this.effects)
            {
                if (effect != null)
                {
                    Destroy(effect);
                }
            }
            this.effects = new List<MonoBehaviour>();
        }
        public void RemoveColorEffects()
        {
            foreach (ColorEffect colorEffect in this.colorEffects)
            {
                if (colorEffect != null)
                {
                    Destroy(colorEffect);
                }
            }
            this.colorEffects = new List<ColorEffect>();
        }
        public void RemoveAllEffects()
        {
            this.RemoveEffects();
            this.RemoveColorEffects();
        }
        public void ResetTimer()
        {
            this.timeOfLastApply = Time.time;
        }
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }

        public void SetCenterRay(Vector2 ray)
        {
            this.centerRay = ray;
        }
        public void SetRange(float range)
        {
            this.range = range;
        }
        public void SetAngle(float angle)
        {
            this.angle = angle;
        }
        public void SetPeriod(float period)
        {
            this.period = period;
        }
        public void SetEffectFunc(Func<Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, List<MonoBehaviour>> effectFunc)
        {
            this.effectFunc = effectFunc;
        }
        public void SetOtherEffectFunc(Func<Player, Gun, GunAmmo, CharacterData, HealthHandler, Gravity, Block, CharacterStatModifiers, List<MonoBehaviour>> effectFunc)
        {
            this.otherPlayerEffectFunc = effectFunc;
        }
        public void SetColor(Color color)
        {
            this.colorMaxWhileActive = color;
            this.colorMinWhileActive = color;
        }
        public void SetColorMin(Color color)
        {
            this.colorMinWhileActive = color;
        }
        public void SetColorMax(Color color)
        {
            this.colorMaxWhileActive = color;
        }
        public void SetNeedsLineOfSight(bool needsLineOfSight)
        {
            this.needsLineOfSight = needsLineOfSight;
        }
        public void SetCheckEnemiesOnly(bool checkEnemiesOnly)
        {
            this.checkEnemiesOnly = checkEnemiesOnly;
        }
        public void SetApplyToOthers(bool applyToOthers)
        {
            this.applyToOthers = applyToOthers;
        }
        public void SetApplyToSelf(bool applyToSelf)
        {
            this.applyToSelf = applyToSelf;
        }
    }
}

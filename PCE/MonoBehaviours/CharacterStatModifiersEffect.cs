using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours; 

namespace PCE.MonoBehaviours
{

    public class CharacterStatModifiersEffect : MonoBehaviour
    {

		private CharacterStatModifiers originalStats;
		
		private Player player;
		private CharacterStatModifiers statsToSet = null;

		void Awake()
		{

			this.originalStats = this.gameObject.AddComponent<CharacterStatModifiers>();

            this.player = this.gameObject.GetComponent<Player>();

			// save the original stats
			CharacterStatModifiersEffect.CopyStats(this.player.data.stats, this.originalStats);
		}

        void Start()
        {
			if (this.statsToSet != null)
			{
				CharacterStatModifiersEffect.CopyStats(this.statsToSet, this.player.data.stats);
			}
		}
		void Update()
        {

        }
		void LateUpdate()
        {

		}
        public void OnDestroy()
        {
			// reset stats back to original
			CharacterStatModifiersEffect.CopyStats(this.originalStats, this.player.data.stats);
			// destroy the new stats and the copy of the original
			Destroy(this.statsToSet);
			Destroy(this.originalStats);
		}
		public void Destroy()
		{
			UnityEngine.Object.Destroy(this);
		}
		public void SetStats(CharacterStatModifiers stats)
		{
			this.statsToSet = stats;
		}
		public static void CopyStats(CharacterStatModifiers copyFromStats, CharacterStatModifiers copyToStats)
		{
			// get private field "data"
			CharacterData copyToData = (CharacterData)Traverse.Create(copyToStats).Field("data").GetValue();
			CharacterData copyFromData = (CharacterData)Traverse.Create(copyFromStats).Field("data").GetValue();

			copyToStats.objectsAddedToPlayer = copyFromStats.objectsAddedToPlayer;
			//copyToData.health = copyFromData.health;
			//copyToData.maxHealth = copyFromData.maxHealth;
			copyToStats.sizeMultiplier = copyFromStats.sizeMultiplier;
			copyToStats.health = copyFromStats.health;
			copyToStats.movementSpeed = copyFromStats.movementSpeed;
			copyToStats.jump = copyFromStats.jump;
			copyToStats.gravity = copyFromStats.gravity;
			copyToStats.slow = copyFromStats.slow;
			copyToStats.slowSlow = copyFromStats.slowSlow;
			copyToStats.fastSlow = copyFromStats.fastSlow;
			copyToStats.secondsToTakeDamageOver = copyFromStats.secondsToTakeDamageOver;
			copyToStats.numberOfJumps = copyFromStats.numberOfJumps;
			copyToStats.regen = copyFromStats.regen;
			copyToStats.lifeSteal = copyFromStats.lifeSteal;
			copyToStats.respawns = copyFromStats.respawns;
			copyToStats.refreshOnDamage = copyFromStats.refreshOnDamage;
			copyToStats.automaticReload = copyFromStats.automaticReload;
			copyToStats.tasteOfBloodSpeed = copyFromStats.tasteOfBloodSpeed;
			copyToStats.rageSpeed = copyFromStats.rageSpeed;
			copyToStats.attackSpeedMultiplier = copyFromStats.attackSpeedMultiplier;

			
			copyToStats.regen = copyFromStats.regen;
			copyToStats.slowPart = copyFromStats.slowPart;
			copyToStats.soundCharacterSlowFreeze = copyFromStats.soundCharacterSlowFreeze;
			copyToStats.DealtDamageAction = copyFromStats.DealtDamageAction;
			copyToStats.WasDealtDamageAction = copyFromStats.WasDealtDamageAction;

			// copy various private fields
			Traverse.Create(copyToStats).Field("dealtDamageEffects").SetValue((DealtDamageEffect[])Traverse.Create(copyFromStats).Field("dealtDamageEffects").GetValue());
			Traverse.Create(copyToStats).Field("soundBigThreshold").SetValue((float)Traverse.Create(copyFromStats).Field("soundBigThreshold").GetValue());
			Traverse.Create(copyToStats).Field("soundSlowSpeedSec").SetValue((float)Traverse.Create(copyFromStats).Field("soundSlowSpeedSec").GetValue());
			Traverse.Create(copyToStats).Field("soundSlowTime").SetValue((float)Traverse.Create(copyFromStats).Field("soundSlowTime").GetValue());
			Traverse.Create(copyToStats).Field("wasDealtDamageEffects").SetValue((WasDealtDamageEffect[])Traverse.Create(copyFromStats).Field("soundSwasDealtDamageEffectslowTime").GetValue());

			// set private field "data"
			Traverse.Create(copyToStats).Field("data").SetValue(copyToData);

			copyToStats.WasUpdated();

			// call internal method "ConfigureMassAndSize"
			//typeof(CharacterStatModifiers).InvokeMember("ConfigureMassAndSize", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, copyToStats, new object[] { });
		}
	}
}

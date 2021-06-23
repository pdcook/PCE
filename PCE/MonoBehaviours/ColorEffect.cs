using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;
using PCE.Extensions;

namespace PCE.MonoBehaviours
{
	public class ColorEffect : MonoBehaviour
    {
		private ColorEffectBase colorEffectBase;
		private SetTeamColor[] teamColors;
		private Player player;
		private Color colorMinToSet;
		private Color colorMaxToSet;

		public void Awake()
        {
			this.player = this.gameObject.GetComponent<Player>();

			// create the base only if it doesn't already exist, to prevent accidentally running Awake again
			this.colorEffectBase = this.player.gameObject.GetOrAddComponent<ColorEffectBase>();
			this.colorEffectBase.colorEffectDrones.Add(this);

			this.teamColors = this.player.transform.root.GetComponentsInChildren<SetTeamColor>();
		}
		void Start()
		{
			this.ApplyColor();
		}

		void Update()
		{
		}
		public void OnDestroy()
		{
			// tell the base that the color effect is over
			this.colorEffectBase.OnDroneDestroy(this);

		}
		public void ApplyColor()
		{
			PlayerSkinParticle[] componentsInChildren2 = this.player.gameObject.GetComponentsInChildren<PlayerSkinParticle>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				ParticleSystem particleSystem2 = (ParticleSystem)componentsInChildren2[j].GetFieldValue("part");
				ParticleSystem.MainModule main2 = particleSystem2.main;
				ParticleSystem.MinMaxGradient startColor2 = particleSystem2.main.startColor;
				startColor2.colorMin = this.colorMinToSet;
				startColor2.colorMax = this.colorMaxToSet;
				main2.startColor = startColor2;
			}
			SetTeamColor[] teamColors = this.teamColors;
			for (int j = 0; j < teamColors.Length; j++)
			{
				teamColors[j].Set(new PlayerSkin
				{
					color = this.colorMaxToSet,
					backgroundColor = this.colorMaxToSet,
					winText = this.colorMaxToSet,
					particleEffect = this.colorMaxToSet
				});
			}
		}
		public void SetColor(Color color)
        {
			this.colorMaxToSet = color;
			this.colorMinToSet = color;
        }
		public void SetColorMax(Color color)
		{
			this.colorMaxToSet = color;
		}
		public void SetColorMin(Color color)
		{
			this.colorMinToSet = color;
		}
		public void Destroy()
        {
			UnityEngine.GameObject.Destroy(this);
        }
	}
	internal class ColorEffectBase : MonoBehaviour
    {

		internal List<ColorEffect> colorEffectDrones = new List<ColorEffect>();
		private Color originalColorMax;
		private Color originalColorMin;
		private Player player;
		private SetTeamColor[] teamColors;

		public void Awake()
        {
			this.player = this.gameObject.GetComponent<Player>();
			// get original color
			this.originalColorMin = GetPlayerColor.GetColorMin(this.player);
			this.originalColorMax = GetPlayerColor.GetColorMax(this.player);

			this.teamColors = this.player.transform.root.GetComponentsInChildren<SetTeamColor>();
		}
		public void OnDroneDestroy(ColorEffect colorEffect)
        {
			int idx = this.colorEffectDrones.IndexOf(colorEffect);
			// if it was the only drone left, then reapply the original colors
			if (this.colorEffectDrones.Count == 1 && idx == 0)
            {
				this.ResetColor();
            }
			// if it was the last drone in the list, then reapply the previous color
			else if (idx == this.colorEffectDrones.Count-1)
            {
				this.colorEffectDrones[idx - 1].ApplyColor();
            }
			// if it was in the middle of the list, do nothing
			else
            {

            }
			// then remove it from the list
			this.colorEffectDrones.Remove(colorEffect);

        }
		private void ResetColor()
		{
			PlayerSkinParticle[] componentsInChildren2 = this.player.gameObject.GetComponentsInChildren<PlayerSkinParticle>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				ParticleSystem particleSystem2 = (ParticleSystem)componentsInChildren2[j].GetFieldValue("part");
				ParticleSystem.MainModule main2 = particleSystem2.main;
				ParticleSystem.MinMaxGradient startColor2 = particleSystem2.main.startColor;
				startColor2.colorMin = this.originalColorMin;
				startColor2.colorMax = this.originalColorMax;
				main2.startColor = startColor2;
			}
			SetTeamColor[] teamColors = this.teamColors;
			for (int j = 0; j < teamColors.Length; j++)
			{
				teamColors[j].Set(new PlayerSkin
				{
					color = this.originalColorMax,
					backgroundColor = this.originalColorMax,
					winText = this.originalColorMax,
					particleEffect = this.originalColorMax
				});
			}
		}
		public void OnDestroy()
		{
			foreach (ColorEffect colorEffect in this.colorEffectDrones)
			{
				if (colorEffect != null) { Destroy(colorEffect); }
			}
			this.ResetColor();
		}
		public void Destroy()
		{
			UnityEngine.GameObject.Destroy(this);
		}

	}
	public class GunColorEffect : MonoBehaviour
	{
		private GunColorEffectBase gunColorEffectBase;
		private Player player;
		private Gun gun;
		private Color colorToSet;

		public void Awake()
		{
			this.player = this.gameObject.GetComponent<Player>();
			this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();
			// create the base only if it doesn't already exist, to prevent accidentally running Awake again
			this.gunColorEffectBase = this.player.gameObject.GetOrAddComponent<GunColorEffectBase>();
			this.gunColorEffectBase.gunColorEffectDrones.Add(this);
		}
		void Start()
		{
			this.ApplyColor();
		}

		void Update()
		{
		}
		public void OnDestroy()
		{
			// tell the base that the color effect is over
			this.gunColorEffectBase.OnDroneDestroy(this);

		}
		public void ApplyColor()
		{
			this.gun.projectileColor = this.colorToSet;
		}
		public void SetColor(Color color)
		{
			this.colorToSet = color;
		}
		public void Destroy()
		{
			UnityEngine.GameObject.Destroy(this);
		}
	}
	internal class GunColorEffectBase : MonoBehaviour
	{

		internal List<GunColorEffect> gunColorEffectDrones = new List<GunColorEffect>();
		private Color originalColor;
		private Player player;
		private Gun gun;

		public void Awake()
		{
			this.player = this.gameObject.GetComponent<Player>();
			this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();
			// get original color
			this.originalColor = this.gun.projectileColor;
		}
		public void OnDroneDestroy(GunColorEffect gunColorEffect)
		{
			int idx = this.gunColorEffectDrones.IndexOf(gunColorEffect);
			// if it was the only drone left, then reapply the original colors
			if (this.gunColorEffectDrones.Count == 1 && idx == 0)
			{
				this.ResetColor();
			}
			// if it was the last drone in the list, then reapply the previous color
			else if (idx == this.gunColorEffectDrones.Count - 1)
			{
				this.gunColorEffectDrones[idx - 1].ApplyColor();
			}
			// if it was in the middle of the list, do nothing
			else
			{

			}
			// then remove it from the list
			this.gunColorEffectDrones.Remove(gunColorEffect);

		}
		private void ResetColor()
		{
			this.gun.projectileColor = this.originalColor;
		}

		public void OnDestroy()
        {
			foreach (GunColorEffect gunColorEffect in this.gunColorEffectDrones)
            {
				if(gunColorEffect != null) { Destroy(gunColorEffect); }
            }
			this.ResetColor();
        }
		public void Destroy()
		{
			UnityEngine.GameObject.Destroy(this);
		}

	}
}

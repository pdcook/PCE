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

    public class ColorEffect : MonoBehaviour
    {
		private Color colorMinToSet;
		private Color colorMaxToSet;
		private SetTeamColor[] teamColors;
		private Color colorMax;
		private Color colorMin;

		private Player player;

        void Awake()
        {
            this.player = this.gameObject.GetComponent<Player>();
        }

        void Start()
        {
			// I "borrowed" this code from Willis

			// get original color
			Color colorMin = Color.white;
			Color colorMax = Color.white;
			Color white = Color.white;
			SetTeamColor[] array = this.player.gameObject.GetComponentsInChildren<SetTeamColor>();
			for (int i = 0; i < array.Length; i++)
			{
				object fieldValue = array[i].GetFieldValue("meshRend");
				if (fieldValue != null)
				{
					Color color = ((MeshRenderer)fieldValue).material.color;
				}
			}
			PlayerSkinParticle[] componentsInChildren = this.player.gameObject.GetComponentsInChildren<PlayerSkinParticle>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				ParticleSystem particleSystem = (ParticleSystem)componentsInChildren[i].GetFieldValue("part");
				ParticleSystem.MainModule main = particleSystem.main;
				ParticleSystem.MinMaxGradient startColor = particleSystem.main.startColor;
				this.colorMax = startColor.colorMax;
				this.colorMin = startColor.colorMin;
				startColor.colorMin = Color.black;
				startColor.colorMax = Color.white;
				main.startColor = startColor;
			}
			this.teamColors = this.player.transform.root.GetComponentsInChildren<SetTeamColor>();

			this.ApplyColor(this.colorMinToSet, this.colorMaxToSet);
		}

        void Update()
        {
        }
        public void OnDestroy()
        {
			// reset color back to default
			this.ApplyColor(this.colorMin, this.colorMax);
		}
		public void ApplyColor(Color colorMin, Color colorMax)
        {
			PlayerSkinParticle[] componentsInChildren2 = this.player.gameObject.GetComponentsInChildren<PlayerSkinParticle>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				ParticleSystem particleSystem2 = (ParticleSystem)componentsInChildren2[j].GetFieldValue("part");
				ParticleSystem.MainModule main2 = particleSystem2.main;
				ParticleSystem.MinMaxGradient startColor2 = particleSystem2.main.startColor;
				startColor2.colorMin = colorMin;
				startColor2.colorMax = colorMax;
				main2.startColor = startColor2;
			}
			SetTeamColor[] teamColors = this.teamColors;
			for (int j = 0; j < teamColors.Length; j++)
			{
				teamColors[j].Set(new PlayerSkin
				{
					color = colorMax,
					backgroundColor = colorMax,
					winText = colorMax,
					particleEffect = colorMax
				});
			}
		}
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }
		public void SetColorMax(Color color)
		{
			this.colorMaxToSet = color;
		}
		public void SetColorMin(Color color)
		{
			this.colorMinToSet = color;
		}

    }
}

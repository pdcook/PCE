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

    public class ColorFlash : MonoBehaviour
    {
        private float duration = 1f;
        private float startTime;
		private int numberOfFlashes = 1;
		private float delayBetweenFlashes = 1f;
		private Color colorMinToFlash = Color.black;
		private Color colorMaxToFlash = Color.black;
		private SetTeamColor[] teamColors;
		private Color colorMax;
		private Color colorMin;
		private int flashNum = 0;
		private bool flashing = false;

		private Player player;

        void Awake()
        {
            this.player = this.gameObject.GetComponent<Player>();
        }

        void Start()
        {
            this.ResetTimer();

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

			this.Flash(this.colorMinToFlash, this.colorMaxToFlash);
		}

        void Update()
        {
			if (this.flashing && Time.time >= this.startTime + this.duration)
            {
				this.Unflash(this.colorMin, this.colorMax);
            }
			else if (!this.flashing && Time.time >= this.startTime + this.delayBetweenFlashes)
            {
				this.Flash(this.colorMinToFlash, this.colorMaxToFlash);
            }
			else if (this.flashNum >= this.numberOfFlashes)
            {
				this.Destroy();
            }
        }
        public void OnDestroy()
        {
			// reset color back to default
			this.ApplyColor(this.colorMin, this.colorMax);
		}
		public void Flash(Color colorMin, Color colorMax)
        {
			this.flashing = true;
			this.ResetTimer();
			this.ApplyColor(colorMin, colorMax);
		}
		public void Unflash(Color colorMin, Color colorMax)
        {
			this.flashing = false;
			this.flashNum++;
			this.ResetTimer();
			this.ApplyColor(colorMin, colorMax);
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
        public void ResetTimer()
        {
            this.startTime = Time.time;
        }
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }
		public void SetNumberOfFlashes(int flashes)
        {
			this.numberOfFlashes = flashes;
        }
		public void SetDelayBetweenFlashes(float delay)
        {
			this.delayBetweenFlashes = delay;
        }
		public void SetColorMax(Color color)
		{
			this.colorMaxToFlash = color;
		}
		public void SetColorMin(Color color)
		{
			this.colorMinToFlash = color;
		}
		public void SetDuration(float duration)
        {
			this.duration = duration;
        }

    }
}

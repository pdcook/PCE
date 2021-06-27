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
		private int flashNum = 0;
		private bool flashing = false;
		private ColorEffect colorEffect;

		private Player player;

		void Awake()
		{
			this.player = this.gameObject.GetComponent<Player>();
		}

		void Start()
		{
			this.ResetTimer();

			this.Flash(this.colorMinToFlash, this.colorMaxToFlash);
		}

		void Update()
		{
			if (this.flashing && Time.time >= this.startTime + this.duration)
			{
				this.Unflash();
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
			if (this.colorEffect != null)
			{
				Destroy(this.colorEffect);
			}
		}
		public void Flash(Color colorMin, Color colorMax)
		{
			this.flashing = true;
			this.ResetTimer();
			this.colorEffect = this.player.gameObject.AddComponent<ColorEffect>();
			this.colorEffect.SetColorMax(colorMax);
			this.colorEffect.SetColorMin(colorMin);

		}
		public void Unflash()
		{
			this.flashing = false;
			this.flashNum++;
			this.ResetTimer();
			if (this.colorEffect != null) { Destroy(this.colorEffect); }
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
		public void SetColor(Color color)
        {
			this.colorMaxToFlash = color;
			this.colorMinToFlash = color;
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
		public Color GetOriginalColorMax()
		{
			return this.colorEffect.colorEffectBase.originalColorMax;
		}
		public Color GetOriginalColorMin()
		{
			return this.colorEffect.colorEffectBase.originalColorMin;
		}

	}
}

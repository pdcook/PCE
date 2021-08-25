using ModdingUtils.MonoBehaviours;
using UnityEngine;

namespace PCE.MonoBehaviours
{
    public class DiscombobulateEffect : ReversibleEffect
    {
        private float
          startTime,
          duration,
          movementspeedMultiplier = 1f;
        private Color color;
        private ColorFlash colorEffect;
        public override void OnAwake()
        {
            ResetTimer();
        }

        public override void OnStart()
        {
            this.colorEffect = base.player.gameObject.AddComponent<ColorFlash>();
            this.colorEffect.SetColor(this.color);
            this.colorEffect.SetNumberOfFlashes(int.MaxValue);
            this.colorEffect.SetDuration(0.25f);
            this.colorEffect.SetDelayBetweenFlashes(0.25f);
            base.characterStatModifiersModifier.movementSpeed_mult = this.movementspeedMultiplier;
        }

        public override void OnUpdate()
        {
            // when time is up, destroy this effect, the base class will handle cleanup
            if (Time.time - this.startTime >= this.duration)
            {
                UnityEngine.Object.Destroy(this);
            }
        }
        public override void OnOnDestroy()
        {
            if (this.colorEffect != null) { Destroy(this.colorEffect); }
        }
        public void ResetTimer()
        {
            startTime = Time.time;
        }
        public void SetDuration(float duration)
        {
            this.duration = duration;
        }
        public void SetMovementSpeedMultiplier(float mult)
        {
            this.movementspeedMultiplier = mult;
        }
        public void SetColor(Color color)
        {
            this.color = color;
        }
    }
}

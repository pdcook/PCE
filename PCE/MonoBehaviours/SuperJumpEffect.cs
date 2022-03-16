using UnityEngine;
using HarmonyLib;
using ModdingUtils.MonoBehaviours;
using PCE.Extensions;
using UnboundLib;

namespace PCE.MonoBehaviours
{
    public class SuperJumpEffect : ReversibleEffect // using ReversibleEffect just to handle cleanup/resetting
    {
        private bool active = false;
        ColorFlash colorFlash = null;
        private float multiplier = 1f;
        private float timeMultiplier = 0f;
        private readonly float chargeTime = 2.5f;
        private float maxMultiplier = 3f;
        private readonly float minChargeTime = 0.5f;
        private float startTime = -1f;
        private int numberOfBlocks = 0;

        private readonly float outOfBoundsTime = 5f;
        public override void OnAwake()
        {
            base.SetLivesToEffect(int.MaxValue);
        }
        public override void OnStart()
        {
            base.applyImmediately = false;
            this.active = false;
        }
        public override void OnOnEnable()
        {
            this.active = false;
        }
        public override void OnUpdate()
        {

            if (!this.active && base.data.isGrounded && base.data.playerActions.Down.IsPressed)
            {
                this.active = true;
                this.ResetTimer();
            }

            if (this.active && base.data.isGrounded && base.data.playerActions.Down.IsPressed && this.HeldTime() >= this.minChargeTime)
            {
                this.multiplier = UnityEngine.Mathf.Clamp(((this.maxMultiplier - 1f) / (this.chargeTime / this.timeMultiplier - this.minChargeTime)) * (this.HeldTime() - this.minChargeTime) + 1f, 1f, this.maxMultiplier);
            }
            

            if (this.active && base.data.isGrounded && !base.data.playerActions.Down.IsPressed && this.HeldTime()>=this.minChargeTime)
            {
                base.data.jump.Jump(true, this.multiplier);
                if (this.PercCharged() >= 0.5f)
                {
                    this.StartCoroutine(this.BlockAtApex());
                }
                this.StartCoroutine(this.DisableTopOutOfBounds());
                base.data.currentJumps++;
                this.multiplier = 1f;
                this.active = false;
                this.ResetTimer();
            }
            else if (this.active && base.data.isGrounded && !base.data.playerActions.Down.IsPressed && this.HeldTime() < this.minChargeTime)
            {
                this.active = false;
                this.multiplier = 1f;
                this.ResetTimer();
            }
            else if (this.active && !base.data.isGrounded)
            {
                this.active = false;
                this.multiplier = 1f;
                this.ResetTimer();
            }

            if (this.active && base.data.isGrounded && base.data.playerActions.Down.IsPressed && this.HeldTime()>=this.minChargeTime)
            {
                this.colorFlash = base.player.gameObject.GetOrAddComponent<ColorFlash>();
                this.colorFlash.SetColor(Color.white);
                float perc = 1f - this.PercCharged();
                this.colorFlash.SetDelayBetweenFlashes(perc);
                this.colorFlash.SetDuration(perc);
                this.colorFlash.SetNumberOfFlashes(int.MaxValue);
            }
            else
            {
                if (this.colorFlash != null) { this.colorFlash.Destroy(); }
            }
        }
        public override void OnOnDisable()
        {
            // if the player is dead, this is no longer active
            this.active = false;
            base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = true;
        }
        public override void OnOnDestroy()
        {
            base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = true;
        }
        private void ResetTimer()
        {
            this.startTime = Time.time;
        }
        public void AddToJumpMultiplier(float add)
        {
            this.maxMultiplier += add;
        }
        public void AddToTimeMultiplier(float add)
        {
            this.timeMultiplier += add;
            this.timeMultiplier = UnityEngine.Mathf.Clamp(this.timeMultiplier, 1f, this.chargeTime);
        }
        public void AddToBlocks(int add)
        {
            this.numberOfBlocks += add;
        }
        private float PercCharged()
        {
            return this.multiplier / this.maxMultiplier;
        }
        private void ResetMultiplier()
        {
            this.multiplier = 1f;
        }
        private float HeldTime()
        {
            return (Time.time - this.startTime);
        }
        private System.Collections.IEnumerator DisableTopOutOfBounds()
        {
            float startTime = Time.time;
            int k = 0;
            while (base.data.isGrounded && k < 10)
            {
                k++;
                yield return null;
            }
            while (!base.data.isGrounded && Time.time < startTime + this.outOfBoundsTime)
            {
                Vector2 vector = ModdingUtils.Extensions.OutOfBoundsHandlerExtensions.BoundsPointFromWorldPosition(this.data.GetAdditionalData().outOfBoundsHandler, data.transform.position);

                if (vector.x <= 0f || vector.x >= 1f || vector.y <= 0f)
                {
                    base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = true;
                }
                else
                {
                    base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = false;
                }

                yield return null;
            }
            base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = true;
            yield break;
        }

        private readonly int maxFramesToWait = 200;
        private System.Collections.IEnumerator BlockAtApex()
        {
            bool upOnLastFrame = true;

            int k = 0;
            while (base.data.isGrounded && k < 10)
            {
                k++;
                yield return null;
            }
            int i = 0;
            int j = 0;
            while (!base.data.isGrounded && i < this.maxFramesToWait && j < this.numberOfBlocks)
            {

                if (((Vector2)Traverse.Create(base.data.playerVel).Field("velocity").GetValue()).y > 0f)
                {
                    upOnLastFrame = true;
                }
                // block at apex
                else if (j > 0 || (upOnLastFrame && ((Vector2)Traverse.Create(base.data.playerVel).Field("velocity").GetValue()).y <= 0f))
                {
                    upOnLastFrame = false;
                    j++;
                    // force the player to block (for free)
                    base.block.CallDoBlock(true, true, BlockTrigger.BlockTriggerType.Default);
                    yield return new WaitForSecondsRealtime(0.5f/(float)this.numberOfBlocks);
                }
                else if (((Vector2)Traverse.Create(base.data.playerVel).Field("velocity").GetValue()).y <= 0f)
                {
                    upOnLastFrame = false;
                }
                yield return null;
                i++;
            }

            yield break;

        }
    }

    internal class SuperJumpBlockEffect : Block
    {

    }

}

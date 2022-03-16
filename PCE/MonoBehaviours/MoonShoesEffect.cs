using UnityEngine;
using ModdingUtils.MonoBehaviours;
using PCE.Extensions;

namespace PCE.MonoBehaviours
{
    public class MoonShoesEffect : ReversibleEffect // using ReversibleEffect just to handle cleanup/resetting
    {
        private Coroutine ooBCoroutine = null;
        internal float outOfBoundsTime = 0f;

        public override void OnAwake()
        {
            base.SetLivesToEffect(int.MaxValue);
        }
        public override void OnStart()
        {
            base.applyImmediately = false;
        }
        public override void OnOnEnable()
        {
            if (this.ooBCoroutine != null)
            {
                this.StopCoroutine(this.ooBCoroutine);
            }
            this.ooBCoroutine = this.StartCoroutine(this.DisableTopOutOfBounds());
        }
        public override void OnUpdate()
        {
            /*
            if (this.groundedLastFrame && !base.data.isGrounded && !this.blockCoroutineRunning)
            {
                this.blockCoroutine = this.StartCoroutine(this.BlockAtApex());
            }
            if (base.data.isGrounded && this.blockCoroutineRunning)
            {
                this.blockCoroutineRunning = false;
                this.StopCoroutine(this.blockCoroutine);
            }

            this.groundedLastFrame = base.data.isGrounded;*/

        }
        public override void OnOnDisable()
        {
            if (this.ooBCoroutine != null) { this.StopCoroutine(this.ooBCoroutine); }
            base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = true;
            //if (this.blockCoroutine != null) { this.blockCoroutineRunning = false;  this.StopCoroutine(this.blockCoroutine); }
        }
        public override void OnOnDestroy()
        {
            if (this.ooBCoroutine != null) { this.StopCoroutine(this.ooBCoroutine); }
            base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = true;
            //if (this.blockCoroutine != null) { this.blockCoroutineRunning = false; this.StopCoroutine(this.blockCoroutine); }
        }

        private System.Collections.IEnumerator DisableTopOutOfBounds()
        {
            float startTime = Time.time;

            while (true)
            {

                Vector2 vector = ModdingUtils.Extensions.OutOfBoundsHandlerExtensions.BoundsPointFromWorldPosition(this.data.GetAdditionalData().outOfBoundsHandler, data.transform.position);

                if (Time.time > startTime + outOfBoundsTime || vector.x <= 0f || vector.x >= 1f || vector.y <= 0f)
                {
                    base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = true;
                }
                if (!(vector.x <= 0f || vector.x >= 1f || vector.y <= 0f) && vector.y <= 1f)
                {
                    startTime = Time.time;
                    base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = false;
                }

                yield return null;
            }
            yield break;
        }
        /*
        private readonly int maxFramesToWait = 200;
        private System.Collections.IEnumerator BlockAtApex()
        {
            this.blockCoroutineRunning = true;

            bool upOnLastFrame = true;

            int k = 0;
            while (base.data.isGrounded && k < 10)
            {
                k++;
                yield return null;
            }
            int i = 0;
            int j = 0;
            while (!base.data.isGrounded && i < this.maxFramesToWait && j < 1)
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
                    yield return new WaitForSecondsRealtime(0.5f);
                }
                else if (((Vector2)Traverse.Create(base.data.playerVel).Field("velocity").GetValue()).y <= 0f)
                {
                    upOnLastFrame = false;
                }
                yield return null;
                i++;
            }
            this.blockCoroutineRunning = false;
            yield break;

        }*/
    }
}

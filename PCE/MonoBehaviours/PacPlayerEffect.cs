using UnityEngine;
using HarmonyLib;
using ModdingUtils.MonoBehaviours;
using PCE.Extensions;
using UnboundLib;
using Photon.Pun;

namespace PCE.MonoBehaviours
{
    public class PacPlayerEffect : ReversibleEffect // using ReversibleEffect just to handle cleanup/resetting
    {
        private bool active = false;
        private bool waitX = false;
        private bool waitY = false;
        private float upper = 1f;
        private float lower = 0f;

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
        }
        private void WarpX(Vector3 pos)
        {
            bool flag = false;
            if (pos.x > upper || pos.x < lower)
            {
                flag = true;
                pos.x = upper - pos.x;
            }
            if (!flag)
            {
                this.waitX = false;
            }

            if (flag && !this.waitX && base.characterStatModifiers.GetAdditionalData().remainingWraps > 0)
            {
                int currentWraps = base.characterStatModifiers.GetAdditionalData().remainingWraps;
                if (!this.waitX) { Unbound.Instance.ExecuteAfterSeconds(0.1f, () => { this.waitX = false; base.characterStatModifiers.GetAdditionalData().remainingWraps = currentWraps - 1; }); }
                this.waitX = true;
                if (!PhotonNetwork.OfflineMode && base.GetComponent<PhotonView>().IsMine) { base.GetComponent<PhotonView>().RPC(nameof(RPCA_Teleport), RpcTarget.All, new object[] { MainCam.instance.transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(pos.x * (float)Screen.width, pos.y * (float)Screen.height, pos.z)) }); }
                else if (PhotonNetwork.OfflineMode) { RPCA_Teleport(MainCam.instance.transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(pos.x * (float)Screen.width, pos.y * (float)Screen.height, pos.z))); }
            }
        }
        private void WarpY(Vector3 pos)
        {
            bool flag = false;
            if (pos.y > upper || pos.y < lower)
            {
                flag = true;
                pos.y = upper - pos.y;
            }
            if (!flag)
            {
                this.waitY = false;
            }

            if (flag && !this.waitY && base.characterStatModifiers.GetAdditionalData().remainingWraps > 0)
            {
                int currentWraps = base.characterStatModifiers.GetAdditionalData().remainingWraps;
                if (!this.waitY) { Unbound.Instance.ExecuteAfterSeconds(0.1f, () => { this.waitY = false;  base.characterStatModifiers.GetAdditionalData().remainingWraps = currentWraps - 1; }); }
                this.waitY = true;
                if (!PhotonNetwork.OfflineMode && base.GetComponent<PhotonView>().IsMine) { base.GetComponent<PhotonView>().RPC(nameof(RPCA_Teleport), RpcTarget.All, new object[] { MainCam.instance.transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(pos.x * (float)Screen.width, pos.y * (float)Screen.height, pos.z)) }); }
                else if (PhotonNetwork.OfflineMode) { RPCA_Teleport(MainCam.instance.transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(pos.x * (float)Screen.width, pos.y * (float)Screen.height, pos.z))); }
            }
        }
        public override void OnUpdate()
        {
            if (!PlayerStatus.PlayerAliveAndSimulated(base.player) && base.characterStatModifiers.GetAdditionalData().wraps > 0 && !this.active)
            {
                Unbound.Instance.ExecuteAfterSeconds(0.5f, () =>
                {
                    base.characterStatModifiers.GetAdditionalData().remainingWraps = base.characterStatModifiers.GetAdditionalData().wraps;
                    base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = false;
                    this.active = true;
                    this.waitX = false;
                    this.waitY = false;
                });
                return;
            }

            if (this.active && !PlayerStatus.PlayerAliveAndSimulated(base.player))
            {
                return;
            }

            if (!this.active)
            {
                return;
            }
            else if (PlayerStatus.PlayerAliveAndSimulated(base.player) && !(this.waitX || this.waitY) && this.active && base.characterStatModifiers.GetAdditionalData().remainingWraps <= 0)
            {
                Unbound.Instance.ExecuteAfterSeconds(0.1f, () => { base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = true; });
                this.active = false;
                return;
            }
            else if (this.active && base.characterStatModifiers.GetAdditionalData().remainingWraps >0)
            {
                base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = false;
            }

            Vector3 pos = MainCam.instance.transform.GetComponent<Camera>().WorldToScreenPoint(new Vector3(data.transform.position.x, data.transform.position.y, 0f));

            pos.x /= (float)Screen.width;
            pos.y /= (float)Screen.height;

            if (!this.waitX)
            {
                WarpX(pos);
            }
            if (!this.waitY)
            {
                WarpY(pos);
            }




        }
        public void PlayParts()
        {
            PlayerJump playerJump = this.player.GetComponent<PlayerJump>();
            if (playerJump != null)
            {
                for (int i = 0; i < playerJump.jumpPart.Length; i++)
                {
                    playerJump.jumpPart[i].transform.position = this.player.transform.position;
                    playerJump.jumpPart[i].transform.rotation = Quaternion.LookRotation(new Vector3(0f, 1f, 0f));
                    playerJump.jumpPart[i].Play();
                }
            }
        }
        [PunRPC]
        public void RPCA_Teleport(Vector3 pos)
        {
            this.PlayParts();
            base.player.GetComponentInParent<PlayerCollision>().IgnoreWallForFrames(2);
            base.player.transform.position = pos;
            this.PlayParts();
        }
        public override void OnOnDisable()
        {
            // if the player is dead, this is no longer active
            base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = true;
            this.active = false;
        }
        public override void OnOnDestroy()
        {
            base.player.data.GetAdditionalData().outOfBoundsHandler.enabled = true;
            this.active = false;
        }
    }
}

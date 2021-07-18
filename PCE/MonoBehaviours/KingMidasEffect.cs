using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using UnityEngine;
using System.Linq;
using HarmonyLib;
using PCE.Extensions;
using UnboundLib.Networking;
using Photon.Pun;
using System.Reflection;

namespace PCE.MonoBehaviours
{
    public class KingMidasEffect : MonoBehaviour
    {
        private Player player;

        private readonly float range = 1.75f;

        void Awake()
        {
            this.player = this.gameObject.GetComponent<Player>();
        }

        void Start()
        {

        }

        void Update()
        {
            // if any player (friendlies included) is touched (i.e. within a very small range) turn them into gold
            if (PlayerStatus.PlayerAliveAndSimulated(this.player))
            {
                // get all alive players that are not this player
                List<Player> otherPlayers = PlayerManager.instance.players.Where(player => PlayerStatus.PlayerAliveAndSimulated(player) && (player.playerID != this.player.playerID)).ToList();

                Vector2 displacement;

                foreach (Player otherPlayer in otherPlayers)
                {
                    displacement = otherPlayer.transform.position - this.player.transform.position;
                    if (displacement.magnitude <= this.range)
                    {
                        // if the other player is within range, then add the gold effect to them

                        // locally
                        if (PhotonNetwork.OfflineMode)
                        {
                            otherPlayer.gameObject.GetOrAddComponent<GoldEffect>();
                        }
                        // via network
                        else if (this.player.GetComponent<PhotonView>().IsMine)
                        {
                            NetworkingManager.RPC(typeof(KingMidasEffect), "RPCA_TurnGold", new object[] { otherPlayer.data.view.ControllerActorNr });
                        }
                    }

                }
            }

        }
        public void OnDestroy()
        {
        }
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }

        [UnboundRPC]
        private static void RPCA_TurnGold(int actorID)
        {
            Player playerToEffect = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithActorID",
                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                    BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { actorID });

            playerToEffect.gameObject.GetOrAddComponent<GoldEffect>();
        }

    }

    public class GoldEffect : ReversibleEffect //this is a separate effect just for bookkeeping which players are gold
    {
        private readonly float movementSpeedReduction = 0.5f;
        private readonly float jumpReduction = 0.25f;
        private readonly Color color = Color.yellow;
        private ReversibleColorEffect colorEffect = null;

        public override void OnOnEnable()
        {
            if (this.colorEffect != null) { this.colorEffect.Destroy(); }
        }
        public override void OnStart()
        {
            base.characterStatModifiersModifier.movementSpeed_mult = (1f - this.movementSpeedReduction);
            base.characterStatModifiersModifier.jump_mult = (1f - this.jumpReduction);

            this.colorEffect = base.player.gameObject.AddComponent<ReversibleColorEffect>();
            this.colorEffect.SetColor(this.color);
            this.colorEffect.SetLivesToEffect(1);
        }
        public override void OnOnDisable()
        {
            if (this.colorEffect != null) { this.colorEffect.Destroy(); }
        }
        public override void OnOnDestroy()
        {
            if (this.colorEffect != null) { this.colorEffect.Destroy(); }
        }
    }
}

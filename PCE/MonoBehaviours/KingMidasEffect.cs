using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib;
using UnityEngine;
using System.Linq;
using HarmonyLib;
using PCE.Extensions;

namespace PCE.MonoBehaviours
{
    public class KingMidasEffect : MonoBehaviour
    {
        private Player playerToModify;
        private CharacterStatModifiers charStatsToModify;

        private readonly float range = 1.75f;

        void Awake()
        {
            this.playerToModify = this.gameObject.GetComponent<Player>();
            this.charStatsToModify = this.gameObject.GetComponent<CharacterStatModifiers>();
        }

        void Start()
        {

        }

        void Update()
        {
            // if any player (friendlies included) is touched (i.e. within a very small range) turn them into gold
            if (PlayerStatus.PlayerAliveAndSimulated(this.playerToModify))
            {
                // get all alive players that are not this player
                List<Player> otherPlayers = PlayerManager.instance.players.Where(player => PlayerStatus.PlayerAliveAndSimulated(player) && (player.playerID != this.playerToModify.playerID)).ToList();

                Vector2 displacement;

                foreach (Player otherPlayer in otherPlayers)
                {
                    displacement = otherPlayer.transform.position - this.playerToModify.transform.position;
                    if (displacement.magnitude <= this.range)
                    {
                        // if the other player is within range, then add the gold effect to them
                        otherPlayer.gameObject.GetOrAddComponent<GoldEffect>();
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

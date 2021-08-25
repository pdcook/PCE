using System;
using UnityEngine;
using Photon.Pun;
using PCE.Extensions;

namespace PCE.MonoBehaviours
{

    public class PopEffect : MonoBehaviour
    {
        private float period;
        private float spacing;
		private Player player;
        private float startTime;
        private float currentDuration;

        private readonly int maxAttemps = 100;

        private readonly System.Random rng = new System.Random();

        void Awake()
        {
            this.player = this.gameObject.GetComponent<Player>();
        }

        void Start()
        {
            this.ResetTimer();
            this.GetNewDuration();

        }

        void Update()
        {
            // if the player is alive and enough time has passed
            if (PlayerStatus.PlayerAliveAndSimulated(this.player) && Time.time >= this.startTime + this.currentDuration)
            {
                int i = 0;
                Player otherPlayer = PlayerManager.instance.players[rng.Next(0, PlayerManager.instance.players.Count)];

                // while the other player isn't alive or is the current player
                while ((!PlayerStatus.PlayerAliveAndSimulated(otherPlayer) || otherPlayer.playerID == this.player.playerID) && i < this.maxAttemps)
                {
                    otherPlayer = PlayerManager.instance.players[rng.Next(0, PlayerManager.instance.players.Count)];
                    i++;
                }

                
                float rangle = (float)(rng.NextDouble() * 2 * Math.PI);
                float rradius = spacing * (float)rng.NextGaussianDouble();

                //Player otherPlayer = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithID", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { otherPlayerID });

                Vector3 newPos = otherPlayer.transform.position + new Vector3(rradius * (float)Math.Cos(rangle), rradius * (float)Math.Sin(rangle), 0f);

                if (PhotonNetwork.OfflineMode)
                {
                    this.PlayParts();
                    this.player.transform.position = newPos;
                    this.PlayParts();

                }
                else
                {
                    // teleport player with RPC

                    Player[] array = new Player[] { player };
                    int[] array2 = new int[array.Length];

                    for (int j = 0; j < array.Length; j++)
                    {
                        array2[j] = array[j].data.view.ControllerActorNr;
                    }
                    if (base.GetComponent<PhotonView>().IsMine)
                    {

                        base.GetComponent<PhotonView>().RPC("RPCA_Teleport", RpcTarget.All, new object[] { newPos });

                    }
                }
                
                this.ResetTimer();
                this.GetNewDuration();
            }



        }
        public void OnDestroy()
        {
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }
        public void ResetTimer()
        {
            this.startTime = Time.time;
        }
        public void GetNewDuration()
        {
            this.currentDuration = this.period*(float)rng.NextDouble()+this.period/2;
        }

        public void SetSpacing(float spacing)
        {
            this.spacing = spacing;
        }
        public void SetPeriod(float period)
        {
            this.period = period;
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
            this.player.transform.position = pos;
            this.PlayParts();
        }

    }
}

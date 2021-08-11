using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using System.Reflection;
using HarmonyLib;

namespace PCE.Extensions
{
    public static class PlayerStatus
    {
        public static bool PlayerAlive(Player player)
        {
            return !player.data.dead;
        }
        public static bool PlayerSimulated(Player player)
        {
            return (bool)Traverse.Create(player.data.playerVel).Field("simulated").GetValue();
        }
        public static bool PlayerAliveAndSimulated(Player player)
        {
            return (PlayerStatus.PlayerAlive(player) && PlayerStatus.PlayerSimulated(player));
        }
        public static int GetNumberOfEnemyPlayers(Player player)
        {
            int num = 0;
            foreach (Player other_player in PlayerManager.instance.players)
            {
                if (other_player.teamID != player.teamID)
                {
                    num++;
                }
            }
            return num;
        }
    }
}

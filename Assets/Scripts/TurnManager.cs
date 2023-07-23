using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
    public static class TurnManager
    {
        private readonly static List<Player> players = new();

        private static int currentPlayerIndex = 0;

        public static IReadOnlyList<Player> Players => players.AsReadOnly();
        public static Player CurrentPlayer { get; private set; }
    
        public static void AddPlayer(Player player) =>
            players.Add(player);

        public static void Next()
        {
            currentPlayerIndex = ++currentPlayerIndex % players.Count;
            CurrentPlayer = players[currentPlayerIndex];
        }
    }
}
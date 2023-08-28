using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
    public static class TurnManager
    {
        private readonly static List<IPlayer> players = new();

        private static int currentPlayerIndex = 0;

        public static IReadOnlyList<IPlayer> Players => players.AsReadOnly();
        public static IPlayer CurrentPlayer { get; private set; }
    
        public static void AddPlayer(IPlayer player) =>
            players.Add(player);

        public static void Next()
        {
            currentPlayerIndex = ++currentPlayerIndex % players.Count;
            CurrentPlayer = players[currentPlayerIndex];
        }
    }
}
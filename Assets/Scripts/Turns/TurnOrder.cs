using System.Collections;
using System.Collections.Generic;

namespace LittleKingdom
{
    public class TurnOrder : IEnumerator<IPlayer>
    {
        private readonly List<IPlayer> players = new();

        private int playerIndex = -1;

        public IPlayer Current { get; private set; }

        object IEnumerator.Current => Current;

        public int Count => players.Count;

        public void AddPlayer(IPlayer player) =>
            players.Add(player);

        public bool MoveNext()
        {
            playerIndex++;
            if (playerIndex < 0 || playerIndex >= players.Count)
            {
                Current = null;
                return false;
            }

            Current = players[playerIndex];
            return true;
        }

        public void Reset()
        {
            playerIndex = -1;
            Current = null;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
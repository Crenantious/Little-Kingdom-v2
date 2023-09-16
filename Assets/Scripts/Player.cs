using UnityEngine;

namespace LittleKingdom
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private Town town;

        public ITown Town => town;

        public int Number { get; private set; }

        public void Initialise(int creationIndex) =>
            Number = creationIndex;
    }
}
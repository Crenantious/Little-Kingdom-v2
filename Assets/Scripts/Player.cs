using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private Town town;

        public ITown Town => town;

        public int Number { get; private set; }

        public Resources.Resources Resources { get; private set; } = new();

        public List<IPowerCard> OffensiveCards { get; private set; } = new();

        public List<IPowerCard> DefensiveCards { get; private set; } = new();

        public List<IPowerCard> UtilityCards { get; private set; } = new();

        public void Initialise(int creationIndex) =>
            Number = creationIndex;
    }
}
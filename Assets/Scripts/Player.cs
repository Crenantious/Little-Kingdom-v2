using LittleKingdom.CharacterTurns;
using LittleKingdom.Factories;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private Town town;
        private CharacterTurnFactory turnFactory;

        public ITown Town => town;

        public string Name { get; private set; }
        public int Number { get; private set; }

        public Resources.Resources Resources { get; private set; } = new();

        public List<IPowerCard> OffensiveCards { get; private set; } = new();

        public List<IPowerCard> DefensiveCards { get; private set; } = new();

        public List<IPowerCard> UtilityCards { get; private set; } = new();

        public ICharacterTurn Turn { get; private set; }


        [Inject]
        public void Construct(CharacterTurnFactory turnFactory) =>
            this.turnFactory = turnFactory;

        public void Initialise(int creationIndex, string name)
        {
            Number = creationIndex;
            Turn = turnFactory.Create(this);
            Name = name;
        }
    }
}
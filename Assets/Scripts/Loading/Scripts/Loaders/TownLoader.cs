using LittleKingdom.CharacterTurns;
using LittleKingdom.Factories;
using System;
using Zenject;

namespace LittleKingdom.Loading
{
    public class TownLoader : Loader<TownLC>
    {
        private TownPlacementFactory townPlacementFactory;

        private ITownPlacement townPlacement;
        private int currentPlayerIndex = 0;
        private CharacterTurnOrder turnOrder;

        [Inject]
        public void Construct(TownPlacementFactory townPlacementFactory, CharacterTurnOrder turnOrder)
        {
            this.townPlacementFactory = townPlacementFactory;
            this.turnOrder = turnOrder;
            AddDependency<BoardLoader>();
        }

        public override void Load(TownLC config)
        {
            townPlacement = townPlacementFactory.Create();
            townPlacement.TownPlaced += OnTownPlaced;
            BeginNextPlacement();
        }

        private void OnTownPlaced(ITown town) =>
            BeginNextPlacement();

        private void BeginNextPlacement()
        {
            if (turnOrder.MoveNext() is false)
            {
                townPlacement.TownPlaced -= OnTownPlaced;
                return;
            }

            townPlacement.Place(turnOrder.Current.Town);
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }
    }
}
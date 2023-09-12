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

        [Inject]
        public void Construct(TownPlacementFactory townPlacementFactory)
        {
            this.townPlacementFactory = townPlacementFactory;
            AddDependency<BoardLoader>();
        }

        public override void Load(TownLC config)
        {
            townPlacement = townPlacementFactory.Create();
            if (TurnManager.Players.Count <= 0)
                return;

            townPlacement.TownPlaced += OnTownPlaced;
            BeginNextPlacement();
        }

        private void OnTownPlaced(ITown town) =>
            BeginNextPlacement();

        private void BeginNextPlacement()
        {
            if (currentPlayerIndex >= TurnManager.Players.Count)
            {
                townPlacement.TownPlaced -= OnTownPlaced;
                return;
            }

            townPlacement.BeginPlacement(TurnManager.Players[currentPlayerIndex++].Town);
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }
    }
}
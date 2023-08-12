using LittleKingdom.Events;
using LittleKingdom.Factories;
using LittleKingdom.Loading;
using System;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class TownLoader : Loader<TownLC>
    {
        [SerializeField] private BoardLoader boardLoader;

        private TownPlacementFactory townPlacementFactory;

        private ITownPlacement townPlacement;
        private int currentPlayerIndex = 0;

        [Inject]
        public void Construct(TownPlacementFactory townPlacementFactory) =>
            this.townPlacementFactory = townPlacementFactory;

        private void Awake() =>
            Dependencies.Add(boardLoader);

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

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}
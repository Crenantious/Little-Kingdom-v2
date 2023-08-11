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
        private TownPlacedEvent townPlacedEvent;

        private ITownPlacement townPlacement;
        private int currentPlayerIndex = 0;

        [Inject]
        public void Construct(TownPlacementFactory townPlacementFactory, TownPlacedEvent townPlacedEvent)
        {
            this.townPlacementFactory = townPlacementFactory;
            this.townPlacedEvent = townPlacedEvent;
        }

        private void Awake() =>
            Dependencies.Add(boardLoader);

        public override void Load(TownLC config)
        {
            townPlacement = townPlacementFactory.Create();
            if (TurnManager.Players.Count <= 0)
                return;

            townPlacedEvent.Subscribe(BeginNextPlacement);
            BeginNextPlacement(new(TurnManager.Players[0].Town));
        }

        private void BeginNextPlacement(TownPlacedEvent.EventData eventData)
        {
            if (currentPlayerIndex >= TurnManager.Players.Count)
            {
                townPlacedEvent.Unsubscribe(BeginNextPlacement);
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
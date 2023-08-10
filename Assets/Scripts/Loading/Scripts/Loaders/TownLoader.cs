using LittleKingdom.Loading;
using UnityEngine;
using System;
using Zenject;
using LittleKingdom.Factories;

namespace LittleKingdom
{
    public class TownLoader : Loader<TownLC>
    {
        [SerializeField] private BoardLoader boardLoader;
        private ITownPlacement townPlacement;
        private TownPlacementFactory townPlacementFactory;

        [Inject]
        public void Construct(TownPlacementFactory townPlacementFactory) =>
            this.townPlacementFactory = townPlacementFactory;

        private void Awake() =>
            Dependencies.Add(boardLoader);

        public override void Load(TownLC config)
        {
            townPlacement = townPlacementFactory.Create();
            //TODO: JR - place one town after another.
            foreach (Player player in TurnManager.Players)
            {
                townPlacement.BeginPlacement(player.Town);
            }
        }

        public override void Unload()
        {
            throw new NotImplementedException();
        }
    }
}
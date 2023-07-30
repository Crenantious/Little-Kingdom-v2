using LittleKingdom.Board;
using LittleKingdom.Input;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class TownPlacement : IUpdatable
    {
        private const float ManualUpdateDelay = 0.5f;

        private readonly InGameInput inGameInput;
        private readonly MonoSimulator monoSimulator;

        private Town town;

        public TownPlacement(InGameInput inGameInput, MonoSimulator monoSimulator)
        {
            this.inGameInput = inGameInput;
            this.monoSimulator = monoSimulator;
        }

        public void PlaceManually(Town town)
        {
            this.town = town;
            monoSimulator.RegisterForUpdate(this, ManualUpdateDelay);
        }

        public void Update()
        {
            Tile originTile = GetTownOriginTile();
            MoveTownToTile(town, originTile);
        }

        public void PlaceAutomatically(Town town)
        {
            throw new NotImplementedException();
        }

        private Tile GetTownOriginTile()
        {
            throw new NotImplementedException();
        }

        private void MoveTownToTile(Town town, Tile origin)
        {
            throw new NotImplementedException();
        }
    }
}
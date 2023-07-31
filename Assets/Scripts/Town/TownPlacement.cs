using LittleKingdom.Board;
using LittleKingdom.Extensions;
using LittleKingdom.Input;
using System;
using UnityEngine;

namespace LittleKingdom
{
    public class TownPlacement : IUpdatable
    {
        private const float ManualUpdateDelay = 0.5f;

        private readonly InputUtility inputUtility;
        private readonly InGameInput inGameInput;
        private readonly MonoSimulator monoSimulator;
        private readonly BoardMono board;

        private Town town;
        private bool isPlacing;

        public TownPlacement(InputUtility inputUtility, InGameInput inGameInput, MonoSimulator monoSimulator, BoardMono board)
        {
            this.inGameInput = inGameInput;
            this.monoSimulator = monoSimulator;
            this.inputUtility = inputUtility;
            this.board = board;
        }

        public void PlaceManually(Town town)
        {
            if (isPlacing)
            {
                // TODO: JR - log properly.
                Debug.Log("Already placing a town.");
                return;
            }

            isPlacing = true;
            this.town = town;
            monoSimulator.RegisterForUpdate(this, ManualUpdateDelay);
        }

        public void Update()
        {
            TileMono originTile = GetTownOriginTile();
            MoveTownToTile(town, originTile);
        }

        public void PlaceAutomatically(Town town)
        {
            throw new NotImplementedException();
        }

        private TileMono GetTownOriginTile()
        {
            inGameInput.Enable();
            if (inputUtility.RaycastFromPointer(inGameInput.GetPointerPosition(), out RaycastHit hit) is false)
                return board.Tiles.GetNearestElement(new Vector2());

            return board.Tiles.GetNearestElement(hit.point);
        }

        private void MoveTownToTile(Town town, TileMono origin)
        {
            float xOffset = (town.Width) * board.TileWidth;
            float zOffset = (town.Height) * board.TileHeight;
            town.transform.position = origin.transform.position + new Vector3(xOffset, 0, zOffset);
        }
    }
}
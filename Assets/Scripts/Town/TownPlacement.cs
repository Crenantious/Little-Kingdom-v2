using LittleKingdom.Board;
using LittleKingdom.Extensions;
using LittleKingdom.Input;
using System;
using UnityEngine;

namespace LittleKingdom
{
    public class TownPlacement : IUpdatable
    {
        private const float ManualUpdateDelay = 0.05f;

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

        /// <summary>
        /// Place the town based on where the user specifies.
        /// </summary>
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

        /// <summary>
        /// Place the town based on an algorithm.
        /// </summary>
        public void PlaceAutomatically(Town town)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Update()
        {
            TileMono originTile = GetTownOriginTile();
            MoveTownToTile(town, originTile);
        }

        private TileMono GetTownOriginTile()
        {
            // TODO: JR - put this in a standardised place.
            inGameInput.Enable();

            // If true, the position is increased by half a tile since the grid expects the pivot point of the tiles to be the bottom left.
            Vector2 position = inputUtility.RaycastFromPointer(inGameInput.GetPointerPosition(), out RaycastHit hit) ?
                new Vector2(hit.point.x + board.TileWidth / 2, hit.point.z + board.TileHeight / 2) :
                new Vector2();
            return board.Tiles.GetNearestElement(position);
        }

        private void MoveTownToTile(Town town, TileMono origin)
        {
            float xOffset = (town.Width) * board.TileWidth;
            float zOffset = (town.Height) * board.TileHeight;
            town.transform.position = origin.transform.position + new Vector3(xOffset, 0, zOffset);
        }
    }
}
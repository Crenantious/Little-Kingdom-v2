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

            Vector2 position = GetWorldspacePointerPosition();
            return GetTileFromPointerPosition(position);
        }

        private Vector2 GetWorldspacePointerPosition() =>
            // If true, the position is increased by half a tile since the grid expects the pivot point of the tiles to be the bottom left.
            inputUtility.RaycastFromPointer(inGameInput.GetPointerPosition(), out RaycastHit hit) ?
                new Vector2(hit.point.x + board.TileWidth / 2, hit.point.z + board.TileHeight / 2) :
                new Vector2();

        private TileMono GetTileFromPointerPosition(Vector2 position)
        {
            (int column, int row) = board.Tiles.GetNearestIndex(position);
            column = (int)Mathf.Clamp(column, 0, board.Tiles.Width - 1 - MathF.Ceiling((float)town.Width / 2));
            row = (int)Mathf.Clamp(row, MathF.Ceiling((float)town.Height / 2), board.Tiles.Height - 1);
            return board.Tiles.Get(column, row);
        }

        private void MoveTownToTile(Town town, TileMono origin)
        {
            float xOffset = MathF.Ceiling((float)town.Width / 2) * (board.TileWidth / 2);
            float zOffset = -MathF.Ceiling((float)town.Height / 2) * (board.TileHeight / 2);
            town.transform.position = origin.transform.position + new Vector3(xOffset, 0, zOffset);
        }
    }
}
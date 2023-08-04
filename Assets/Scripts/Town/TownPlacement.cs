using LittleKingdom.Board;
using LittleKingdom.Events;
using LittleKingdom.Extensions;
using LittleKingdom.Input;
using LittleKingdom.UI;
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
        private readonly DialogBox dialogBox;
        private readonly TownPlacedEvent townPlacedEvent;

        private Town town;
        private bool isPlacing;
        private bool isConfirmingPlacement;

        public TownPlacement(InputUtility inputUtility, InGameInput inGameInput, MonoSimulator monoSimulator,
            BoardMono board, DialogBox dialogBox, TownPlacedEvent townPlacedEvent)
        {
            this.inputUtility = inputUtility;
            this.inGameInput = inGameInput;
            this.monoSimulator = monoSimulator;
            this.board = board;
            this.dialogBox = dialogBox;
            this.townPlacedEvent = townPlacedEvent;
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
            inGameInput.PointerTap += ConfirmPlacement;
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
            if (isConfirmingPlacement)
                return;

            TileMono originTile = GetTownOriginTile();
            MoveTownToTile(town, originTile);
        }

        private TileMono GetTownOriginTile()
        {
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

            float maxColumnIndex = (board.Tiles.Width - 1) - MathF.Ceiling((float)town.Width / 2);
            float minRowIndex = MathF.Ceiling((float)town.Height / 2);

            column = (int)Mathf.Clamp(column, 0, maxColumnIndex);
            row = (int)Mathf.Clamp(row, minRowIndex, board.Tiles.Height - 1);

            return board.Tiles.Get(column, row);
        }

        private void MoveTownToTile(Town town, TileMono origin)
        {
            float xOffset = MathF.Ceiling((float)town.Width / 2) * (board.TileWidth / 2);
            float zOffset = -MathF.Ceiling((float)town.Height / 2) * (board.TileHeight / 2);
            town.transform.position = origin.transform.position + new Vector3(xOffset, 0, zOffset);
            town.OriginTile = origin;
        }

        private void ConfirmPlacement()
        {
            isConfirmingPlacement = true;
            dialogBox.Open("Place town here?", ("Yes", OnPlacementConfirmed), ("No", OnPlacementRejected));
        }

        private void OnPlacementConfirmed(string option)
        {
            isConfirmingPlacement = false;
            SetTileValues();
            townPlacedEvent.Invoke(new TownPlacedEvent.EventData(town));
        }

        private void SetTileValues()
        {
            for (int column = town.OriginTile.Column; column < town.OriginTile.Column + town.Width - 1; column++)
            {
                for (int row = town.OriginTile.Row; row < town.OriginTile.Row + town.Height - 1; row++)
                {
                    Tile tile = board.Tiles.Get(column, row).Tile;
                    tile.Town = town;
                    town.Tiles.Set(column, row, tile);
                }
            }
        }

        private void OnPlacementRejected(string option)
        {
            isConfirmingPlacement = false;
        }
    }
}
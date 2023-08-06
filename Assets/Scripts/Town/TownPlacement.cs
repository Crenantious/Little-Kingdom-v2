using LittleKingdom.Board;
using LittleKingdom.Events;
using LittleKingdom.Extensions;
using LittleKingdom.Input;
using LittleKingdom.UI;
using System;
using UnityEngine;

namespace LittleKingdom
{
    public class TownPlacement : IUpdatable, ITownPlacement
    {
        private const float ManualUpdateDelay = 0.05f;

        #region DI

        private readonly InputUtility inputUtility;
        private readonly InGameInput inGameInput;
        private readonly MonoSimulator monoSimulator;
        private readonly BoardMono board;
        private readonly DialogBox dialogBox;
        private readonly TownPlacedEvent townPlacedEvent;
        private readonly TileEntityAssignment tileEntityAssignment;

        #endregion

        private readonly Vector2 defaultPointerWorldPosition = new();

        private Town town;
        private bool isPlacing;
        private bool isConfirmingPlacement;

        public TownPlacement(InputUtility inputUtility, InGameInput inGameInput, MonoSimulator monoSimulator,
            BoardMono board, DialogBox dialogBox, TownPlacedEvent townPlacedEvent, TileEntityAssignment tileEntityAssignment)
        {
            this.inputUtility = inputUtility;
            this.inGameInput = inGameInput;
            this.monoSimulator = monoSimulator;
            this.board = board;
            this.dialogBox = dialogBox;
            this.townPlacedEvent = townPlacedEvent;
            this.tileEntityAssignment = tileEntityAssignment;
        }

        /// <summary>
        /// Place the town based on where the user specifies.
        /// </summary>
        public void Place(Town town)
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

        private TileMono GetTownOriginTile() =>
            board.GetTownOriginFromPointerPosition(town, GetWorldspacePointerPosition());

        private Vector2 GetWorldspacePointerPosition() =>
            // If true, the position is increased by half a tile since the grid expects the pivot point of the tiles to be the bottom left.
            inputUtility.RaycastFromPointer(inGameInput.GetPointerPosition(), out RaycastHit hit) ?
                new Vector2(hit.point.x + board.TileWidth / 2, hit.point.z + board.TileHeight / 2) :
                defaultPointerWorldPosition;

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
            tileEntityAssignment.AssignTown(town);
            townPlacedEvent.Invoke(new TownPlacedEvent.EventData(town));
        }

        private void OnPlacementRejected(string option)
        {
            isConfirmingPlacement = false;
        }
    }
}
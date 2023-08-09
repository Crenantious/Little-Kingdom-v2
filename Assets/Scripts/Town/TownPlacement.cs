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
        private readonly IBoard board;
        private readonly DialogBox dialogBox;
        private readonly TownPlacedEvent townPlacedEvent;
        private readonly TileEntityAssignment tileEntityAssignment;

        #endregion

        private readonly Vector2 defaultPointerWorldPosition = new();

        private Town town;
        private bool isPlacing;
        private bool isConfirmingPlacement;

        public TownPlacement(InputUtility inputUtility, InGameInput inGameInput, MonoSimulator monoSimulator,
            IBoard board, DialogBox dialogBox, TownPlacedEvent townPlacedEvent, TileEntityAssignment tileEntityAssignment)
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

        /// <inheritdoc/>
        public void Update()
        {
            if (isConfirmingPlacement)
                return;

            ITile originTile = GetTownOriginTile();
            MoveTownToTile(town, originTile);
        }

        private ITile GetTownOriginTile() =>
            board.GetTownOriginFromPointerPosition(town, GetWorldspacePointerPosition());

        private Vector2 GetWorldspacePointerPosition() =>
            // If true, the position is increased by half a tile since the grid expects the pivot point of the tiles to be the bottom left.
            inputUtility.RaycastFromPointer(inGameInput.GetPointerPosition(), out RaycastHit hit) ?
                new Vector2(hit.point.x + References.TileWidth / 2, hit.point.z + References.TileHeight / 2) :
                defaultPointerWorldPosition;

        private void MoveTownToTile(Town town, ITile origin)
        {
            float xOffset = MathF.Ceiling((float)town.Width / 2) * (References.TileWidth / 2);
            float zOffset = -MathF.Ceiling((float)town.Height / 2) * (References.TileHeight / 2);
            town.transform.position = new Vector3(origin.XPosition + xOffset, 0, origin.YPosition + zOffset);
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
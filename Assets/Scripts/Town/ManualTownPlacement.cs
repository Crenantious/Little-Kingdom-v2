using LittleKingdom.Board;
using LittleKingdom.Events;
using LittleKingdom.Extensions;
using LittleKingdom.Input;
using LittleKingdom.UI;
using System;
using UnityEngine;

namespace LittleKingdom
{
    public class ManualTownPlacement : IUpdatable, ITownPlacement
    {
        private const float ManualUpdateDelay = 0.05f;

        #region DI

        private readonly InGameInput inGameInput;
        private readonly MonoSimulator monoSimulator;
        private readonly DialogBox dialogBox;
        private readonly TownPlacedEvent townPlacedEvent;
        private readonly TileEntityAssignment tileEntityAssignment;
        private readonly TownPlacementUtilities townPlacementUtilities;

        #endregion

        private Town town;
        private ITile originTile;
        private bool isPlacing;
        private bool isConfirmingPlacement;

        public ManualTownPlacement(InGameInput inGameInput, MonoSimulator monoSimulator,
            DialogBox dialogBox, TownPlacedEvent townPlacedEvent, TileEntityAssignment tileEntityAssignment,
            TownPlacementUtilities townPlacementUtilities)
        {
            this.inGameInput = inGameInput;
            this.monoSimulator = monoSimulator;
            this.dialogBox = dialogBox;
            this.townPlacedEvent = townPlacedEvent;
            this.tileEntityAssignment = tileEntityAssignment;
            this.townPlacementUtilities = townPlacementUtilities;
        }

        /// <summary>
        /// Place the town based on where the user specifies.
        /// </summary>
        public void BeginPlacement(Town town)
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

        public void FinalisePlacement()
        {
            isConfirmingPlacement = false;
            tileEntityAssignment.AssignTown(town, originTile);
            townPlacedEvent.Invoke(new TownPlacedEvent.EventData(town));
        }

        /// <inheritdoc/>
        public void Update()
        {
            if (isConfirmingPlacement)
                return;

            ITile newOriginTile = townPlacementUtilities.GetTownOriginTile(town);
            if (originTile != newOriginTile)
            {
                originTile = newOriginTile;
                townPlacementUtilities.MoveTownToTile(town, originTile);
            }
        }

        private void ConfirmPlacement()
        {
            isConfirmingPlacement = true;
            dialogBox.Open("Place town here?", ("Yes", (o) => FinalisePlacement()), ("No", OnPlacementRejected));
        }

        private void OnPlacementRejected(string option)
        {
            isConfirmingPlacement = false;
        }
    }
}
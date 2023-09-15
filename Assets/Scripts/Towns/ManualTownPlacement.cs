using LittleKingdom.Board;
using LittleKingdom.Input;
using LittleKingdom.UI;
using UnityEngine;
using Zenject;
using System;

namespace LittleKingdom
{
    public class ManualTownPlacement : ITickable, ITownPlacement
    {
        #region DI

        private readonly StandardInput inGameInput;
        private readonly DialogBox dialogBox;
        private readonly TileEntityAssignment tileEntityAssignment;
        private readonly TownPlacementUtilities townPlacementUtilities;

        #endregion

        private ITown town;
        private ITile originTile;
        private bool isPlacing;
        private bool isConfirmingPlacement;

        public event SimpleEventHandler<ITown> TownPlaced;

        public ManualTownPlacement(StandardInput inGameInput,
            DialogBox dialogBox, TileEntityAssignment tileEntityAssignment,
            TownPlacementUtilities townPlacementUtilities)
        {
            this.inGameInput = inGameInput;
            this.dialogBox = dialogBox;
            this.tileEntityAssignment = tileEntityAssignment;
            this.townPlacementUtilities = townPlacementUtilities;
        }

        public void BeginPlacement(ITown town)
        {
            if (isPlacing)
            {
                // TODO: JR - log properly.
                Debug.LogWarning("Already placing a town.");
                return;
            }

            isPlacing = true;
            this.town = town;
            inGameInput.PointerPressAndRelease += ConfirmPlacement;
        }

        public void FinalisePlacement()
        {
            tileEntityAssignment.AssignTown(town, originTile);
            isPlacing = false;
            isConfirmingPlacement = false;
            TownPlaced.Invoke(town);
        }

        public void Tick()
        {
            if (isPlacing is false || isConfirmingPlacement)
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
            dialogBox.Open("Place town here?", ("Yes", FinalisePlacement), ("No", OnPlacementRejected));
        }

        private void OnPlacementRejected() =>
            isConfirmingPlacement = false;
    }
}
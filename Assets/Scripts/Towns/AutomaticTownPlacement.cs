using LittleKingdom.Board;
using LittleKingdom.Events;
using System;

namespace LittleKingdom
{
    public class AutomaticTownPlacement : ITownPlacement
    {
        #region DI

        private readonly IBoard board;
        private readonly TownPlacedEvent townPlacedEvent;
        private readonly TileEntityAssignment tileEntityAssignment;

        #endregion

        public AutomaticTownPlacement(IBoard board, TownPlacedEvent townPlacedEvent, TileEntityAssignment tileEntityAssignment)
        {
            this.board = board;
            this.townPlacedEvent = townPlacedEvent;
            this.tileEntityAssignment = tileEntityAssignment;
        }

        public event SimpleEventHandler<ITown> TownPlaced;

        public void BeginPlacement(ITown town)
        {
            throw new NotImplementedException();
        }

        public void FinalisePlacement()
        {
            throw new NotImplementedException();
        }
    }
}
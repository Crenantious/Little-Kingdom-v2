using System;

namespace LittleKingdom
{
    public interface ITownPlacement
    {
        public event SimpleEventHandler<ITown> TownPlaced;

        /// <summary>
        /// Perform pre-placement logic such as getting user input or calculating placement position.
        /// </summary>
        public void BeginPlacement(ITown town);

        public void FinalisePlacement();
    }
}
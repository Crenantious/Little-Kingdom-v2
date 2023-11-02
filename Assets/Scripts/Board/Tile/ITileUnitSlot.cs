using LittleKingdom.Units;
using UnityEngine;

namespace LittleKingdom.Board
{
    public interface ITileUnitSlot
    {
        public ITileUnitSlot Next { get; }

        public ITileUnitSlot Previous { get; }

        public Unit Unit { get; }

        public Transform Transform { get; }

        public void ShowAvailability();

        public void HideAvailability();
    }
}
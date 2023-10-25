using LittleKingdom.Units;
using UnityEngine;

namespace LittleKingdom.Board
{
    public class TileUnitSlot : MonoBehaviour
    {
        public TileUnitSlot Next { get; set; }

        public TileUnitSlot Previous { get; set; }

        public Unit Unit { get; set; }
    }
}
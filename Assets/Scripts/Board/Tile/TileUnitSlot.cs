using LittleKingdom.Input;
using LittleKingdom.Units;
using System;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Board
{
    public class TileUnitSlot : MonoBehaviour
    {
        public TileUnitSlot Next { get; set; }

        public TileUnitSlot Previous { get; set; }

        public Unit Unit { get; set; }
    }
}
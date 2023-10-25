using LittleKingdom.Resources;
using LittleKingdom.Units;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Board
{
    public interface ITile : IProduceResources
    {
        /// <summary>
        /// The index of the column on the board that this tile is located.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// The index of the row on the board that this tile is located.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// The x position relative to the board.
        /// </summary>
        public float XPosition { get; set; }

        /// <summary>
        /// The y position relative to the board.
        /// </summary>
        public float YPosition { get; set; }

        public TileUnitSlots UnitSlots { get; }

#nullable enable
        /// <summary>
        /// The town that occupies this tile.
        /// </summary>
        public ITown? Town { get; set; }
#nullable disable

        /// <summary>
        /// Only to be called from a factory.
        /// </summary>
        public void Initialise(Resources.Resources resources);

        /// <summary>
        /// Sets <see cref="XPosition"/> and <see cref="YPosition"/>. Use this if both are needed to be set.
        /// </summary>
        public void SetPos(Vector2 position);

        public bool Add(IPlaceableInTile placeable);
        public bool Remove(IPlaceableInTile placeable);
    }
}
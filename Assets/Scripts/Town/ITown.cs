using LittleKingdom.Board;
using LittleKingdom.DataStructures;
using UnityEngine;

namespace LittleKingdom
{
    public interface ITown
    {
        /// <summary>
        /// The 2D width (board dimensions) of the town in tiles.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The 2D height (board dimensions) of the town in tiles.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// The top-left tile that town occupies.
        /// </summary>
        public ITile OriginTile { get; set; }

        /// <summary>
        /// The tiles that the town occupies.
        /// </summary>
        public Grid<ITile> Tiles { get; set; }

        /// <summary>
        /// The x position relative to the board.
        /// </summary>
        public float XPosition { get; }

        /// <summary>
        /// The y position relative to the board.
        /// </summary>
        public float YPosition { get; }

        public void SetPosition(Vector2 position);
    }
}
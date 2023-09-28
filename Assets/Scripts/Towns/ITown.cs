using LittleKingdom.Board;
using LittleKingdom.DataStructures;
using UnityEngine;

namespace LittleKingdom
{
    public interface ITown : IPlaceable
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
        /// The x position relative to the board.
        /// </summary>
        public float XPosition { get; }

        /// <summary>
        /// The y position relative to the board.
        /// </summary>
        public float YPosition { get; }

        public IPlayer Player { get; }

        public void SetPosition(Vector2 position);
    }
}
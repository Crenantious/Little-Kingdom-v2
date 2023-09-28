using UnityEngine;

namespace LittleKingdom.Board
{
    public interface ITileInfo
    {
        /// <summary>
        /// The texture to be used for the tile's material.
        /// </summary>
        public Texture Texture { get; }

        /// <summary>
        /// The resources the tile produces.
        /// </summary>
        public Resources.Resources Resources { get; }

        /// <summary>
        /// The percent of the board that will be filled with this type of tile.
        /// </summary>
        public float PercentOfBoard { get; }
    }
}
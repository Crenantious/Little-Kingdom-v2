using LittleKingdom.Board;
using LittleKingdom.DataStructures;
using UnityEngine;

namespace LittleKingdom
{
    public class Town : MonoBehaviour
    {
        /// <summary>
        /// The 2D width (board dimensions) of the town in tiles.
        /// </summary>
        [field: SerializeField] public int Width { get; private set; }

        /// <summary>
        /// The 2D height (board dimensions) of the town in tiles.
        /// </summary>
        [field: SerializeField] public int Height { get; private set; }

        /// <summary>
        /// The top-left tile that town occupies.
        /// </summary>
        public TileMono OriginTile { get; set; }

        /// <summary>
        /// The tiles that the town occupies.
        /// </summary>
        public Grid<Tile> Tiles { get; set; }
    }
}
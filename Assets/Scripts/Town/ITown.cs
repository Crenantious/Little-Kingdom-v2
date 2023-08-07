using LittleKingdom.Board;
using LittleKingdom.DataStructures;

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
        public Tile OriginTile { get; set; }

        /// <summary>
        /// The tiles that the town occupies.
        /// </summary>
        public Grid<Tile> Tiles { get; set; }
    }
}
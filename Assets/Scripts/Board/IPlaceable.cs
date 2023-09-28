using LittleKingdom.DataStructures;

namespace LittleKingdom.Board
{
    /// <summary>
    /// Used for entities that can be placed on the board.
    /// </summary>
    public interface IPlaceable
    {
        /// <summary>
        /// The top-left tile that entity occupies.
        /// </summary>
        public ITile OriginTile { get; set; }

        /// <summary>
        /// The tiles that the entity occupies.
        /// </summary>
        public Grid<ITile> Tiles { get; set; }
    }
}
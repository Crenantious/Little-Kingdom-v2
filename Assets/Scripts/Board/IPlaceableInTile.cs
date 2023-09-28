namespace LittleKingdom.Board
{
    /// <summary>
    /// Used for entities that can be placed on the board, within a tile. Such as a unit.
    /// </summary>
    public interface IPlaceableInTile
    {
        public Tile Tile { get; }
    }
}
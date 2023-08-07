using LittleKingdom.DataStructures;

namespace LittleKingdom.Board
{
    public interface IBoard
    {
        public SizedGrid<Tile> Tiles { get; }
    }
}
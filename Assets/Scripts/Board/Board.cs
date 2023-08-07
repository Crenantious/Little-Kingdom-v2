using LittleKingdom.DataStructures;

namespace LittleKingdom.Board
{
    public class Board : IBoard
    {
        public SizedGrid<Tile> Tiles { get; private set; }

        public Board(SizedGrid<Tile> tiles) => Tiles = tiles;
    }
}
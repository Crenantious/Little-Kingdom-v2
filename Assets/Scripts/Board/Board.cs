using LittleKingdom.DataStructures;

namespace LittleKingdom.Board
{
    public class Board : IBoard
    {
        public SizedGrid<ITile> Tiles { get; private set; }

        public Board(SizedGrid<ITile> tiles) => Tiles = tiles;
    }
}
using LittleKingdom.DataStructures;
using System.Collections.Generic;

namespace LittleKingdom.Board
{
    public class BoardGeneratorMono : IBoardGenerator
    {
        private readonly BoardGenerator boardGenerator;

        public BoardGeneratorMono(BoardGenerator boardGenerator) =>
            this.boardGenerator = boardGenerator;

        public IBoard Generate(int widthInTiles, int heightInTiles, IEnumerable<ITileInfo> tileInfos)
        {
            IBoard board = boardGenerator.Generate(widthInTiles, heightInTiles, tileInfos);
            PositionTiles(board.Tiles);
            return board;
        }

        private void PositionTiles(SizedGrid<ITile> tiles)
        {
            for (int i = 0; i < tiles.Width; i++)
            {
                for (int j = 0; j < tiles.Height; j++)
                {
                    tiles.Get(i, j).SetPos(new(References.TileWidth * i, References.TileHeight * j));
                }
            }
        }
    }
}
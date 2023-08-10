using LittleKingdom.DataStructures;
using System.Collections.Generic;

namespace LittleKingdom.Board
{
    public class BoardGeneratorMono : IBoardGenerator
    {
        private readonly BoardGenerator boardGenerator;
        private readonly IReferences references;

        public BoardGeneratorMono(BoardGenerator boardGenerator, IReferences references)
        {
            this.boardGenerator = boardGenerator;
            this.references = references;
        }

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
                    tiles.Get(i, j).SetPos(new(references.TileWidth * i, references.TileHeight * j));
                }
            }
        }
    }
}
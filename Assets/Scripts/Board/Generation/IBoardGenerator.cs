using System.Collections.Generic;

namespace LittleKingdom.Board
{
    public interface IBoardGenerator
    {
        public IBoard Generate(int widthInTiles, int heightInTiles, IEnumerable<TileInfo> tileInfos);
    }
}
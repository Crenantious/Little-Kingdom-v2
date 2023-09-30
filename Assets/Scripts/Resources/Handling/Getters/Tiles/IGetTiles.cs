using LittleKingdom.Board;
using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public interface IGetTiles
    {
        public IEnumerable<ITile> Get();
    }
}
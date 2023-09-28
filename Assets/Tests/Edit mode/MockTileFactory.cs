using LittleKingdom.Board;
using Moq;
using Zenject;

namespace LittleKingdom.Factories
{
    public class MockTileFactory : IFactory<ITileInfo, ITile>
    {
        public ITile Create(ITileInfo tileInfo)
        {
            Mock<ITile> tile = new();
            tile.SetupAllProperties();
            return tile.Object;
        }
    }
}
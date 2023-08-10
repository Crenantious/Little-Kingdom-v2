using LittleKingdom.Board;
using Zenject;

namespace LittleKingdom.Factories
{
    public class CustomTileFactory : IFactory<ITileInfo, ITile>
    {
        private readonly DiContainer container;

        public CustomTileFactory(DiContainer container) => this.container = container;

        public ITile Create(ITileInfo tileInfo)
        {
            Tile tile = new();
            container.Inject(tile);
            return tile;
        }
    }
}
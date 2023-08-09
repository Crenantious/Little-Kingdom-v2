using LittleKingdom.Board;
using Zenject;

namespace LittleKingdom.Factories
{
    public class CustomTileFactory : IFactory<TileInfo, ITile>
    {
        private readonly DiContainer container;

        public CustomTileFactory(DiContainer container) => this.container = container;

        public ITile Create(TileInfo tileInfo)
        {
            Tile tile = new();
            container.Inject(tile);
            return tile;
        }
    }
}
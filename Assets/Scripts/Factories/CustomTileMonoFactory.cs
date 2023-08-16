using LittleKingdom.Board;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Factories
{
    public class CustomTileMonoFactory : IFactory<ITileInfo, ITile>
    {
        private readonly DiContainer container;
        private readonly TileMono tileMono;

        public CustomTileMonoFactory(DiContainer container, TileMono tileMono)
        {
            this.container = container;
            this.tileMono = tileMono;
        }

        public ITile Create(ITileInfo tileInfo)
        {
            TileMono tile = container.InstantiatePrefabForComponent<TileMono>(tileMono);
            tile.Initialise(tileInfo.ResourceType);
            tile.GetComponent<Renderer>().material.mainTexture = tileInfo.Texture;
            return tile;
        }
    }
}
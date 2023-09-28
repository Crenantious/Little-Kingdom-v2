using LittleKingdom.Board;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Factories
{
    public class CustomTileMonoFactory : IFactory<ITileInfo, ITile>
    {
        private readonly DiContainer container;
        private readonly Tile tileMono;

        public CustomTileMonoFactory(DiContainer container, Tile tileMono)
        {
            this.container = container;
            this.tileMono = tileMono;
        }

        public ITile Create(ITileInfo tileInfo)
        {
            Tile tile = container.InstantiatePrefabForComponent<Tile>(tileMono);
            tile.Initialise(tileInfo.Resources);
            tile.GetComponent<Renderer>().material.mainTexture = tileInfo.Texture;
            return tile;
        }
    }
}
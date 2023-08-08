using LittleKingdom.Board;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Factories
{
    public class CustomTileFactory : IFactory<TileInfo, Tile>
    {
        private readonly DiContainer container;

        public CustomTileFactory(DiContainer container) => this.container = container;

        public Tile Create(TileInfo tileInfo)
        {
            Tile tile = container.InstantiatePrefabForComponent<Tile>(PrefabReferences.Tile);
            tile.GetComponent<Renderer>().material.mainTexture = tileInfo.Texture;
            return tile;
        }
    }
}
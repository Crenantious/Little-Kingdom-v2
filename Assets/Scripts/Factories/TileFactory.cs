using LittleKingdom.Board;
using System.Linq;
using UnityEngine;

namespace LittleKingdom.Factories
{
    public static class TileFactory
    {
        public static Tile Create(string resourceType) =>
            new(resourceType);

        public static TileMono CreateMono(Tile tile)
        {
            GameObject go = Object.Instantiate(PrefabReferences.Tile);

            TileInfo tileInfo = References.TilesInfo.Tiles.First(t => t.ResourceName == tile.ResourceName);
            go.GetComponent<Renderer>().material.mainTexture = tileInfo.Texture;

            TileMono tileMono = go.GetComponent<TileMono>();
            tileMono.Initialise(tile);

            return tileMono;
        }
    }
}
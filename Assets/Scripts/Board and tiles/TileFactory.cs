using UnityEngine;

namespace LittleKingdom
{
    public static class TileFactory
    {
        public static Tile Create(string resourceType) =>
            new(resourceType);

        public static TileMono CreateMono(Tile tile)
        {
            TileMono tileMono = Object.Instantiate(FactoriesConfig.TilePrefab).GetComponent<TileMono>();
            tileMono.Initialise(tile);
            return tileMono;
        }
    }
}
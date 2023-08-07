using LittleKingdom.Board;
using System.Linq;
using UnityEngine;

namespace LittleKingdom.Factories
{
    public static class TileFactory
    {
        public static Tile Create(TileInfo tileInfo)
        {
            GameObject go = Object.Instantiate(PrefabReferences.Tile);
            go.GetComponent<Renderer>().material.mainTexture = tileInfo.Texture;

            return go.GetComponent<Tile>();
        }
    }
}
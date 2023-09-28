using LittleKingdom.Board;
using UnityEngine;

namespace LittleKingdom
{
    public interface IReferences
    {
        public int DefaultLayer { get; }
        public int IgnoreRaycastLayer { get; }
        public int MaxResourceAmount { get; }
        public int MaxPlaceablesOnATile { get; }
        public Camera ActiveCamera { get; }
        public float TileWidth { get; }
        public float TileHeight { get; }
        public IBoard Board { get; set; }
        public GameState GameState { get; set; }
    }
}
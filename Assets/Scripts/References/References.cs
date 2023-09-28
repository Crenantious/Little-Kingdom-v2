using LittleKingdom.Board;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    // This should only be placed on one GameObject.
    public class References : MonoBehaviour, IReferences
    {
        [field: SerializeField] public int DefaultLayer { get; private set; }
        [field: SerializeField] public int IgnoreRaycastLayer { get; private set; }
        [field: SerializeField] public int MaxResourceAmount { get; private set; }
        [field: SerializeField]  public Camera ActiveCamera { get; set; }
        [field: SerializeField]  public int MaxPlaceablesOnATile { get; set; }

        public float TileWidth { get; private set; }
        public float TileHeight { get; private set; }

        public IBoard Board { get; set; }
        public GameState GameState { get; set; }

        [Inject]
        public void Construct(Tile tile)
        {
            TileWidth = tile.MeshRenderer.bounds.size.x;
            TileHeight = tile.MeshRenderer.bounds.size.z;
        }
    }
}
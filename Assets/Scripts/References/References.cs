using LittleKingdom.Board;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    // This class is loaded before the default load time, so references are ensured to be set up before being called.
    // This should only be placed on one GameObject.
    public class References : MonoBehaviour, IReferences
    {
        [field: SerializeField] public IUIReferences UI { get; private set; }
        [field: SerializeField] public int DefaultLayer { get; private set; }
        [field: SerializeField] public int IgnoreRaycastLayer { get; private set; }
        [field: SerializeField] public Camera ActiveCamera { get; private set; }

        public float TileWidth { get; private set; }
        public float TileHeight { get; private set; }

        public IBoard Board { get; set; }

        [Inject]
        public void Construct(TileMono tile)
        {
            TileWidth = tile.MeshRenderer.bounds.size.x;
            TileHeight = tile.MeshRenderer.bounds.size.z;
        }
    }
}
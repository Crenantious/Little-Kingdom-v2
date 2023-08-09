using LittleKingdom.Board;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    //This class is loaded before the default load time, so references are ensured to be set up before being called.
    //This should only be placed on one GameObject.
    // TODO: JR - dissolve into a Zenject installer.
    public class References : MonoBehaviour
    {
        private static References instance;

        public static int DefaultLayer => instance.defaultLayer;
        [SerializeField] private int defaultLayer;

        public static int IgnoreRaycastLayer => instance.ignoreRaycastLayer;
        [SerializeField] private int ignoreRaycastLayer;

        public static float TileWidth { get; private set; }
        public static float TileHeight { get; private set; }

        [Inject]
        public void Construct(TileMono tile)
        {
            TileWidth = tile.MeshRenderer.bounds.size.x;
            TileHeight = tile.MeshRenderer.bounds.size.x;
        }

        private void Awake()
        {
            instance = FindObjectOfType<References>();
        }
    }
}
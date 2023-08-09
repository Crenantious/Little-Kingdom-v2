using UnityEngine;

namespace LittleKingdom.Board
{
    [RequireComponent(typeof(MeshRenderer))]
	public class Tile : MonoBehaviour, ITile
    {
        [SerializeField] private MeshRenderer meshRenderer;

        public static float Width { get; private set; }
        public static float Height { get; private set; }

        public ResourceType ResourceType { get; private set; }
		public int Column { get; set; }
		public int Row { get; set; }
        public ITown Town { get; set; }

        /// <summary>
        /// Only to be called from a factory.
        /// </summary>
        public void Initialise(ResourceType resourceType)
        {
            ResourceType = resourceType;
            Width = meshRenderer.bounds.size.x;
            Height = meshRenderer.bounds.size.z;
        }
    }
}
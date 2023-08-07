using UnityEngine;

namespace LittleKingdom.Board
{
	public class Tile : MonoBehaviour, ITile
    {
        public static float Width { get; private set; }
        public static float Height { get; private set; }

        public string ResourceName { get; private set; }
		public int Column { get; set; }
		public int Row { get; set; }
        public ITown Town { get; set; }

        /// <summary>
        /// Only to be called from a factory.
        /// </summary>
        public void Initiallise(string resourceName)
        {
            ResourceName = resourceName;

            // TODO: JR - move this to DI.
            MeshRenderer meshRenderer = PrefabReferences.Tile.GetComponent<MeshRenderer>();
            Width = meshRenderer.bounds.size.x;
            Height = meshRenderer.bounds.size.z;
        }
    }
}
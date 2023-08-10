using LittleKingdom.Board;
using LittleKingdom.DataStructures;
using UnityEngine;

namespace LittleKingdom
{
    public class Town : MonoBehaviour, ITown
    {
        /// <inheritdoc/>
        [field: SerializeField] public int Width { get; protected set; }

        /// <inheritdoc/>
        [field: SerializeField] public int Height { get; protected set; }

        /// <summary>
        /// The top-left tile that town occupies.
        /// </summary>
        public ITile OriginTile { get; set; }

        /// <summary>
        /// The tiles that the town occupies.
        /// </summary>
        public Grid<ITile> Tiles { get; set; }

        public void SetPosition(Vector3 position) =>
            transform.position = position;
    }
}
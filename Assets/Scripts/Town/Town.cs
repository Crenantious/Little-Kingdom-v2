using UnityEngine;

namespace LittleKingdom
{
    public class Town : MonoBehaviour
    {
        /// <summary>
        /// The width of the town in tiles.
        /// </summary>
        [field: SerializeField] public int Width { get; private set; }

        /// <summary>
        /// The height of the town in tiles.
        /// </summary>
        [field: SerializeField] public int Height { get; private set; }
    }
}
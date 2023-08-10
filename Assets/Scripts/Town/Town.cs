using LittleKingdom.Board;
using LittleKingdom.DataStructures;
using UnityEngine;

namespace LittleKingdom
{
    public class Town : MonoBehaviour, ITown
    {
        [field: SerializeField] public int Width { get; protected set; }

        [field: SerializeField] public int Height { get; protected set; }

        public ITile OriginTile { get; set; }
        public Grid<ITile> Tiles { get; set; }
        public float XPosition { get => transform.position.x; }
        public float YPosition { get => transform.position.z; }

        public void SetPosition(Vector2 position) =>
            transform.position = new(position.x, 0, position.y);
    }
}
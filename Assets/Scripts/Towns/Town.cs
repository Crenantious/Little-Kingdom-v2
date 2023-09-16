using LittleKingdom.Board;
using LittleKingdom.DataStructures;
using System;
using UnityEngine;

namespace LittleKingdom
{
    [Serializable]
    public class Town : MonoBehaviour, ITown
    {
        [SerializeField] private Player player;

        [field: SerializeField] public int Width { get; protected set; }

        [field: SerializeField] public int Height { get; protected set; }

        public IPlayer Player => player;

        public ITile OriginTile { get; set; }
        public Grid<ITile> Tiles { get; set; }
        public float XPosition { get => transform.position.x; }
        public float YPosition { get => transform.position.z; }

        public void Awake() =>
            Tiles = new(Width, Height);

        public void SetPosition(Vector2 position) =>
            transform.position = new(position.x, 0, position.y);
    }
}
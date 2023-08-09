using UnityEngine;

namespace LittleKingdom.Board
{
    public class Tile : ITile
    {
        public ResourceType ResourceType { get; private set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public float XPosition { get; set; }
        public float YPosition { get; set; }
        public ITown Town { get; set; }

        public void Initialise(ResourceType resourceType) =>
            ResourceType = resourceType;

        public void SetPos(Vector2 position)
        {
            XPosition = position.x;
            YPosition = position.y;
        }
    }
}
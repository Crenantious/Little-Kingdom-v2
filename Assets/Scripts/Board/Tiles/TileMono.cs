using LittleKingdom.Resources;
using UnityEngine;

namespace LittleKingdom.Board
{
    [RequireComponent(typeof(MeshRenderer))]
    public class TileMono : MonoBehaviour, ITile
    {
        [field: SerializeField] public MeshRenderer MeshRenderer { get; private set; }

        public Transform Transform { get; private set; }
        public ResourceType ResourceType { get; private set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public float XPosition { get => transform.position.x; set => transform.position = new(value, transform.position.y, transform.position.z); }
        public float YPosition { get => transform.position.z; set => transform.position = new(transform.position.x, transform.position.y, value); }
        public ITown Town { get; set; }

        public void Initialise(ResourceType resourceType)
        {
            ResourceType = resourceType;
            Transform = transform;
        }

        public void SetPos(Vector2 position) =>
            transform.position = new(position.x, 0, position.y);
    }
}
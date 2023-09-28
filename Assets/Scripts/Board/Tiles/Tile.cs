using LittleKingdom.Resources;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Board
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Tile : MonoBehaviour, ITile
    {
        private readonly List<IPlaceableInTile> placeables = new();
        private readonly List<IHoldResources> holders = new();

        private IReferences references;

        [field: SerializeField] public MeshRenderer MeshRenderer { get; private set; }

        public Transform Transform { get; private set; }
        public Resources.Resources Resources { get; private set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public float XPosition { get => transform.position.x; set => transform.position = new(value, transform.position.y, transform.position.z); }
        public float YPosition { get => transform.position.z; set => transform.position = new(transform.position.x, transform.position.y, value); }
        public ITown Town { get; set; }

        public IReadOnlyList<IHoldResources> Holders { get; private set; }

        [Inject]
        public void Construct(IReferences references)
        {
            this.references = references;
            Holders = holders.AsReadOnly();
        }

        public void Initialise(Resources.Resources resources)
        {
            Resources = resources;
            Transform = transform;
        }

        public void SetPos(Vector2 position) =>
            transform.position = new(position.x, 0, position.y);

        public bool Add(IPlaceableInTile placeable)
        {
            if (placeables.Count == references.MaxPlaceablesOnATile)
                return false;

            placeables.Add(placeable);
            if (Inherits<IHoldResources>(placeable))
                holders.Add((IHoldResources)placeable);

            return true;
        }

        public bool Remove(IPlaceableInTile placeable)
        {
            if (placeables.Count == 0 || placeables.Contains(placeable) is false)
                return false;

            placeables.Remove(placeable);
            if (Inherits<IHoldResources>(placeable))
                holders.Add((IHoldResources)placeable);

            return true;
        }

        private static bool Inherits<T>(IPlaceableInTile placeable) =>
            typeof(T).IsAssignableFrom(placeable.GetType());
    }
}
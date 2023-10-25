using LittleKingdom.Input;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Board
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Tile : MonoBehaviour, ITile
    {
        private readonly List<IPlaceableInTile> placeables = new();

        private IReferences references;

        [field: SerializeField] public TileUnitSlots UnitSlots { get; private set; }

        [field: SerializeField] public MeshRenderer MeshRenderer { get; private set; }

        public Transform Transform { get; private set; }
        public Resources.Resources Resources { get; private set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public float XPosition { get => transform.position.x; set => transform.position = new(value, transform.position.y, transform.position.z); }
        public float YPosition { get => transform.position.z; set => transform.position = new(transform.position.x, transform.position.y, value); }
        public ITown Town { get; set; }

        [Inject]
        public void Construct(IReferences references, SelectedObjectTracker selection)
        {
            this.references = references;
        }

        public void Initialise(Resources.Resources resources)
        {
            Resources = resources;
            Transform = transform;
            UnitSlots.Initialise();
            UnitSlots.gameObject.SetActive(false);
        }

        public void SetPos(Vector2 position) =>
            transform.position = new(position.x, 0, position.y);

        public bool Add(IPlaceableInTile placeable)
        {
            if (placeables.Count == references.MaxPlaceablesOnATile)
                return false;

            placeables.Add(placeable);
            return true;
        }

        public bool Remove(IPlaceableInTile placeable)
        {
            if (placeables.Count == 0 || placeables.Contains(placeable) is false)
                return false;

            placeables.Remove(placeable);
            return true;
        }
    }
}
using LittleKingdom.Input;
using LittleKingdom.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Board
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Tile : MonoBehaviour, ITile
    {
        private readonly List<IPlaceableInTile> placeables = new();
        private readonly List<TileUnitSlot> unitSlots = new();

        private IReferences references;

        [SerializeField] private int numberOfUnitSlots = 8;
        [SerializeField] private float unitSlotPlacementRadius = 1;
        [SerializeField] private TileUnitSlot UnitSlotPrefab;

        [field: SerializeField] public MeshRenderer MeshRenderer { get; private set; }

        public Transform Transform { get; private set; }
        public Resources.Resources Resources { get; private set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public float XPosition { get => transform.position.x; set => transform.position = new(value, transform.position.y, transform.position.z); }
        public float YPosition { get => transform.position.z; set => transform.position = new(transform.position.x, transform.position.y, value); }
        public ITown Town { get; set; }

        public IReadOnlyList<TileUnitSlot> UnitSlots { get; private set; }

        [Inject]
        public void Construct(IReferences references, SelectedObjectTracker selection)
        {
            this.references = references;
            UnitSlots = unitSlots.AsReadOnly();
        }

        public void Initialise(Resources.Resources resources)
        {
            Resources = resources;
            Transform = transform;
            CreateUnitSlots();
        }

        private void CreateUnitSlots()
        {
            for (int i = 0; i < numberOfUnitSlots; i++)
            {
                float angle = i * Mathf.PI * 2f / numberOfUnitSlots;
                Vector3 position = new(Mathf.Cos(angle) * unitSlotPlacementRadius, 0, Mathf.Sin(angle) * unitSlotPlacementRadius);
                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                TileUnitSlot slot = Instantiate(UnitSlotPrefab);
                slot.transform.SetPositionAndRotation(transform.position + position, rotation);
                slot.transform.SetParent(transform, true);
                unitSlots.Add(slot);
            }
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
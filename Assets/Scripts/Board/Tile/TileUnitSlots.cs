using LittleKingdom.Factories;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Board
{
    [RequireComponent(typeof(MeshRenderer))]
    public class TileUnitSlots : MonoBehaviour
    {
        private readonly List<ITileUnitSlot> slots = new();

        [SerializeField] private int numberOfUnitSlots = 6;
        [SerializeField] private float unitSlotPlacementRadius = 0.6f;

        private TileUnitSlotFactory unitSlotFactory;

        public IReadOnlyList<ITileUnitSlot> Slots { get; private set; }

        [Inject]

        public void Construct(TileUnitSlotFactory unitSlotFactory) =>
            this.unitSlotFactory = unitSlotFactory;

        public void Initialise()
        {
            Slots = slots.AsReadOnly();
            CreateUnitSlots();
        }

        private void CreateUnitSlots()
        {
            for (int i = 0; i < numberOfUnitSlots; i++)
            {
                (Vector3 position, Quaternion rotation) = GetSlotPositionAndRotation(i);
                CreateUnitSlot(i, position, rotation);
            }
        }

        /// <summary>
        /// For placing uniformly on a circle about the center of <see cref="gameObject"/>.
        /// </summary>
        private (Vector3, Quaternion) GetSlotPositionAndRotation(int i)
        {
            float angle = i * Mathf.PI * 2f / numberOfUnitSlots;
            Vector3 position = new(Mathf.Cos(angle) * unitSlotPlacementRadius, 0, Mathf.Sin(angle) * unitSlotPlacementRadius);
            Quaternion rotation = Quaternion.identity;
            return (position, rotation);
        }

        private void CreateUnitSlot(int i, Vector3 position, Quaternion rotation)
        {
            ITileUnitSlot slot = unitSlotFactory.Create();
            slot.Transform.SetParent(transform);
            slot.Transform.SetPositionAndRotation(transform.position + position, rotation);
            slot.Transform.gameObject.name = "Unit slot " + i;
            slots.Add(slot);
        }
    }
}
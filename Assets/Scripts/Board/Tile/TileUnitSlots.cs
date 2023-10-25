using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Board
{
    [RequireComponent(typeof(MeshRenderer))]
    public class TileUnitSlots : MonoBehaviour
    {
        private readonly List<TileUnitSlot> slots = new();

        [SerializeField] private int numberOfUnitSlots = 8;
        [SerializeField] private float unitSlotPlacementRadius = 1;
        [SerializeField] private TileUnitSlot UnitSlotPrefab;

        public void Initialise()
        {
            CreateUnitSlots();
        }

        private void CreateUnitSlots()
        {
            for (int i = 0; i < numberOfUnitSlots; i++)
            {
                float angle = i * Mathf.PI * 2f / numberOfUnitSlots;
                Vector3 position = new(Mathf.Cos(angle) * unitSlotPlacementRadius, 0, Mathf.Sin(angle) * unitSlotPlacementRadius);
                Quaternion rotation = Quaternion.identity;

                TileUnitSlot slot = Instantiate(UnitSlotPrefab, transform);
                slot.transform.SetPositionAndRotation(transform.position + position, rotation);
                slots.Add(slot);
            }
        }
    }
}
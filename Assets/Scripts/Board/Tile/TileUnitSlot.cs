using LittleKingdom.Units;
using UnityEngine;

namespace LittleKingdom.Board
{
    public class TileUnitSlot : MonoBehaviour, ITileUnitSlot
    {
        [SerializeField] private new MeshRenderer renderer;
        [SerializeField] private Material neutral;
        [SerializeField] private Material free;
        [SerializeField] private Material taken;

        public ITileUnitSlot Next { get; set; }

        public ITileUnitSlot Previous { get; set; }

        public Unit Unit { get; set; }

        public Transform Transform => transform;

        public void ShowAvailability() =>
            renderer.material = Unit == null ? free : taken;

        public void HideAvailability() =>
            renderer.material = neutral;
    }
}
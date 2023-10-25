using LittleKingdom.Units;
using UnityEngine;

namespace LittleKingdom.Board
{
    public class TileUnitSlot : MonoBehaviour
    {
        [SerializeField] private new MeshRenderer renderer;
        [SerializeField] private Material neutral;
        [SerializeField] private Material free;
        [SerializeField] private Material taken;

        public TileUnitSlot Next { get; set; }

        public TileUnitSlot Previous { get; set; }

        public Unit Unit { get; set; }


        public void ShowAvailability() =>
            renderer.material = Unit == null ? free : taken;

        public void HideAvailability() =>
            renderer.material = neutral;

    }
}
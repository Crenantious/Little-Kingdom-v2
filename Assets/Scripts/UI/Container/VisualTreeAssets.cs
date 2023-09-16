using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    public class VisualTreeAssets : MonoBehaviour, IVisualTreeAssets
    {
        [field: SerializeField] public VisualTreeAsset BuildingInfoPanel { get; private set; }
    }
}
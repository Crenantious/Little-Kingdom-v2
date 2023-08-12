using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    public class BuildingInfoDisplayComponent : MonoBehaviour
    {
        [field: SerializeField] public UIDocument Document { get; private set; }
    }
}
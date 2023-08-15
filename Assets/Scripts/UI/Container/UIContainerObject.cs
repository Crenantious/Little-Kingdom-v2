using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class UIContainerObject : MonoBehaviour
    {
        [field: SerializeField] public UIDocument Document { get; private set; }
    }
}
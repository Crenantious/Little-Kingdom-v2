using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class UIContainerObject : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("What to display when the associated UIContainer.Show is called. " +
            "Note the UIDocument.SourceAsset will be overridden at runtime.")]
        private UIDocument document;

        public UIDocument Document => document;
    }
}
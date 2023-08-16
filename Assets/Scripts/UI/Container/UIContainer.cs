using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    public class UIContainer : MonoBehaviour, IUIContainer
    {
        [SerializeField] private UIContainerObject ContainerObject;

        public UIDocument Document => ContainerObject.Document;

        public void Show(VisualTreeAsset visualTree)
        {
            ContainerObject.Document.visualTreeAsset = visualTree;
            ContainerObject.gameObject.SetActive(true);
        }

        public void Hide() => ContainerObject.gameObject.SetActive(false);
    }
}
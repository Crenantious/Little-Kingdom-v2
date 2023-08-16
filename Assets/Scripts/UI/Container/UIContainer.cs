using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    public class UIContainer : IUIContainer
    {
        [field: SerializeField] public UIContainerObject ContainerObject;

        public UIContainer(UIContainerObject containerObject) =>
            ContainerObject = containerObject;

        public void Show(VisualTreeAsset visualTree)
        {
            ContainerObject.Document.visualTreeAsset = visualTree;
            ContainerObject.gameObject.SetActive(true);
        }

        public void Hide() => ContainerObject.gameObject.SetActive(false);
    }
}
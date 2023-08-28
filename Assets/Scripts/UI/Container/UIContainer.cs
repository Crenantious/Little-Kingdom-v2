using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    public class UIContainer : MonoBehaviour, IUIContainer
    {
        [SerializeField] private UIContainerObject ContainerObject;

        private Action teardown;

        public UIDocument Document => ContainerObject.Document;

        public void Show(VisualTreeAsset visualTree, Action teardown)
        {
            this.teardown?.Invoke();
            this.teardown = teardown;

            ContainerObject.Document.visualTreeAsset = visualTree;
            ContainerObject.gameObject.SetActive(true);
        }

        public void Hide()
        {
            teardown?.Invoke();
            teardown = null;
            ContainerObject.gameObject.SetActive(false);
        }
    }
}
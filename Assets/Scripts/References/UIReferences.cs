using LittleKingdom.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom
{
    // This should only be placed on one GameObject.
    public class UIReferences : MonoBehaviour, IUIReferences
    {
        #region UIContainers
        public UIContainer InfoPanelContainer { get; private set; }
        #endregion

        #region UIContainerObjects
        [SerializeField] private UIContainerObject infoPanelContainerObject;
        #endregion

        #region VisualTreeAssets
        [field: SerializeField] public VisualTreeAsset BuildingInfoPanelVisualTree { get; private set; }
        #endregion

        private void Awake()
        {
            InfoPanelContainer = new(infoPanelContainerObject);
        }
    }
}
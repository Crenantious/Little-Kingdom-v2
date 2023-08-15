using LittleKingdom.UI;
using UnityEngine.UIElements;

namespace LittleKingdom
{
    public interface IUIReferences
    {
        #region UIContainers
        public UIContainer InfoPanelContainer { get; }
        #endregion

        #region VisualTreeAssets
        public VisualTreeAsset BuildingInfoPanelVisualTree { get; }
        #endregion
    }
}
using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    public class UIBuildingInfoPanel : IUIContainter<BuildingInfoPanelData>
    {
        private readonly UIContainer infoPanel;
        private readonly VisualTreeAsset visualTreeAsset;

        public UIBuildingInfoPanel(UIContainer infoPanel)
        {
            this.infoPanel = infoPanel;
            visualTreeAsset = infoPanel.gameObject.GetComponent<InfoPanelVisualTreeAssets>().Buildings;
        }

        public void Show(BuildingInfoPanelData data)
        {
            infoPanel.Show(visualTreeAsset);
            GetTitle().text = data.Title;
            GetLevel().text = data.BuildingLevel.ToString();
            GetDescription().text = data.Description;
            GetUpgradeButton().clicked += data.UpgradeCallback;
        }

        public void Hide() => infoPanel.Hide();

        private Label GetTitle() =>
            GetRootVisualElement().Q("Title") as Label;

        private Label GetLevel() =>
            GetRootVisualElement().Q("BuildingLevelValue") as Label;

        private Label GetDescription() =>
            GetRootVisualElement().Q("BuildingDescription") as Label;

        private Button GetUpgradeButton() =>
            GetRootVisualElement().Q("UpgradeButton") as Button;

        private VisualElement GetRootVisualElement() =>
            infoPanel.Document.rootVisualElement;
    }
}
using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    public class UIBuildingInfoPanel : IUIContainter<BuildingInfoPanelData>
    {
        private readonly IUIReferences references;

        public UIBuildingInfoPanel(IUIReferences references) =>
            this.references = references;

        public void Show(BuildingInfoPanelData data)
        {
            references.InfoPanelContainer.Show(references.BuildingInfoPanelVisualTree);
            GetTitle().text = data.Title;
            GetLevel().text = data.BuildingLevel.ToString();
            GetDescription().text = data.Description;
            GetUpgradeButton().clicked += data.UpgradeCallback;
        }

        public void Hide() => references.InfoPanelContainer.Hide();

        private Label GetTitle() =>
            GetRootVisualElement().Q("Title") as Label;

        private Label GetLevel() =>
            GetRootVisualElement().Q("BuildingLevelValue") as Label;

        private Label GetDescription() =>
            GetRootVisualElement().Q("BuildingDescription") as Label;

        private Button GetUpgradeButton() =>
            GetRootVisualElement().Q("UpgradeButton") as Button;

        private VisualElement GetRootVisualElement() =>
            references.InfoPanelContainer.ContainerObject.Document.rootVisualElement;
    }
}
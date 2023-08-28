using LittleKingdom.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace LittleKingdom.Buildings
{
    [AddComponentMenu("LittleKingdom/Building info panel")]
    public class UIBuildingInfoPanel : MonoBehaviour, IUIContainter<Building>
    {
        [HideInInspector] private UIContainer infoPanel;
        [HideInInspector] private VisualTreeAsset visualTreeAsset;

        [Inject]
        public void Construct(UIContainer infoPanel, VisualTreeAsset visualTreeAsset)
        {
            this.infoPanel = infoPanel;
            this.visualTreeAsset = visualTreeAsset;
        }

        public void Show(Building data)
        {
            infoPanel.Show(visualTreeAsset, () => OnPanelHide(data));
            GetTitle().text = data.Title;
            GetLevel().text = data.BuildingLevel.ToString();
            GetDescription().text = data.Description;
            GetUpgradeButton().clicked += data.UpgradeCallback;
        }

        public void Hide() => infoPanel.Hide();

        private void OnPanelHide(Building data) =>
            GetUpgradeButton().clicked -= data.UpgradeCallback;

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
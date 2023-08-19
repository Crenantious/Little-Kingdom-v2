using LittleKingdom.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom.Buildings
{
    [RequireComponent(typeof(UIContainer))]
    [AddComponentMenu("LittleKingdom/Building info panel")]
    public class UIBuildingInfoPanel : MonoBehaviour, IUIContainter<Building>
    {
        [SerializeField] private UIContainer infoPanel;
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        //public void Awake()
        //{
        //    visualTreeAsset = infoPanel.gameObject.GetComponent<InfoPanelVisualTreeAssets>().Buildings;
        //}

        public void Show(Building data)
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
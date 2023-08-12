using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    public class BuildingInfoDisplay : MonoBehaviour, IInfoPanelDisplay<BuildingInfo>
    {
        [field: SerializeField] private GameObject displayObject;
        [field: SerializeField] public UIDocument Document { get; private set; }

        public void Display(BuildingInfo infoPanel)
        {
            displayObject.SetActive(true);
            GetTitle().text = infoPanel.Title;
            GetLevel().text = infoPanel.BuildingLevel.ToString();
            GetDescription().text = infoPanel.Description;
            GetUpgradeButton().clicked += infoPanel.UpgradeCallback;
        }

        private Label GetTitle() =>
            Document.rootVisualElement.Q("Title") as Label;

        private Label GetLevel() =>
            Document.rootVisualElement.Q("BuildingLevelValue") as Label;

        private Label GetDescription() =>
            Document.rootVisualElement.Q("BuildingDescription") as Label;

        private Button GetUpgradeButton() =>
            Document.rootVisualElement.Q("UpgradeButton") as Button;
    }
}
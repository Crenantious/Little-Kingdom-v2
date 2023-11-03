using LittleKingdom.Buildings;
using LittleKingdom.UI;
using UnityEngine;
using static LittleKingdom.InstallerUtilities;

namespace LittleKingdom
{
    public class UIInstaller : MonoInstaller<UIInstaller.BindType>
    {
        /// <summary>
        /// The prefab.
        /// </summary>
        [field: SerializeField] public DialogBox DialogBox { get; set; }

        /// <summary>
        /// The prefab.
        /// </summary>
        [field: SerializeField] public UIContainer InfoPanel { get; set; }

        /// <summary>
        /// The prefab.
        /// </summary>
        [field: SerializeField] public PlayerHUD PlayerHUD { get; set; }

        [field: SerializeField] public VisualTreeAssets VisualTreeAssets { get; set; }

        public enum BindType
        {
            DialogBox,
            PlayerHUD,
            VisualTreeAssets,
            InfoPanelForUIBuildingInfoPanel
        }

        public override void InstallBindings()
        {
            base.InstallBindings();

            Install(BindType.DialogBox, () => InstallPrefab<DialogBox>(DialogBox.gameObject).AsSingle().NonLazy());
            Install(BindType.PlayerHUD, () => InstallPrefab<PlayerHUD>(PlayerHUD.gameObject).AsSingle().NonLazy());
            Install(BindType.VisualTreeAssets, () => Container.BindInstance<IVisualTreeAssets>(VisualTreeAssets).AsSingle());
            // TODO: JR - find a nicer way to do this.
            Install(BindType.InfoPanelForUIBuildingInfoPanel,
                    () => Container.BindInstance(Instantiate(InfoPanel)).AsSingle().WhenInjectedInto<UIBuildingInfoPanel>());
        }
    }
}
using LittleKingdom.Buildings;
using LittleKingdom.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class UIInstaller : MonoInstaller<UIInstaller>
    {
        private static HashSet<BindType> excludeBinds = new();

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

        public static void ExcludeFromInstall(params BindType[] exclude) =>
            excludeBinds = exclude.ToHashSet();

        public override void InstallBindings()
        {
            Dictionary<BindType, Action> installActions = new()
            {
                { BindType.DialogBox, () => Container.BindInstance(Container.InstantiatePrefabForComponent<DialogBox>(DialogBox)) },
                { BindType.PlayerHUD, () => Container.BindInstance(Container.InstantiatePrefabForComponent<PlayerHUD>(PlayerHUD)) },
                { BindType.VisualTreeAssets,() => Container.BindInstance<IVisualTreeAssets>(VisualTreeAssets).AsSingle() },
                // TODO: JR - find a nicer way to do this.
                { BindType.InfoPanelForUIBuildingInfoPanel, () => Container.BindInstance(Instantiate(InfoPanel)).AsSingle().WhenInjectedInto<UIBuildingInfoPanel>() },
            };

            foreach (BindType bindType in Enum.GetValues(typeof(BindType)))
            {
                if (excludeBinds.Contains(bindType) is false)
                    installActions[bindType]();
            }
        }
    }
}
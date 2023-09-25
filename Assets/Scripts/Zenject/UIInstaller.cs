using LittleKingdom.Buildings;
using LittleKingdom.UI;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class UIInstaller : MonoInstaller<UIInstaller>
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

        public override void InstallBindings()
        {
            Container.BindInstance(Container.InstantiatePrefabForComponent<DialogBox>(DialogBox));
            Container.BindInstance(Container.InstantiatePrefabForComponent<PlayerHUD>(PlayerHUD));

            Container.BindInstance<IVisualTreeAssets>(VisualTreeAssets).AsSingle();

            // TODO: JR - find a nicer way to do this.
            Container.BindInstance(Instantiate(InfoPanel)).AsSingle().WhenInjectedInto<UIBuildingInfoPanel>();
        }
    }
}
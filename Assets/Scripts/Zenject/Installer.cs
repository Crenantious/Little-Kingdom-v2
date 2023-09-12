using LittleKingdom.Board;
using LittleKingdom.Buildings;
using LittleKingdom.Events;
using LittleKingdom.Factories;
using LittleKingdom.Input;
using LittleKingdom.Interactions;
using LittleKingdom.Loading;
using LittleKingdom.UI;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace LittleKingdom
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private Player player;
        [SerializeField] private References references;
        [SerializeField] private LoaderProfiles loaderProfiles;

        // UI
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private UIContainer infoPanel;
        [SerializeField] private VisualTreeAsset buildingInfoPanelTree;

        // Prefabs
        [SerializeField] private TileMono tilePrefab;

        public override void InstallBindings()
        {

            Container.Bind<Inputs>().AsSingle();
            Container.Bind<Loading.Loading>().AsSingle();
            Container.Bind<StandardInput>().AsSingle();
            Container.Bind<TownPlacedEvent>().AsSingle();
            Container.Bind<ObjectClickthrough>().AsSingle();
            Container.Bind<RaycastFromPointer>().AsSingle();
            Container.Bind<TileEntityAssignment>().AsSingle();
            Container.Bind<InteractionUtilities>().AsSingle();
            Container.Bind<SelectedObjectTracker>().AsSingle();
            Container.Bind<TownPlacementUtilities>().AsSingle();
            Container.Bind<IBoardGenerator>().To<BoardGenerator>().AsSingle();

            Container.BindInstance(player).AsSingle();
            Container.BindInstance(dialogBox).AsSingle();
            Container.BindInstance(tilePrefab).AsSingle();
            Container.BindInstance(loaderProfiles).AsSingle();
            Container.BindInstance(loaderProfiles.Current).AsSingle();
            Container.BindInstance<IReferences>(references).AsSingle();

            Container.BindInstance(infoPanel).AsSingle().WhenInjectedInto<UIBuildingInfoPanel>();
            Container.BindInstance(buildingInfoPanelTree).AsSingle().WhenInjectedInto<UIBuildingInfoPanel>();

            Container.BindFactory<ITownPlacement, TownPlacementFactory>().FromFactory<ManualTownPlacementFactory>();
            Container.BindFactory<ITileInfo, ITile, TileFactory>().FromFactory<CustomTileMonoFactory>();
            Container.BindFactory<IPlayer, PlayerFactory>().FromFactory<PlayerFactory>();
            Container.BindFactory<Type, ILoader, LoaderFactory>().FromFactory<LoaderFactory>();

            BindLoaderConfigs();
        }

        private void BindLoaderConfigs()
        {
            Container.BindInstance(loaderProfiles.Current.GetConfig<TownLC>()).AsSingle();
            Container.BindInstance(loaderProfiles.Current.GetConfig<BoardLC>()).AsSingle();
            Container.BindInstance(loaderProfiles.Current.GetConfig<GameSetupLC>()).AsSingle();
        }
    }
}
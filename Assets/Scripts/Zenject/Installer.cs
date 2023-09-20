using LittleKingdom.Board;
using LittleKingdom.Buildings;
using LittleKingdom.CharacterTurns;
using LittleKingdom.Events;
using LittleKingdom.Factories;
using LittleKingdom.Input;
using LittleKingdom.Interactions;
using LittleKingdom.Loading;
using LittleKingdom.UI;
using System;
using UnityEngine;
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
        [SerializeField] private PlayerHUD playerHUD;
        [SerializeField] private VisualTreeAssets visualTreeAssets;

        // Prefabs
        [SerializeField] private TileMono tilePrefab;

        public override void InstallBindings()
        {
            InstallSingletons();
            InstallTransients();
            InstallFactories();
            InstallLoaderConfigs();
        }

        private void InstallSingletons()
        {
            Container.Bind<Inputs>().AsSingle();
            Container.Bind<StandardInput>().AsSingle();
            Container.Bind<Loading.Loading>().AsSingle();
            Container.Bind<TownPlacedEvent>().AsSingle();
            Container.Bind<ObjectClickthrough>().AsSingle();
            Container.Bind<RaycastFromPointer>().AsSingle();
            Container.Bind<TileEntityAssignment>().AsSingle();
            Container.Bind<InteractionUtilities>().AsSingle();
            Container.Bind<SelectedObjectTracker>().AsSingle();
            Container.Bind<TownPlacementUtilities>().AsSingle();
            Container.Bind<CharacterTurnTransitions>().AsSingle();

            Container.Bind<IBoardGenerator>().To<BoardGenerator>().AsSingle();

            Container.BindInstance(player).AsSingle();
            Container.BindInstance(dialogBox).AsSingle();
            Container.BindInstance(playerHUD).AsSingle();
            Container.BindInstance(tilePrefab).AsSingle();
            Container.BindInstance(loaderProfiles).AsSingle();
            Container.BindInstance(loaderProfiles.Current).AsSingle();

            Container.BindInstance<IReferences>(references).AsSingle();
            Container.BindInstance<IVisualTreeAssets>(visualTreeAssets).AsSingle();

            Container.BindInstance(infoPanel).AsSingle().WhenInjectedInto<UIBuildingInfoPanel>();
        }

        private void InstallTransients()
        {
            Container.Bind<CharacterTurnOrder>().AsTransient();
        }

        private void InstallFactories()
        {
            Container.BindFactory<IPlayer, PlayerFactory>().FromFactory<PlayerFactory>();
            Container.BindFactory<Type, ILoader, LoaderFactory>().FromFactory<LoaderFactory>();
            Container.BindFactory<ITileInfo, ITile, TileFactory>().FromFactory<CustomTileMonoFactory>();
            BindTownPlacementFactory();
        }

        private void BindTownPlacementFactory()
        {
            if (loaderProfiles.Current.GetConfig<TownLC>().AutoPlace)
                Container.BindFactory<ITownPlacement, TownPlacementFactory>().FromFactory<AutomaticTownPlacementFactory>();
            else
                Container.BindFactory<ITownPlacement, TownPlacementFactory>().FromFactory<ManualTownPlacementFactory>();
        }

        private void InstallLoaderConfigs()
        {
            Container.BindInstance(loaderProfiles.Current.GetConfig<TownLC>()).AsSingle();
            Container.BindInstance(loaderProfiles.Current.GetConfig<BoardLC>()).AsSingle();
            Container.BindInstance(loaderProfiles.Current.GetConfig<GameSetupLC>()).AsSingle();
            Container.BindInstance(loaderProfiles.Current.GetConfig<CharacterTurnsLC>()).AsSingle();
        }
    }
}
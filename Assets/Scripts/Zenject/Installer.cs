using LittleKingdom.Board;
using LittleKingdom.Events;
using LittleKingdom.Factories;
using LittleKingdom.Loading;
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
        [SerializeField] private LoaderTransitions loaderTransitions;

        // Prefabs
        [SerializeField] private TileMono tilePrefab;

        public override void InstallBindings()
        {
            InstallSingletons();
            InstallFactories();
            InstallLoaderConfigs();

            InputInstaller.Install(Container);
            CharacterTurnInstaller.Install(Container);
            PlayerInstaller.Install(Container);
        }

        private void InstallSingletons()
        {
            Container.Bind<Loading.Loading>().AsSingle();
            Container.Bind<TownPlacedEvent>().AsSingle();
            Container.Bind<TileEntityAssignment>().AsSingle();
            Container.Bind<TownPlacementUtilities>().AsSingle();

            Container.Bind<IBoardGenerator>().To<BoardGenerator>().AsSingle();

            Container.BindInstance(tilePrefab).AsSingle();
            Container.BindInstance(loaderProfiles).AsSingle();
            Container.BindInstance(loaderProfiles.Current).AsSingle();

            Container.BindInstance<IReferences>(references).AsSingle();
        }

        private void InstallFactories()
        {
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
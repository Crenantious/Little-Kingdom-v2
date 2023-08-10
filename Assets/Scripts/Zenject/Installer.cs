using LittleKingdom.Board;
using LittleKingdom.Events;
using LittleKingdom.Factories;
using LittleKingdom.Input;
using LittleKingdom.Loading;
using LittleKingdom.UI;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private MonoSimulator monoSimulator;
        [SerializeField] private LoaderProfiles loaderProfiles;
        [SerializeField] private References references;

        // Prefabs
        [SerializeField] private TileMono tilePrefab;

        public override void InstallBindings()
        {
            Container.Bind<Inputs>().AsSingle();
            Container.Bind<UIInput>().AsSingle();
            Container.Bind<InGameInput>().AsSingle();
            Container.Bind<InputUtility>().AsSingle();
            Container.Bind<ManualTownPlacement>().AsSingle();
            Container.Bind<TownPlacedEvent>().AsSingle();
            Container.Bind<TileEntityAssignment>().AsSingle();
            Container.Bind<TownPlacementUtilities>().AsSingle();

            Container.Bind<IBoardGenerator>().To<BoardGenerator>().AsSingle();

            Container.BindInstance(dialogBox).AsSingle();
            Container.BindInstance(monoSimulator).AsSingle();
            Container.BindInstance(loaderProfiles).AsSingle();
            Container.BindInstance(loaderProfiles.Current).AsSingle();
            Container.BindInstance<IReferences>(references).AsSingle();

            Container.BindInstance(tilePrefab).AsSingle();

            Container.BindFactory<ManualTownPlacement, TownPlacementFactory>();
            Container.BindFactory<ITileInfo, ITile, TileFactory>().FromFactory<CustomTileMonoFactory>();

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
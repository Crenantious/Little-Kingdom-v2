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
        public static IBoard Board { get; set; }

        [SerializeField] private new Camera camera;
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private MonoSimulator monoSimulator;
        [SerializeField] private LoaderProfiles loaderProfiles;

        // Prefabs
        [SerializeField] private TileMono tilePrefab;

        public override void InstallBindings()
        {
            Container.Bind<Inputs>().AsSingle();
            Container.Bind<UIInput>().AsSingle();
            Container.Bind<InGameInput>().AsSingle();
            Container.Bind<InputUtility>().AsSingle();
            Container.Bind<TownPlacement>().AsSingle();
            Container.Bind<BoardGenerator>().AsSingle();
            Container.Bind<TownPlacedEvent>().AsSingle();
            Container.Bind<TileEntityAssignment>().AsSingle();

            Container.Bind<IBoard>().FromMethod(x => Board).AsSingle();
            Container.Bind<IBoardGenerator>().To<BoardGeneratorMono>().AsSingle();

            Container.BindInstance(camera).AsSingle();
            Container.BindInstance(dialogBox).AsSingle();
            Container.BindInstance(monoSimulator).AsSingle();
            Container.BindInstance(loaderProfiles).AsSingle();
            Container.BindInstance(loaderProfiles.Current).AsSingle();

            Container.BindInstance(tilePrefab).AsSingle();

            Container.BindFactory<TownPlacement, TownPlacementFactory>();
            Container.BindFactory<TileInfo, ITile, TileFactory>().FromFactory<CustomTileMonoFactory>();

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
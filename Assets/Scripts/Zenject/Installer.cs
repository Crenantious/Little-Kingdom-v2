using LittleKingdom.Board;
using LittleKingdom.Events;
using LittleKingdom.Factories;
using LittleKingdom.Input;
using LittleKingdom.Interactions;
using LittleKingdom.Loading;
using LittleKingdom.UI;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private LoaderProfiles loaderProfiles;
        [SerializeField] private References references;

        // Prefabs
        [SerializeField] private TileMono tilePrefab;

        public override void InstallBindings()
        {
            Container.Bind<Inputs>().AsSingle();
            Container.Bind<StandardInput>().AsSingle();
            Container.Bind<ObjectClickthrough>().AsSingle();
            Container.Bind<TownPlacedEvent>().AsSingle();
            Container.Bind<TileEntityAssignment>().AsSingle();
            Container.Bind<InteractionUtilities>().AsSingle();
            Container.Bind<SelectedObjectTracker>().AsSingle();
            Container.Bind<TownPlacementUtilities>().AsSingle();
            Container.Bind<IBoardGenerator>().To<BoardGenerator>().AsSingle();

            Container.BindInstance(dialogBox).AsSingle();
            Container.BindInstance(tilePrefab).AsSingle();
            Container.BindInstance(loaderProfiles).AsSingle();
            Container.BindInstance(loaderProfiles.Current).AsSingle();
            Container.BindInstance<IReferences>(references).AsSingle();

            Container.BindFactory<ITownPlacement, TownPlacementFactory>().FromFactory<ManualTownPlacementFactory>();
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
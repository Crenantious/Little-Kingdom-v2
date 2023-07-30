using LittleKingdom.Input;
using Zenject;

namespace LittleKingdom
{
    public class Installer : MonoInstaller
    {
        public Inputs inputs;
        public TownPlacement townPlacement;
        public MonoSimulator monoSimulator;

        public override void InstallBindings()
        {
            Container.BindInstance(inputs).AsSingle();
            Container.BindInstance(townPlacement).AsSingle();
            Container.BindInstance(monoSimulator).AsSingle();
        }
    }
}
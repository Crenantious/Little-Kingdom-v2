using LittleKingdom.Input;
using LittleKingdom.Interactions;
using Zenject;

namespace LittleKingdom
{
    public class InputInstaller : Installer<InputInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Inputs>().AsSingle();
            Container.Bind<StandardInput>().AsSingle();
            Container.Bind<ObjectClickthrough>().AsSingle();
            Container.Bind<RaycastFromPointer>().AsSingle();
            Container.Bind<InteractionUtilities>().AsSingle();
            Container.Bind<SelectedObjectTracker>().AsSingle();
        }
    }
}
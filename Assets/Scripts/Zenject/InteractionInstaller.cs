using LittleKingdom.Input;
using LittleKingdom.Interactions;
using Zenject;

namespace LittleKingdom
{
    public class InteractionInstaller : Installer<InteractionInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ObjectClickthrough>().AsSingle();
            Container.Bind<RaycastFromPointer>().AsSingle();
            Container.Bind<InteractionUtilities>().AsSingle();
            Container.Bind<SelectedObjectTracker>().AsSingle();
        }
    }
}
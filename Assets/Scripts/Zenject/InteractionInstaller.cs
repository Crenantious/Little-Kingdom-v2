using LittleKingdom.Input;
using LittleKingdom.Interactions;

namespace LittleKingdom
{
    public class InteractionInstaller : Installer<InteractionInstaller.BindType, InteractionInstaller>
    {
        public enum BindType
        {
            ObjectClickthrough,
            RaycastFromPointer,
            InteractionUtilities,
            SelectedObjectTracker
        }

        public override void InstallBindings()
        {
            Install(BindType.ObjectClickthrough, () => Container.Bind<ObjectClickthrough>().AsSingle());
            Install(BindType.RaycastFromPointer, () => Container.Bind<RaycastFromPointer>().AsSingle());
            Install(BindType.InteractionUtilities, () => Container.Bind<InteractionUtilities>().AsSingle());
            Install(BindType.SelectedObjectTracker, () => Container.Bind<SelectedObjectTracker>().AsSingle());
        }
    }
}
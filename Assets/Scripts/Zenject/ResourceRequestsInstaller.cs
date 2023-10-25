using LittleKingdom.Resources;
using Zenject;

namespace LittleKingdom
{
    public class ResourceRequestsInstaller : MonoInstaller
    {
        public ResourceCollectionOrder collectionOrder;

        public override void InstallBindings()
        {
            Container.Bind<RegisteredMoveResourceRequests>().AsSingle();
            Container.Bind<RegisteredHaltResourceRequests>().AsSingle();
            Container.Bind<IResourceCollectionOrder>().FromInstance(collectionOrder).AsSingle();
        }
    }
}
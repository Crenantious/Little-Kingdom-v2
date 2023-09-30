using LittleKingdom.Resources;
using Zenject;

namespace LittleKingdom
{
    public class ResourceRequestsInstaller : Installer<PlayerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<RegisteredMoveResourceRequests>().AsSingle();
            Container.Bind<RegisteredHaltResourceRequests>().AsSingle();
        }
    }
}
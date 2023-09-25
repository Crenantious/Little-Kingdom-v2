using LittleKingdom.Input;
using Zenject;

namespace LittleKingdom
{
    public class InputInstaller : Installer<InputInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Inputs>().AsSingle();
            Container.Bind<StandardInput>().AsSingle();
        }
    }
}
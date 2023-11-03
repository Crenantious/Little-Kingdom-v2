using LittleKingdom.Input;

namespace LittleKingdom
{
    public class InputInstaller : Installer<InputInstaller.BindType, InputInstaller>
    {
        public enum BindType
        {
            Inputs,
            StandardInput,
            RaycastFromPointer
        }

        public override void InstallBindings()
        {
            Install(BindType.Inputs, () => Container.Bind<Inputs>().AsSingle());
            Install(BindType.StandardInput, () => Container.Bind<StandardInput>().AsSingle());
            Install(BindType.RaycastFromPointer, () => Container.Bind<RaycastFromPointer>().AsSingle());
        }
    }
}
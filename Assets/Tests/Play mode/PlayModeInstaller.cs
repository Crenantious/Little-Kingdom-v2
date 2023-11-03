using LittleKingdom;
using LittleKingdom.TestsCommon;
using static LittleKingdom.TestsCommon.TestsInstallerUtilities;

namespace PlayModeTests
{
    public class PlayModeInstaller : TestsInstaller<PlayModeInstaller.BindType, PlayModeInstaller>
    {
        public enum BindType
        {
            IReferences,
            Input,
            UI,
            PlayModeTestHelper,
        }

        public override void InstallBindings()
        {
            base.InstallBindings();

            Install(BindType.IReferences, () => InstallDefaultMock<IReferences>().AsSingle());
            Install(BindType.Input, () => InputInstaller.Install(Container));
            Install(BindType.UI, () => UIInstaller.InstallFromResource("Installers/UIInstaller", Container));
            Install(BindType.PlayModeTestHelper, () => Container.Bind<PlayModeTestHelper>().AsSingle());
        }
    }
}
using System;

namespace LittleKingdom.TestsCommon
{
    public abstract class TestsInstaller<BindType, TDerived> : Installer<BindType, TDerived>
        where BindType : struct, Enum
        where TDerived : Installer<BindType, TDerived>
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            TestsInstallerUtilities.Container = Container;
        }
    }
}
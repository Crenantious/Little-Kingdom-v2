using System;
using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom
{
    public abstract class MonoInstaller<BindType> : Zenject.MonoInstaller<MonoInstaller<BindType>>
        where BindType : struct, Enum
    {
        private static HashSet<BindType> excludeBinds = new();

        /// <summary>
        /// Sets the binds to ignore when installing.
        /// </summary>
        public static void ExcludeFromInstall(params BindType[] exclude) =>
            excludeBinds = exclude.ToHashSet();

        /// <summary>
        /// Install <see cref="Container"/> bindings using <see cref="Install(BindType, Action)"/>.
        /// </summary>
        public override void InstallBindings()
        {
            InstallerUtilities.Container = Container;
        }

        public void Install(BindType bind, Action install)
        {
            if (excludeBinds.Contains(bind) is false)
                install();
        }
    }
}
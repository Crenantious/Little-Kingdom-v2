using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public static class InstallerUtilities
    {
        public static DiContainer Container { get; set; }

        public enum InstallPrefabMode
        {
            Cached,
            Single,
            Transient
        }

        public static void InstallBindings<BindType>(Dictionary<BindType, Action> installActions, HashSet<BindType> excludeBinds)
            where BindType : struct, Enum
        {
            foreach (BindType bindType in Enum.GetValues(typeof(BindType)))
            {
                if (excludeBinds.Contains(bindType) is false)
                    installActions[bindType]();
            }
        }

        public static void Install<BindType>(BindType bind, Action install, HashSet<BindType> excludeBinds)
            where BindType : struct, Enum
        {
            if (excludeBinds.Contains(bind) is false)
                install();
        }

        public static NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder InstallPrefab<T>(GameObject prefab)
            where T : Component =>
            Container.Bind<T>().FromComponentInNewPrefab(prefab);

        public static NameTransformScopeConcreteIdArgConditionCopyNonLazyBinder InstallPrefab<T>(string prefabName)
            where T : Component =>
            InstallPrefab<T>(AssetUtilities.LoadPrefab(prefabName));
    }
}
using LittleKingdom;
using LittleKingdom.Buildings;
using LittleKingdom.UI;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace PlayModeTests
{
    public class PlayModeInstaller : Installer<PlayModeInstaller>
    {
        private HashSet<BindType> excludeBinds = new();

        public enum BindType
        {
            IReferences,
            TestUtilities,
            Input,
            UI,
            PlayModeTestHelper,
        }

        public void ExcludeFromInstall(params BindType[] exclude) =>
            excludeBinds = exclude.ToHashSet();

        public override void InstallBindings()
        {
            Dictionary<BindType, Action> installActions = new()
            {
                { BindType.IReferences, () => InstallMock(CreateDefaultMock<IReferences>()).AsSingle() },
                { BindType.TestUtilities, () => Container.Bind<TestUtilities>().AsSingle() },
                { BindType.Input,() => InputInstaller.Install(Container) },
                { BindType.UI, () => UIInstaller.InstallFromResource(Container) },
                { BindType.PlayModeTestHelper, () => Container.Bind<PlayModeTestHelper>().AsSingle()},
            };

            foreach (BindType bindType in Enum.GetValues(typeof(BindType)))
            {
                if (excludeBinds.Contains(bindType) is false)
                    installActions[bindType]();
            }
        }

        private T InstallPrefab<T>(string prefabName) where T : Component
        {
            GameObject prefab = UnityEngine.Object.Instantiate(TestUtilities.LoadPrefab(prefabName));
            Container.Bind<T>().FromComponentInNewPrefab(prefab).AsSingle();
            return prefab.GetComponent<T>();
        }

        private Mock<T> CreateDefaultMock<T>() where T : class
        {
            Mock<T> mock = new();
            mock.SetupAllProperties();
            return mock;
        }

        private ScopeConcreteIdArgConditionCopyNonLazyBinder InstallMock<T>(Mock<T> mock) where T : class =>
            Container.Bind<T>().FromInstance(mock.Object);
    }
}
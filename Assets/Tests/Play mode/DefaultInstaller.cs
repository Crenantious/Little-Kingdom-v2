using LittleKingdom;
using LittleKingdom.Input;
using LittleKingdom.UI;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace PlayModeTests
{
    public class DefaultInstaller
    {
        private readonly DiContainer container;
        private readonly Action[] installActions;

        public enum BindType
        {
            IReferences,
            TestUtilities,
            PlayModeTestHelper,
            Inputs,
            DialogBox,
            StandardInput,
        }

        public DefaultInstaller(DiContainer container)
        {
            this.container = container;

            installActions = new Action[]
            {
                () => InstallMock(CreateDefaultMock<IReferences>()).AsSingle(),
                () => container.Bind<TestUtilities>().AsSingle(),
                () => container.Bind<PlayModeTestHelper>().AsSingle(),
                () => container.Bind<Inputs>().AsSingle(),
                () => container.Bind<StandardInput>().AsSingle(),
                () => InstallPrefab<DialogBox>("Dialog box"),
            };
        }

        public DefaultInstaller()
        {
            if (Enum.GetNames(typeof(BindType)).Length != installActions.Length)
                throw new TypeInitializationException(nameof(installActions), new Exception("Not all bind types have an installation associated."));
        }

        public void InstallBindings(params BindType[] exclude)
        {
            HashSet<BindType> excludeBinds = exclude.ToHashSet();

            foreach (BindType bindType in Enum.GetValues(typeof(BindType)))
            {
                if (excludeBinds.Contains(bindType) is false)
                    installActions[(int)bindType]();
            }
        }

        private T InstallPrefab<T>(string prefabName)
        {
            GameObject prefab = UnityEngine.Object.Instantiate(TestUtilities.LoadPrefab(prefabName));
            container.Bind<T>().FromComponentInNewPrefab(prefab).AsSingle();
            return prefab.GetComponent<T>();
        }

        private Mock<T> CreateDefaultMock<T>() where T : class
        {
            Mock<T> mock = new();
            mock.SetupAllProperties();
            return mock;
        }

        private ScopeConcreteIdArgConditionCopyNonLazyBinder InstallMock<T>(Mock<T> mock) where T : class =>
            container.Bind<T>().FromInstance(mock.Object);
    }
}
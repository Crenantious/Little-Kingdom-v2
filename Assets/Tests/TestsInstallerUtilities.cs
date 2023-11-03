using Moq;
using Zenject;

namespace LittleKingdom.TestsCommon
{
    public static class TestsInstallerUtilities
    {
        public static DiContainer Container { get; set; }

        public static Mock<T> CreateDefaultMock<T>() where T : class
        {
            Mock<T> mock = new();
            mock.SetupAllProperties();
            return mock;
        }

        public static ScopeConcreteIdArgConditionCopyNonLazyBinder InstallMock<T>(Mock<T> mock) where T : class =>
            Container.Bind<T>().FromInstance(mock.Object);

        public static ScopeConcreteIdArgConditionCopyNonLazyBinder InstallDefaultMock<T>() where T : class =>
            InstallMock(CreateDefaultMock<T>());
    }
}
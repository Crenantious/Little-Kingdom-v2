using LittleKingdom.Input;
using LittleKingdom.UI;
using UnityEngine;
using Zenject;

namespace PlayModeTests
{
    public static class CommonInstaller
    {
        public static void InstallBindings(DiContainer container)
        {
            container.Bind<TestUtilities>().AsSingle();
            container.Bind<PlayModeTestHelper>().AsSingle();
            container.Bind<UIInput>().AsSingle();
            container.Bind<Inputs>().AsSingle();
            var dialogBox = Object.Instantiate(TestUtilities.LoadPrefab("Dialog box"));
            container.Bind<DialogBox>().FromComponentInNewPrefab(dialogBox).AsSingle();
        }
    }
}
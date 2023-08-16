using LittleKingdom;
using LittleKingdom.UI;
using Moq;
using NUnit.Framework;
using PlayModeTests;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace InfoPanelTests
{
    public class InfoPanelTests : ZenjectUnitTestFixture
    {
        [Inject] private readonly PlayModeTestHelper testHelper;
        [Inject] private readonly UIBuildingInfoPanel buildingInfoPanel;

        [SetUp]
        public void CommonInstall()
        {
            DefaultInstaller defaultInstaller = new(Container);
            Container.Bind<UIBuildingInfoPanel>().AsSingle();

            defaultInstaller.InstallBindings();
            Container.Inject(this);
        }

        [UnityTest]
        public IEnumerator OpenBuildingInfoDisplay_VerifyContent()
        {
            Mock<ITestCallback> testCallback = new();
            BuildingInfoPanelData buildingData = new("Test title", 3, "A super boring description.", testCallback.Object.Callback);
            testHelper.Initialise(() => testCallback.Verify(t => t.Callback(), Times.Once()));

            buildingInfoPanel.Show(buildingData);

            yield return testHelper;
        }
    }
}
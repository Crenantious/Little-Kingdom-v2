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

        private UIBuildingInfoPanel buildingInfoDisplay;

        [SetUp]
        public void CommonInstall()
        {
            CommonInstaller.InstallBindings(Container);
            buildingInfoDisplay = Object.Instantiate(TestUtilities.LoadPrefab("Building info display"))
                .GetComponent<UIBuildingInfoPanel>();
            Container.BindInstance(buildingInfoDisplay).AsSingle();
            Container.Inject(this);
        }

        [UnityTest]
        public IEnumerator OpenBuildingInfoDisplay_VerifyContent()
        {
            Mock<ITestCallback> testCallback = new();
            BuildingInfoPanelData buildingInfo = new("Test title", 3, "A super boring description.", testCallback.Object.Callback);
            testHelper.Initialise(() => testCallback.Verify(t => t.Callback(), Times.Once()));

            buildingInfoDisplay.Show(buildingInfo);

            yield return testHelper;
        }
    }
}
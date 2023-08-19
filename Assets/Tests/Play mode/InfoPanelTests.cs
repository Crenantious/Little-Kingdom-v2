using LittleKingdom.Buildings;
using LittleKingdom.UI;
using Moq;
using NUnit.Framework;
using PlayModeTests;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace InfoPanelTests
{
    public class InfoPanelTests : ZenjectUnitTestFixture
    {
        [Inject] private readonly PlayModeTestHelper testHelper;
        private GameObject infoPanel;

        [SetUp]
        public void CommonInstall()
        {
            DefaultInstaller defaultInstaller = new(Container);
            defaultInstaller.InstallBindings();

            Container.Bind<UIContainer>()
                .FromComponentInNewPrefab(TestUtilities.LoadPrefab("Info panel"))
                .WhenInjectedInto<UIBuildingInfoPanel>();

            Container.Inject(this);

            infoPanel = UnityEngine.Object.Instantiate(TestUtilities.LoadPrefab("Info panel"));
        }

        [UnityTest]
        public IEnumerator OpenBuildingInfoDisplay_VerifyContent()
        {
            (Mock<ITestCallback> _, Building building) = CreateObjects();
            testHelper.Initialise();

            infoPanel.GetComponent<UIBuildingInfoPanel>().Show(building);

            yield return testHelper;
        }

        [UnityTest]
        public IEnumerator OpenBuildingInfoDisplay_DoNotPressUpgrade_WasNotUpgraded()
        {
            (Mock<ITestCallback> testCallback, Building building) = CreateObjects();
            testHelper.Initialise(() => testCallback.Verify(t => t.Callback(), Times.Never()), true);

            infoPanel.GetComponent<UIBuildingInfoPanel>().Show(building);

            yield return testHelper;
        }

        [UnityTest]
        public IEnumerator OpenBuildingInfoDisplay_PressUpgrade_WasUpgraded()
        {
            (Mock<ITestCallback> testCallback, Building building) = CreateObjects();
            testHelper.Initialise(() => testCallback.Verify(t => t.Callback(), Times.Once()), true);

            infoPanel.GetComponent<UIBuildingInfoPanel>().Show(building);

            yield return testHelper;
        }

        private static (Mock<ITestCallback> testCallback, Building building) CreateObjects()
        {
            Mock<ITestCallback> testCallback = new();
            Building building;
            GameObject testObject = new();
            building = testObject.AddComponent<Building>();
            building.Title = "Test title";
            building.BuildingLevel = 3;
            building.Description = "A super boring description.";
            building.UpgradeCallback = testCallback.Object.Callback;
            return (testCallback, building);
        }
    }
}
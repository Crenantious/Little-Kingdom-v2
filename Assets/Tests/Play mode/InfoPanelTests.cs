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
        private BuildingInfoDisplay buildingInfoDisplay;
        private MonoBehaviourTest<UITestObject> uiTestObject;

        [SetUp]
        public void CommonInstall()
        {
            uiTestObject = UITestUtilities.GetUITestObject();
            var a = TestUtilities.LoadPrefab("Building info display");
            var b = Object.Instantiate(a);
            buildingInfoDisplay = b.GetComponent<BuildingInfoDisplay>();

            Container.BindInstance(buildingInfoDisplay).AsSingle();
            Container.Inject(this);
        }

        [UnityTest]
        public IEnumerator CreateSizedGrid_GetNearestIndex_WorksCorrectly()
        {
            Mock<ITestCallback> testCallback = new();
            BuildingInfo buildingInfo = new("Test title", 3, "A super boring description.", testCallback.Object.Callback);

            buildingInfoDisplay.Display(buildingInfo);

            // TODO: JR - add a test panel that has button to accept and reject the test. This is needed since it is visual.
            // On that panel, add a verify button as well that is to be called after performing some actions. It would perform
            // specified asserts, such as the one below.
            //testCallback.Verify(t => t.Callback(), Times.Once());
            yield return uiTestObject;
        }
    }
    public interface ITestCallback
    {
        public void Callback();
    }
}
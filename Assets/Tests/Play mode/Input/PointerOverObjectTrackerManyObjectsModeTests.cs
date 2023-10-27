using InfoPanelTests;
using LittleKingdom.Input;
using LittleKingdom.PlayModeTests.Utilities;
using Moq;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class PointerOverObjectTrackerManyObjectsModeTests : InputTestsBase
{
    [Inject] private readonly PointerOverObjectTracker tracker;

    private MouseUtilities mouse;
    private GameObject objectOneAndTwo;
    private GameObject objectThreeAndFour;
    private Mock<ITestCallback<GameObject>> onPointerEnter;
    private Mock<ITestCallback<GameObject>> onPointerExit;

    protected override void Install()
    {
        Container.Bind<PointerOverObjectTracker>().AsSingle();
        base.Install();
    }

    protected override void PostInstall()
    {
        base.PostInstall();

        tracker.SetMode(PointerOverObjectTracker.Mode.TrackMany);

        // A reasonable z is needed so everything fits in the camera's view
        ObjectOne.transform.position = new(2, 0, 10);
        ObjectTwo.transform.position = new(3, 0, 11);
        ObjectThree.transform.position = new(0, 0, 10);
        ObjectFour.transform.position = new(1, 0, 11);
        EmptySpace.transform.position = new(0, 2, 10);

        objectOneAndTwo = CreateTestObject("ObjectOneAndTwo", false);
        objectOneAndTwo.transform.position = new(2.5f, 0, 10);

        objectThreeAndFour = CreateTestObject("ObjectThreeAndFour", false);
        objectThreeAndFour.transform.position = new(0.5f, 0, 10);

        onPointerEnter = new();
        onPointerExit = new();
        onPointerEnter.SetupAllProperties();
        onPointerExit.SetupAllProperties();
        onPointerEnter.Setup(x => x.Callback(It.IsAny<GameObject>()));
        onPointerExit.Setup(x => x.Callback(It.IsAny<GameObject>()));
        tracker.ObjectEntered += onPointerEnter.Object.Callback;
        tracker.ObjectExited += onPointerExit.Object.Callback;
    }

    protected override void SetupInputSystem()
    {
        mouse = new(InputTestFixture, Camera, Inputs.Standard);
        base.SetupInputSystem();
    }

    [TearDown]
    public override void TearDown()
    {
        tracker.ObjectEntered -= onPointerEnter.Object.Callback;
        tracker.ObjectExited -= onPointerExit.Object.Callback;
        Inputs.Standard.Disable();
        base.TearDown();
    }

    [UnityTest]
    public IEnumerator DoNothing_NoObjectsHovered()
    {
        yield return null;
        AssertHoveredObject(null);
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOne()
    {
        mouse.MoveTo(ObjectOne);
        yield return null;

        AssertHoveredObjects(ObjectOne);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(Times.Once());
        VerifyOnExitEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOneAndTwo()
    {
        mouse.MoveTo(objectOneAndTwo);
        yield return null;

        AssertHoveredObjects(ObjectOne, ObjectTwo);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(ObjectTwo, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(2));
        VerifyOnExitEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOneAndTwo_MouseOverEmptySpace()
    {
        mouse.MoveTo(objectOneAndTwo);
        mouse.MoveTo(EmptySpace);
        yield return null;

        AssertHoveredObjects();
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(ObjectTwo, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(2));
        VerifyOnExitEvent(ObjectOne, Times.Once());
        VerifyOnExitEvent(ObjectTwo, Times.Once());
        VerifyOnExitEvent(Times.Exactly(2));
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOneAndTwo_MouseOverObjectOne()
    {
        mouse.MoveTo(objectOneAndTwo);
        mouse.MoveTo(ObjectOne);
        yield return null;

        AssertHoveredObjects(ObjectOne);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(ObjectTwo, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(2));
        VerifyOnExitEvent(ObjectTwo, Times.Once());
        VerifyOnExitEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOne_MouseOverObjectOneAndTwo()
    {
        mouse.MoveTo(ObjectOne);
        mouse.MoveTo(objectOneAndTwo);
        yield return null;

        AssertHoveredObjects(ObjectOne, ObjectTwo);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(ObjectTwo, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(2));
        VerifyOnExitEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOne_MouseOverObjectTwo()
    {
        mouse.MoveTo(ObjectOne);
        mouse.MoveTo(ObjectTwo);
        yield return null;

        AssertHoveredObjects(ObjectTwo);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(ObjectTwo, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(2));
        VerifyOnExitEvent(ObjectOne, Times.Once());
        VerifyOnExitEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOne_MouseOverEmptySpace_MouseOverObjectOne()
    {
        mouse.MoveTo(ObjectOne);
        mouse.MoveTo(EmptySpace);
        mouse.MoveTo(ObjectOne);
        yield return null;

        AssertHoveredObjects(ObjectOne);
        VerifyOnEnterEvent(ObjectOne, Times.Exactly(2));
        VerifyOnEnterEvent(Times.Exactly(2));
        VerifyOnExitEvent(ObjectOne, Times.Once());
        VerifyOnExitEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOne_MouseOverEmptySpace_MouseOverObjectOneAndTwo()
    {
        mouse.MoveTo(ObjectOne);
        mouse.MoveTo(EmptySpace);
        mouse.MoveTo(objectOneAndTwo);
        yield return null;

        AssertHoveredObjects(ObjectOne, ObjectTwo);
        VerifyOnEnterEvent(ObjectOne, Times.Exactly(2));
        VerifyOnEnterEvent(ObjectTwo, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(3));
        VerifyOnExitEvent(ObjectOne, Times.Once());
        VerifyOnExitEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOneAndTwo_MouseOverEmptySpace_MouseOverObjectOneAndTwo()
    {
        mouse.MoveTo(objectOneAndTwo);
        mouse.MoveTo(EmptySpace);
        mouse.MoveTo(objectOneAndTwo);
        yield return null;

        AssertHoveredObjects(ObjectOne, ObjectTwo);
        VerifyOnEnterEvent(ObjectOne, Times.Exactly(2));
        VerifyOnEnterEvent(ObjectTwo, Times.Exactly(2));
        VerifyOnEnterEvent(Times.Exactly(4));
        VerifyOnExitEvent(ObjectOne, Times.Once());
        VerifyOnExitEvent(ObjectTwo, Times.Once());
        VerifyOnExitEvent(Times.Exactly(2));
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOneAndTwo_MouseOverObjectThreeAndFour()
    {
        mouse.MoveTo(objectOneAndTwo);
        mouse.MoveTo(objectThreeAndFour);
        yield return null;

        AssertHoveredObjects(ObjectThree, ObjectFour);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(ObjectTwo, Times.Once());
        VerifyOnEnterEvent(ObjectThree, Times.Once());
        VerifyOnEnterEvent(ObjectFour, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(4));
        VerifyOnExitEvent(ObjectOne, Times.Once());
        VerifyOnExitEvent(ObjectTwo, Times.Once());
        VerifyOnExitEvent(Times.Exactly(2));
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOne_MoveObjectTwoToObjectOne()
    {
        mouse.MoveTo(ObjectOne);
        ObjectTwo.transform.position = ObjectOne.transform.position;
        yield return null;

        AssertHoveredObjects(ObjectOne, ObjectTwo);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(ObjectTwo, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(2));
        VerifyOnExitEvent(Times.Never());
    }

    private void AssertHoveredObject(GameObject gameObject) =>
        Assert.AreEqual(gameObject, tracker.HoveredObject);

    private void AssertHoveredObjects(params GameObject[] gameObjects)
    {
        GameObject[] expectedGameObjects = new GameObject[PointerOverObjectTracker.MaxObjects];
        gameObjects.CopyTo(expectedGameObjects, 0);
        CollectionAssert.AreEquivalent(expectedGameObjects, tracker.HoveredObjects);
    }

    private void VerifyOnEnterEvent(GameObject gameObject, Times timesCalled) =>
        onPointerEnter.Verify(x => x.Callback(It.Is<GameObject>(s => s == gameObject)), timesCalled);

    private void VerifyOnEnterEvent(Times timesCalled) =>
        onPointerEnter.Verify(x => x.Callback(It.IsAny<GameObject>()), timesCalled);

    private void VerifyOnExitEvent(GameObject gameObject, Times timesCalled) =>
        onPointerExit.Verify(x => x.Callback(It.Is<GameObject>(s => s == gameObject)), timesCalled);

    private void VerifyOnExitEvent(Times timesCalled) =>
        onPointerExit.Verify(x => x.Callback(It.IsAny<GameObject>()), timesCalled);
}
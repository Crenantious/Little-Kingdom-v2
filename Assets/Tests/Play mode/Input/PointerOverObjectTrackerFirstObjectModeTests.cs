using InfoPanelTests;
using LittleKingdom.Input;
using LittleKingdom.PlayModeTests.Utilities;
using Moq;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class PointerOverObjectTrackerFirstObjectModeTests : InputTestsBase
{
    [Inject] private readonly PointerOverObjectTracker tracker;

    private MouseUtilities mouse;
    private GameObject emptySpace;
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

        tracker.SetMode(PointerOverObjectTracker.Mode.TrackFirst);

        // Z of 10 is needed so everything fits in the camera's view
        ObjectOne.transform.position = new(2, 0, 10);
        ObjectTwo.transform.position = new(0, 0, 10);
        EmptySpace.transform.position = new(0, 2, 10);

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
    public IEnumerator DoNothing_NoObjectHovered()
    {
        yield return null;
        AssertHoveredObject(null);
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOne()
    {
        mouse.MoveTo(ObjectOne);
        yield return null;

        AssertHoveredObject(ObjectOne);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(Times.Once());
        VerifyOnExitEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOneThenObjectTwo()
    {
        mouse.MoveTo(ObjectOne);
        mouse.MoveTo(ObjectTwo);
        yield return null;

        AssertHoveredObject(ObjectTwo);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(ObjectTwo, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(2));
        VerifyOnExitEvent(ObjectOne, Times.Once());
        VerifyOnExitEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOneThenEmptySpace()
    {
        mouse.MoveTo(ObjectOne);
        mouse.MoveTo(emptySpace);
        yield return null;

        AssertHoveredObject(null);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(Times.Once());
        VerifyOnExitEvent(ObjectOne, Times.Once());
        VerifyOnExitEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOneThenMoveInsideObjectOne()
    {
        mouse.MoveTo(ObjectOne);
        mouse.MoveTo(ObjectOne, new Vector3(0.1f, 0, 0));
        yield return null;

        AssertHoveredObject(ObjectOne);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(Times.Once());
        VerifyOnExitEvent(Times.Never());
    }

    private void AssertHoveredObject(GameObject gameObject) =>
        Assert.AreEqual(gameObject, tracker.HoveredObject);

    private void VerifyOnEnterEvent(GameObject gameObject, Times timesCalled) =>
        onPointerEnter.Verify(x => x.Callback(It.Is<GameObject>(s => s == gameObject)), timesCalled);

    private void VerifyOnEnterEvent(Times timesCalled) =>
        onPointerEnter.Verify(x => x.Callback(It.IsAny<GameObject>()), timesCalled);

    private void VerifyOnExitEvent(GameObject gameObject, Times timesCalled) =>
        onPointerExit.Verify(x => x.Callback(It.Is<GameObject>(s => s == gameObject)), timesCalled);

    private void VerifyOnExitEvent(Times timesCalled) =>
        onPointerExit.Verify(x => x.Callback(It.IsAny<GameObject>()), timesCalled);
}
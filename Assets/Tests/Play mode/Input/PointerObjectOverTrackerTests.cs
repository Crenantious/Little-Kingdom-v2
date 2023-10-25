using InfoPanelTests;
using LittleKingdom;
using LittleKingdom.Input;
using Moq;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using Zenject;

public class PointerObjectOverTrackerTests : InputTestsBase
{
    [Inject] private readonly PointerObjectHoverTracker tracker;

    private Mouse mouse;
    private GameObject emptySpace;
    private Mock<ITestCallback<GameObject>> onPointerEnter;
    private Mock<ITestCallback<GameObject>> onPointerExit;

    protected override void Install()
    {
        Container.Bind<PointerObjectHoverTracker>().AsSingle();
        base.Install();
    }

    protected override void PostInstall()
    {
        base.PostInstall();

        // Z of 10 is needed so everything fits in the camera's view
        ObjectOne.transform.position = new(2, 0, 10);
        ObjectTwo.transform.position = new(0, 0, 10);

        emptySpace = CreateTestObject(false);
        emptySpace.transform.position = new(0, 2, 10);

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
        mouse = InputSystem.AddDevice<Mouse>();
        Inputs.Standard.Enable();
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
        MoveMouseTo(ObjectOne);
        yield return null;

        AssertHoveredObject(ObjectOne);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(Times.Once());
        VerifyOnExitEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOneThenObjectTwo()
    {
        MoveMouseTo(ObjectOne);
        MoveMouseTo(ObjectTwo);
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
        MoveMouseTo(ObjectOne);
        MoveMouseTo(emptySpace);
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
        MoveMouseTo(ObjectOne);
        MoveMouseTo(ObjectOne, true);
        yield return null;

        AssertHoveredObject(ObjectOne);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(Times.Once());
        VerifyOnExitEvent(Times.Never());
    }

    private void MoveMouseTo(Vector3 position) =>
        InputTestFixture.Move(mouse.position, position);

    private void MoveMouseTo(GameObject gameObject, bool addSmallOffset = false)
    {
        Vector3 position = Camera.WorldToScreenPoint(gameObject.transform.position);
        if (addSmallOffset)
            position += new Vector3(0, 0, 0.01f);
        MoveMouseTo(position);
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
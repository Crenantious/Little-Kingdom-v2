using InfoPanelTests;
using LittleKingdom.Input;
using LittleKingdom.PlayModeTests.Utilities;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Container.BindInterfacesAndSelfTo<PointerOverObjectTracker>().AsSingle();
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
    public IEnumerator MouseOverEmptySpace_NoObjectsHovered()
    {
        MoveMouseTo(EmptySpace);
        yield return null;
        AssertHoveredObject(null);
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOne()
    {
        MoveMouseTo(ObjectOne);
        yield return null;

        AssertHoveredObjects(ObjectOne);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(Times.Once());
        VerifyOnExitEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOneAndTwo()
    {
        MoveMouseTo(objectOneAndTwo);
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
        MoveMouseTo(objectOneAndTwo);
        MoveMouseTo(EmptySpace);
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
        MoveMouseTo(objectOneAndTwo);
        MoveMouseTo(ObjectOne);
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
        MoveMouseTo(ObjectOne);
        MoveMouseTo(objectOneAndTwo);
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
        MoveMouseTo(ObjectOne);
        MoveMouseTo(ObjectTwo);
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
        MoveMouseTo(ObjectOne);
        MoveMouseTo(EmptySpace);
        MoveMouseTo(ObjectOne);
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
        MoveMouseTo(ObjectOne);
        MoveMouseTo(EmptySpace);
        MoveMouseTo(objectOneAndTwo);
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
        MoveMouseTo(objectOneAndTwo);
        MoveMouseTo(EmptySpace);
        MoveMouseTo(objectOneAndTwo);
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
        MoveMouseTo(objectOneAndTwo);
        MoveMouseTo(objectThreeAndFour);
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
        MoveMouseTo(ObjectOne);
        ObjectTwo.transform.position = ObjectOne.transform.position;
        yield return null;
        tracker.FixedTick();
        yield return null;

        AssertHoveredObjects(ObjectOne, ObjectTwo);
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(ObjectTwo, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(2));
        VerifyOnExitEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOne_MoveObjectOneToObjectTwo()
    {
        MoveMouseTo(ObjectOne);
        ObjectOne.transform.position = ObjectTwo.transform.position;
        yield return null;
        tracker.FixedTick();
        yield return null;

        AssertHoveredObjects();
        VerifyOnEnterEvent(ObjectOne, Times.Once());
        VerifyOnEnterEvent(Times.Once());
        VerifyOnExitEvent(ObjectOne, Times.Once());
        VerifyOnExitEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOneAndTwo_ChangeTrackingModeToFirst()
    {
        // Ensure ObjectOne is the closest to the camera.
        ObjectOne.transform.position -= Vector3.forward;
        yield return null;
        MoveMouseTo(objectOneAndTwo);

        tracker.SetMode(PointerOverObjectTracker.Mode.TrackFirst);
        tracker.FixedTick();
        yield return null;

        AssertHoveredObjects();
        Assert.AreEqual(ObjectOne, tracker.HoveredObject);
        VerifyOnEnterEvent(ObjectOne, Times.Exactly(2));
        VerifyOnEnterEvent(ObjectTwo, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(3));
        VerifyOnExitEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator MouseOverTheMaxAmountOfObjects_AllObjectsAreTracked()
    {
        GameObject[] gameObjects = CreateManyGameObjects(PointerOverObjectTracker.MaxObjects);
        yield return null;

        MoveMouseTo(gameObjects[0]);

        AssertHoveredObjects(gameObjects);
        VerifyOnEnterEvent(gameObjects, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(gameObjects.Length));
        VerifyOnExitEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator MouseOverObjectOne_DestroyObjectOneBeforeTrackerUpdates()
    {
        mouse.MoveTo(ObjectOne);
        Object.Destroy(ObjectOne);
        yield return null;

        tracker.FixedTick();

        AssertHoveredObjects();
        VerifyOnEnterEvent(Times.Never());
        VerifyOnExitEvent(Times.Never());
    }

    // This test failed when there was no GameObject null check when invoking events.
    [UnityTest]
    public IEnumerator MouseOverMaxAmountOfObjects_MouseOverObjectOne_DestroyObjectOneBeforeTrackerUpdates()
    {
        GameObject[] gameObjects = CreateManyGameObjects(PointerOverObjectTracker.MaxObjects);
        yield return null;
        MoveMouseTo(gameObjects[0]);

        Object.Destroy(gameObjects[0]);
        // Must be set to null otherwise the CollectionAssert will fail.
        gameObjects[0] = null;

        yield return null;
        tracker.FixedTick();

        AssertHoveredObjects(gameObjects);
        VerifyOnEnterEvent(gameObjects, Times.Once());
        VerifyOnEnterEvent(Times.Exactly(gameObjects.Length));
        VerifyOnExitEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator MouseOverMoreThanMaxAmountOfObjects_AllButOneAreTracked()
    {
        GameObject[] gameObjects = CreateManyGameObjects(PointerOverObjectTracker.MaxObjects + 1);
        yield return null;

        MoveMouseTo(gameObjects[0]);

        List<GameObject> notTracked = new();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (tracker.HoveredObjects.Contains(gameObjects[i]) is false)
            {
                notTracked.Add(gameObjects[i]);
            }
        }

        Assert.AreEqual(notTracked.Count, 1);
        VerifyOnEnterEvent(gameObjects.Except(notTracked), Times.Once());
        VerifyOnEnterEvent(Times.Exactly(gameObjects.Length - 1));
        VerifyOnExitEvent(Times.Never());
    }

    private GameObject[] CreateManyGameObjects(int amount)
    {
        GameObject[] gameObjects = new GameObject[amount];
        for (int i = 0; i < amount; i++)
        {
            gameObjects[i] = CreateTestObject("Object " + i.ToString(), true);
            gameObjects[i].transform.position = ObjectOne.transform.position + new Vector3(0, 2, 0);
        }

        return gameObjects;
    }

    private void MoveMouseTo(GameObject gameObject)
    {
        mouse.MoveTo(gameObject);
        tracker.FixedTick();
    }

    private void AssertHoveredObject(GameObject gameObject) =>
        Assert.AreEqual(gameObject, tracker.HoveredObject);

    private void AssertHoveredObjects(params GameObject[] gameObjects)
    {
        // tracker.HoveredObjects is always the same length so we need to ensure our expected list is the same length.
        GameObject[] expectedGameObjects = new GameObject[PointerOverObjectTracker.MaxObjects];
        gameObjects.CopyTo(expectedGameObjects, 0);
        CollectionAssert.AreEquivalent(expectedGameObjects, tracker.HoveredObjects);
    }

    private void VerifyOnEnterEvent(GameObject gameObject, Times timesCalled) =>
        onPointerEnter.Verify(x => x.Callback(It.Is<GameObject>(s => s == gameObject)), timesCalled);

    private void VerifyOnEnterEvent(IEnumerable<GameObject> gameObjects, Times timesCalled)
    {
        foreach(GameObject gameObject in gameObjects)
        {
            VerifyOnEnterEvent(gameObject, timesCalled);
        }
    }

    private void VerifyOnEnterEvent(Times timesCalled) =>
            onPointerEnter.Verify(x => x.Callback(It.IsAny<GameObject>()), timesCalled);

    private void VerifyOnExitEvent(GameObject gameObject, Times timesCalled) =>
        onPointerExit.Verify(x => x.Callback(It.Is<GameObject>(s => s == gameObject)), timesCalled);

    private void VerifyOnExitEvent(Times timesCalled) =>
        onPointerExit.Verify(x => x.Callback(It.IsAny<GameObject>()), timesCalled);
}
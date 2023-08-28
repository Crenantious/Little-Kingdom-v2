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

public class SelectedObjectTrackerTests : InputTestsBase
{
    [Inject] private readonly SelectedObjectTracker tracker;

    private Mouse mouse;
    private GameObject selectableObject1;
    private GameObject selectableObject2;
    private GameObject nonSelectableObject;
    private GameObject emptySpace;
    private Selectable selectable1;
    private Selectable selectable2;
    private Mock<ITestCallback<Selectable>> onSelected;
    private Mock<ITestCallback<Selectable>> onDeselected;

    protected override void Install()
    {
        Container.Bind<SelectedObjectTracker>().AsSingle();
        base.Install();
    }

    protected override void PostInstall()
    {
        base.PostInstall();

        // Z of 10 is needed so everything fits in the camera's view
        selectableObject1 = CreateTestObject();
        selectable1 = selectableObject1.AddComponent<Selectable>();
        selectableObject1.transform.position = new(2, 0, 10);

        selectableObject2 = CreateTestObject();
        selectable2 = selectableObject2.AddComponent<Selectable>();
        selectableObject2.transform.position = new(0, 0, 10);

        nonSelectableObject = CreateTestObject();
        nonSelectableObject.transform.position = new(-2, 0, 10);

        emptySpace = CreateTestObject(false);
        emptySpace.transform.position = new(0, 2, 10);

        onSelected = new();
        onDeselected = new();
        onSelected.SetupAllProperties();
        onDeselected.SetupAllProperties();
        onSelected.Setup(x => x.Callback(It.IsAny<Selectable>()));
        onDeselected.Setup(x => x.Callback(It.IsAny<Selectable>()));
        tracker.ObjectSelected += onSelected.Object.Callback;
        tracker.ObjectDeselected += onDeselected.Object.Callback;
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
        tracker.ObjectSelected -= onSelected.Object.Callback;
        tracker.ObjectDeselected -= onDeselected.Object.Callback;
        Inputs.Standard.Disable();
        base.TearDown();
    }

    [UnityTest]
    public IEnumerator PressOnSelectable_ReleaseOnSelectable_IsSelected()
    {
        PressAndReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(selectable1);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator PressOnSelectable_ReleaseOnNonSelectable_IsNotSelected()
    {
        PressOn(selectableObject1);
        ReleaseOn(nonSelectableObject);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator PressOnSelectable_ReleaseOnEmptySpace_IsNotSelected()
    {
        PressOn(selectableObject1);
        ReleaseOn(emptySpace);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator SelectSelectable_PressOnSelectable_ReleaseOnSelectable_IsSelectedAndNotReselected()
    {
        Select(selectableObject1);

        PressAndReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(selectable1);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator SelectSelectable_PressOnSelectable_ReleaseOnNonSelectable_IsSelectedAndNotReselected()
    {
        Select(selectableObject1);

        PressOn(selectableObject1);
        ReleaseOn(nonSelectableObject);
        yield return null;

        AssertIsSelected(selectable1);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator SelectSelectable_PressOnSelectable_ReleaseOnNoObject_IsSelectedAndNotReselected()
    {
        Select(selectableObject1);

        PressOn(selectableObject1);
        ReleaseOn(emptySpace);
        yield return null;

        AssertIsSelected(selectable1);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator SelectSelectable_PressOnNonSelectable_ReleaseOnSelectable_IsSelectedAndNotReselected()
    {
        Select(selectableObject1);

        PressOn(nonSelectableObject);
        ReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(selectable1);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator SelectSelectable_PressOnNoObject_ReleaseOnSelectable_IsSelectedAndNotReselected()
    {
        Select(selectableObject1);

        PressOn(emptySpace);
        ReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(selectable1);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator SelectSelectable_PressOnNonSelectable_ReleaseOnNonSelectable_WasDeselected()
    {
        Select(selectableObject1);

        PressAndReleaseOn(nonSelectableObject);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(selectable1, Times.Once());
        VerifyOnDeselectedEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator SelectSelectable_PressOnNoObject_ReleaseOnNoObject_WasDeselected()
    {
        Select(selectableObject1);

        PressAndReleaseOn(emptySpace);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(selectable1, Times.Once());
        VerifyOnDeselectedEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator SelectSelectable_PressOnNonSelectable_ReleaseOnNoObject_WasDeselected()
    {
        Select(selectableObject1);

        PressOn(nonSelectableObject);
        ReleaseOn(emptySpace);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(selectable1, Times.Once());
        VerifyOnDeselectedEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator SelectSelectable_PressOnNoObject_ReleaseOnNonSelectable_WasDeselected()
    {
        Select(selectableObject1);

        PressOn(emptySpace);
        ReleaseOn(nonSelectableObject);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(selectable1, Times.Once());
        VerifyOnDeselectedEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator SelectSelectable1_PressOnSelectable2_ReleaseOnSelectable2_Selectable1WasDeselectedAndSelectable2WasSelected()
    {
        Select(selectableObject1);

        PressAndReleaseOn(selectableObject2);
        yield return null;

        AssertIsSelected(selectable2);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(selectable2, Times.Once());
        VerifyOnSelectedEvent(Times.Exactly(2));
        VerifyOnDeselectedEvent(selectable1, Times.Once());
        VerifyOnDeselectedEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator SelectSelectable1_PressOnSelectable1_ReleaseOnSelectable2_Selectable1WasNotDeselectedOrReselected()
    {
        Select(selectableObject1);

        PressOn(selectableObject1);
        ReleaseOn(selectableObject2);
        yield return null;

        AssertIsSelected(selectable1);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator SelectSelectable1_PressOnSelectable2_ReleaseOnSelectable1_Selectable1WasNotDeselectedOrReselected()
    {
        Select(selectableObject1);

        PressOn(selectableObject2);
        ReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(selectable1);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator SelectSelectable1_PressOnSelectable2_ReleaseNonSelectable_Selectable1WasDeselected()
    {
        Select(selectableObject1);

        PressOn(selectableObject2);
        ReleaseOn(nonSelectableObject);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(selectable1, Times.Once());
        VerifyOnDeselectedEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator SelectSelectable1_PressOnSelectable2_ReleaseNoObject_Selectable1WasDeselected()
    {
        Select(selectableObject1);

        PressOn(selectableObject2);
        ReleaseOn(emptySpace);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(selectable1, Times.Once());
        VerifyOnDeselectedEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator SelectSelectable1_PressOnNonSelectable_ReleaseSelectable2_Selectable1WasDeselected()
    {
        Select(selectableObject1);

        PressOn(nonSelectableObject);
        ReleaseOn(selectableObject2);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(selectable1, Times.Once());
        VerifyOnDeselectedEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator SelectSelectable1_PressOnEmptySpace_ReleaseSelectable2_Selectable1WasDeselected()
    {
        Select(selectableObject1);

        PressOn(emptySpace);
        ReleaseOn(selectableObject2);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(selectable1, Times.Once());
        VerifyOnDeselectedEvent(Times.Once());
    }

    [UnityTest]
    public IEnumerator PressOnSelectable_PressOnSelectableAgain_ReleaseOnNonSelectable_NothingWasSelected()
    {
        PressOn(selectableObject1);
        ReleaseOffScreen();
        PressOn(selectableObject1);
        ReleaseOn(nonSelectableObject);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator PressOnSelectable_ReleaseOnNonSelectable_ReleaseOnSelectable_NothingWasSelected()
    {
        PressOn(selectableObject1);
        ReleaseOn(nonSelectableObject);
        PressOffScreen();
        ReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator PressOnSelectable_PressOnNonSelectable_ReleaseOnSelectable_NothingWasSelected()
    {
        PressOn(selectableObject1);
        ReleaseOffScreen();
        PressOn(nonSelectableObject);
        ReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator PressOnNonSelectable_PressOnSelectable_ReleaseOnSelectable_SelectableWasSelected()
    {
        PressOn(nonSelectableObject);
        ReleaseOffScreen();
        PressOn(selectableObject1);
        ReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(selectable1);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(Times.Never());
    }
    
    [UnityTest]
    public IEnumerator ReleaseOnSelectable_PressOnSelectable_NothingWasSelected()
    {
        PressOffScreen();
        ReleaseOn(selectableObject1);
        PressOn(selectableObject1);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    private void Select(GameObject gameObject) =>
        PressAndReleaseOn(gameObject);

    private void MoveMouseTo(Vector3 position) =>
        InputTestFixture.Move(mouse.position, position);

    private void MoveMouseTo(GameObject gameObject) =>
        MoveMouseTo(Camera.WorldToScreenPoint(gameObject.transform.position));

    private void PressOffScreen()
    {
        // Must be disabled to avoid notifying the object tracker that the mouse was released.
        // It would not get this notification at runtime.
        Inputs.Standard.Disable();
        MoveMouseTo(new Vector3(-1, -1, -1));
        Press();
        Inputs.Standard.Enable();
    }

    private void ReleaseOffScreen()
    {
        // Must be disabled to avoid notifying the object tracker that the mouse was released.
        // It would not get this notification at runtime.
        Inputs.Standard.Disable();
        MoveMouseTo(new Vector3(-1, -1, -1));
        Release();
        Inputs.Standard.Enable();
    }

    private void Press() => InputTestFixture.Press(mouse.leftButton);

    private void Release() => InputTestFixture.Release(mouse.leftButton);

    private void PressOn(GameObject gameObject)
    {
        MoveMouseTo(gameObject);
        Press();
    }

    private void ReleaseOn(GameObject gameObject)
    {
        MoveMouseTo(gameObject);
        Release();
    }

    private void PressAndReleaseOn(GameObject gameObject)
    {
        MoveMouseTo(gameObject);
        Press();
        Release();
    }

    private void VerifyOnSelectedEvent(Selectable selectable, Times timesCalled) =>
        onSelected.Verify(x => x.Callback(It.Is<Selectable>(s => s == selectable)), timesCalled);

    private void VerifyOnSelectedEvent(Times timesCalled) =>
        onSelected.Verify(x => x.Callback(It.IsAny<Selectable>()), timesCalled);

    private void VerifyOnDeselectedEvent(Selectable selectable, Times timesCalled) =>
        onDeselected.Verify(x => x.Callback(It.Is<Selectable>(s => s == selectable)), timesCalled);

    private void VerifyOnDeselectedEvent(Times timesCalled) =>
        onDeselected.Verify(x => x.Callback(It.IsAny<Selectable>()), timesCalled);

    private void AssertIsSelected(Selectable selectable) =>
        Assert.AreEqual(selectable, tracker.Selected);
}
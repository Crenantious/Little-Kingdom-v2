using InfoPanelTests;
using LittleKingdom;
using LittleKingdom.Input;
using LittleKingdom.PlayModeTests.Utilities;
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

    private MouseUtilities mouse;
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
        selectableObject1 = CreateTestObject("SelectableObject1");
        selectable1 = selectableObject1.AddComponent<Selectable>();
        selectableObject1.transform.position = new(2, 0, 10);

        selectableObject2 = CreateTestObject("SelectableObject2");
        selectable2 = selectableObject2.AddComponent<Selectable>();
        selectableObject2.transform.position = new(0, 0, 10);

        nonSelectableObject = CreateTestObject("NonSelectableObject");
        nonSelectableObject.transform.position = new(-2, 0, 10);

        EmptySpace.transform.position = new(0, 2, 10);

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
        mouse = new(InputTestFixture, Camera, Inputs.Standard);
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
        mouse.PressAndReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(selectable1);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator PressOnSelectable_ReleaseOnNonSelectable_IsNotSelected()
    {
        mouse.PressOn(selectableObject1);
        mouse.ReleaseOn(nonSelectableObject);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator PressOnSelectable_ReleaseOnEmptySpace_IsNotSelected()
    {
        mouse.PressOn(selectableObject1);
        mouse.ReleaseOn(emptySpace);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator SelectSelectable_PressOnSelectable_ReleaseOnSelectable_IsSelectedAndNotReselected()
    {
        Select(selectableObject1);

        mouse.PressAndReleaseOn(selectableObject1);
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

        mouse.PressOn(selectableObject1);
        mouse.ReleaseOn(nonSelectableObject);
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

        mouse.PressOn(selectableObject1);
        mouse.ReleaseOn(emptySpace);
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

        mouse.PressOn(nonSelectableObject);
        mouse.ReleaseOn(selectableObject1);
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

        mouse.PressOn(emptySpace);
        mouse.ReleaseOn(selectableObject1);
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

        mouse.PressAndReleaseOn(nonSelectableObject);
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

        mouse.PressAndReleaseOn(emptySpace);
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

        mouse.PressOn(nonSelectableObject);
        mouse.ReleaseOn(emptySpace);
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

        mouse.PressOn(emptySpace);
        mouse.ReleaseOn(nonSelectableObject);
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

        mouse.PressAndReleaseOn(selectableObject2);
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

        mouse.PressOn(selectableObject1);
        mouse.ReleaseOn(selectableObject2);
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

        mouse.PressOn(selectableObject2);
        mouse.ReleaseOn(selectableObject1);
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

        mouse.PressOn(selectableObject2);
        mouse.ReleaseOn(nonSelectableObject);
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

        mouse.PressOn(selectableObject2);
        mouse.ReleaseOn(emptySpace);
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

        mouse.PressOn(nonSelectableObject);
        mouse.ReleaseOn(selectableObject2);
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

        mouse.PressOn(emptySpace);
        mouse.ReleaseOn(selectableObject2);
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
        mouse.PressOn(selectableObject1);
        mouse.ReleaseOffScreen();
        mouse.PressOn(selectableObject1);
        mouse.ReleaseOn(nonSelectableObject);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator PressOnSelectable_ReleaseOnNonSelectable_ReleaseOnSelectable_NothingWasSelected()
    {
        mouse.PressOn(selectableObject1);
        mouse.ReleaseOn(nonSelectableObject);
        mouse.PressOffScreen();
        mouse.ReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator PressOnSelectable_PressOnNonSelectable_ReleaseOnSelectable_NothingWasSelected()
    {
        mouse.PressOn(selectableObject1);
        mouse.ReleaseOffScreen();
        mouse.PressOn(nonSelectableObject);
        mouse.ReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    [UnityTest]
    public IEnumerator PressOnNonSelectable_PressOnSelectable_ReleaseOnSelectable_SelectableWasSelected()
    {
        mouse.PressOn(nonSelectableObject);
        mouse.ReleaseOffScreen();
        mouse.PressOn(selectableObject1);
        mouse.ReleaseOn(selectableObject1);
        yield return null;

        AssertIsSelected(selectable1);
        VerifyOnSelectedEvent(selectable1, Times.Once());
        VerifyOnSelectedEvent(Times.Once());
        VerifyOnDeselectedEvent(Times.Never());
    }
    
    [UnityTest]
    public IEnumerator ReleaseOnSelectable_PressOnSelectable_NothingWasSelected()
    {
        mouse.PressOffScreen();
        mouse.ReleaseOn(selectableObject1);
        mouse.PressOn(selectableObject1);
        yield return null;

        AssertIsSelected(null);
        VerifyOnSelectedEvent(Times.Never());
        VerifyOnDeselectedEvent(Times.Never());
    }

    private void Select(GameObject gameObject) =>
        mouse.PressAndReleaseOn(gameObject);

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
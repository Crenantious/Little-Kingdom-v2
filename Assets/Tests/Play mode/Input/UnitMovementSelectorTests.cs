using InfoPanelTests;
using LittleKingdom.Board;
using LittleKingdom.Input;
using LittleKingdom.PlayModeTests.Utilities;
using LittleKingdom.Units;
using Moq;
using NUnit.Framework;
using PlayModeTests;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using Zenject;

public class UnitMovementSelectorTests : InputTestsBase
{
    [Inject] private readonly UnitMovementSelector movement;

    private MouseUtilities mouse;
    private GameObject unit;
    private GameObject tilePrefab;
    private GameObject tile;
    private GameObject unitSlots;
    private Mock<ITestCallback<GameObject>> onPointerEnter;
    private Mock<ITestCallback<GameObject>> onPointerExit;

    protected override void PreInstall()
    {
        base.PreInstall();

        unit = new GameObject();
        unit.AddComponent<Unit>();
        tilePrefab = TestUtilities.LoadPrefab("Tile");
    }

    protected override void Install()
    {
        Container.Bind<UnitMovementSelector>().AsSingle();
        tile = Container.InstantiatePrefab(tilePrefab);
        base.Install();
    }

    protected override void PostInstall()
    {
        base.PostInstall();

        // Z of 10 is needed so everything fits in the camera's view
        unit.transform.position = new(2, 0, 10);
        tile.transform.position = new(0, 0, 10);

        unitSlots = tile.GetComponent<Tile>().UnitSlots.gameObject;

        onPointerEnter = new();
        onPointerExit = new();
        onPointerEnter.SetupAllProperties();
        onPointerExit.SetupAllProperties();
        onPointerEnter.Setup(x => x.Callback(It.IsAny<GameObject>()));
        onPointerExit.Setup(x => x.Callback(It.IsAny<GameObject>()));
    }

    protected override void SetupInputSystem()
    {
        mouse = new(InputTestFixture, Camera, Inputs.Standard);
        base.SetupInputSystem();
    }

    [TearDown]
    public override void TearDown()
    {
        Inputs.Standard.Disable();
        base.TearDown();
    }

    [UnityTest]
    public IEnumerator ClickOnUnit_UnitSlotsNotShown()
    {
        mouse.PressOn(unit);
        yield return null;
        AssertSlotsActive(false);
    }

    [UnityTest]
    public IEnumerator HoverOverTile_UnitSlotsNotShown()
    {
        mouse.MoveTo(tile);
        yield return null;
        AssertSlotsActive(false);
    }

    [UnityTest]
    public IEnumerator ClickOnUnit_HoverOverTile_UnitSlotsShown()
    {
        mouse.PressOn(unit);
        mouse.MoveTo(tile);
        yield return null;
        AssertSlotsActive(true);
    }

    [UnityTest]
    public IEnumerator HoverOverTile_ClickOnUnit_UnitSlotsShown()
    {
        mouse.MoveTo(tile);
        mouse.PressOn(unit);
        yield return null;
        AssertSlotsActive(true);
    }

    // TODO: JR - add more tests once the hover tracker gets updated.

    private void AssertSlotsActive(bool active) =>
        Assert.AreEqual(active, unitSlots.activeInHierarchy);
}
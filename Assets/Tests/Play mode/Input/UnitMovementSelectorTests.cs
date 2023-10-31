using InfoPanelTests;
using LittleKingdom;
using LittleKingdom.Board;
using LittleKingdom.Input;
using LittleKingdom.PlayModeTests.Utilities;
using LittleKingdom.Units;
using Moq;
using NUnit.Framework;
using PlayModeTests;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class UnitMovementSelectorTests : InputTestsBase
{
    [Inject] private readonly PointerOverObjectTracker pointerHover;

    private MouseUtilities mouse;
    private GameObject unit;
    private GameObject tilePrefab;
    private GameObject tileOne;
    private GameObject tileTwo;
    private TileUnitSlots unitSlotsOne;
    private TileUnitSlots unitSlotsTwo;
    private List<TileUnitSlots> allUnitSlots;
    private Mock<ITestCallback<GameObject>> onPointerEnter;
    private Mock<ITestCallback<GameObject>> onPointerExit;

    protected override void PreInstall()
    {
        base.PreInstall();

        unit = CreateTestObject("Unit");
        unit.AddComponent<Unit>();
        unit.AddComponent<Selectable>();
        tilePrefab = TestUtilities.LoadPrefab("Tile");
    }

    protected override void Install()
    {
        Container.Bind<UnitMovementSelector>().AsSingle();
        Container.Bind<SelectedObjectTracker>().AsSingle();
        Container.Bind<PointerOverObjectTracker>().AsSingle();
        base.Install();
    }

    protected override void PostInstall()
    {
        base.PostInstall();

        (tileOne, unitSlotsOne) = CreateTile("Tile one");
        (tileTwo, unitSlotsTwo) = CreateTile("Tile two");
        allUnitSlots = new() { unitSlotsOne, unitSlotsTwo };

        Camera.transform.SetPositionAndRotation(new(0, 0, 0), Quaternion.Euler(90, 0, 0));
        unit.transform.position = new(0, -10, 0);
        tileOne.transform.position = new(0, -10, 0);
        tileTwo.transform.position = new(2, -10, 0);

        pointerHover.SetMode(PointerOverObjectTracker.Mode.TrackMany);

        onPointerEnter = new();
        onPointerExit = new();
        onPointerEnter.SetupAllProperties();
        onPointerExit.SetupAllProperties();
        onPointerEnter.Setup(x => x.Callback(It.IsAny<GameObject>()));
        onPointerExit.Setup(x => x.Callback(It.IsAny<GameObject>()));
    }

    protected override void SetupInputSystem()
    {
        mouse = new(InputTestFixture, Camera, Inputs.Standard, pointerHover);
        base.SetupInputSystem();
    }

    [TearDown]
    public override void TearDown()
    {
        Inputs.Standard.Disable();
        Object.Destroy(tileOne);
        Object.Destroy(tileTwo);
        base.TearDown();
    }

    [UnityTest]
    public IEnumerator ClickOnUnit_UnitSlotsOneShown()
    {
        mouse.PressAndReleaseOn(unit);
        yield return null;
        pointerHover.FixedTick();
        yield return null;

        AssertSlotsActive(unitSlotsOne);
    }

    [UnityTest]
    public IEnumerator MoveTileOneAway_ClickOnUnit_UnitSlotsOneNotShown()
    {
        tileOne.transform.position += Vector3.left * 2;

        mouse.PressOn(unit);
        pointerHover.FixedTick();
        yield return null;

        AssertSlotsActive();
    }

    [UnityTest]
    public IEnumerator HoverOverTile_UnitSlotsNotShown()
    {
        mouse.MoveTo(tileOne);
        yield return null;

        AssertSlotsActive();
    }

    [UnityTest]
    public IEnumerator ClickOnUnit_HoverOverTileTwo_UnitSlotsShown()
    {
        mouse.PressAndReleaseOn(unit);
        yield return null;

        mouse.MoveTo(tileTwo);
        yield return null;

        AssertSlotsActive(unitSlotsTwo);
    }

    private (GameObject tile, TileUnitSlots slots) CreateTile(string name)
    {
        Tile tile = Container.InstantiatePrefab(tilePrefab).GetComponent<Tile>();
        tile.name = name;
        tile.Initialise(new());

        TileUnitSlots slots = tile.GetComponent<Tile>().UnitSlots;
        slots.name = name + " unit slots";

        return (tile.gameObject, slots);
    }

    private void AssertSlotsActive(params TileUnitSlots[] slots)
    {
        foreach (TileUnitSlots slot in allUnitSlots)
        {
            Assert.AreEqual(slots.Contains(slot), slot.gameObject.activeInHierarchy, $"{slot.name} is active.");
        }
    }
}
using InfoPanelTests;
using LittleKingdom;
using LittleKingdom.Board;
using LittleKingdom.Factories;
using LittleKingdom.Input;
using LittleKingdom.PlayModeTests.Factories;
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
    private TileUnitSlot tileUnitSlotPrefab;
    private GameObject tileOne;
    private GameObject tileTwo;
    private TileUnitSlots unitSlotsOne;
    private TileUnitSlots unitSlotsTwo;
    private List<TileUnitSlots> allUnitSlots;
    private List<(ITileUnitSlot, bool)> unitSlotAvailabilities;
    private Mock<ITestCallback<GameObject>> onPointerEnter;
    private Mock<ITestCallback<GameObject>> onPointerExit;

    protected override void PreInstall()
    {
        base.PreInstall();

        unit = CreateTestObject("Unit");
        unit.AddComponent<Unit>();
        unit.AddComponent<Selectable>();
        tilePrefab = TestUtilities.LoadPrefab("Tile");
        tileUnitSlotPrefab = TestUtilities.LoadPrefab("Tile unit slot").GetComponent<TileUnitSlot>();

        MockTileUnitSlotFactory.ShowAvailabilityCallback = ShowAvailabilityCallback;
        MockTileUnitSlotFactory.HideAvailabilityCallback = HideAvailabilityCallback;
        unitSlotAvailabilities = new();
    }

    protected override void Install()
    {
        Container.Bind<UnitMovementSelector>().AsSingle().NonLazy();
        Container.Bind<SelectedObjectTracker>().AsSingle();
        Container.Bind<PointerOverObjectTracker>().AsSingle();
        Container.BindInstance(tileUnitSlotPrefab).AsSingle();
        Container.BindFactory<ITileUnitSlot, TileUnitSlotFactory>().FromFactory<MockTileUnitSlotFactory>();
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

    #region Unit slots active

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
    public IEnumerator ClickOnUnit_HoverOverTileTwo_OnlyUnitSlotsTwoShown()
    {
        mouse.PressAndReleaseOn(unit);
        yield return null;

        mouse.MoveTo(tileTwo);
        yield return null;

        AssertSlotsActive(unitSlotsTwo);
    }

    [UnityTest]
    public IEnumerator ClickOnUnit_HoverOverEmptySpace_NoUnitSlotsShows()
    {
        mouse.PressAndReleaseOn(unit);
        yield return null;

        mouse.MoveTo(EmptySpace);
        yield return null;

        AssertSlotsActive();
    }

    [UnityTest]
    public IEnumerator ClickOnUnit_HoverOverTileTwo_HoverOverTileOne_OnlyUnitSlotsOneShown()
    {
        mouse.PressAndReleaseOn(unit);
        yield return null;
        mouse.MoveTo(tileTwo);
        yield return null;

        mouse.MoveTo(tileOne);
        yield return null;

        AssertSlotsActive(unitSlotsOne);
    }
    #endregion

    #region Unit slot availability

    [UnityTest]
    public IEnumerator ClickOnUnit_HoverOverUnitSlot_AvailabilityShown()
    {
        mouse.PressAndReleaseOn(unit);
        yield return null;
        MoveMouseToUnitSlot(unitSlotsOne, 0);

        // TODO: JR - fix this once the tests overhaul and input event system fixes have been completed.
        yield return TestHelper.Pause(() => RaycastFromPointer.DrawDebugRay(10, duration: 100));
        yield return null;

        AssertUnitSlotAvailabilities((unitSlotsOne.Slots[0], true));
    }

    #endregion

    private (GameObject tile, TileUnitSlots slots) CreateTile(string name)
    {
        Tile tile = Container.InstantiatePrefab(tilePrefab).GetComponent<Tile>();
        tile.name = name;
        tile.Initialise(new());

        TileUnitSlots slots = tile.GetComponent<Tile>().UnitSlots;
        slots.name = name + " unit slots";

        return (tile.gameObject, slots);
    }

    private void ShowAvailabilityCallback(ITileUnitSlot unitSlot) =>
        unitSlotAvailabilities.Add((unitSlot, true));

    private void HideAvailabilityCallback(ITileUnitSlot unitSlot) =>
        unitSlotAvailabilities.Add((unitSlot, false));

    private void MoveMouseToUnitSlot(TileUnitSlots slots, int slotNumber) =>
        mouse.MoveTo(slots.Slots[slotNumber].Transform.position, false);

    private void AssertUnitSlotAvailabilities(params (ITileUnitSlot, bool)[] unitSlots)
    {
        string expected = GetReadableUnitSlotAvailabilities(unitSlots, "Expected: ");
        string actual = GetReadableUnitSlotAvailabilities(unitSlotAvailabilities, "Actual: ");

        CollectionAssert.AreEqual(unitSlots, unitSlotAvailabilities, $"{expected}\n{actual}");
    }

    private static string GetReadableUnitSlotAvailabilities(IEnumerable<(ITileUnitSlot, bool)> tuples, string prefix)
    {
        string arrayString = prefix;

        for (int i = 0; i < tuples.Count(); i++)
        {
            (ITileUnitSlot slot, bool available) = tuples.ElementAt(i);

            if (i != 0)
            {
                arrayString += ", ";
            }

            string name = string.Join(": ", slot.Transform.parent.parent.name, slot.Transform.gameObject.name);
            arrayString += $"({name}, {available})";
        }

        if (arrayString == prefix)
        {
            arrayString += "empty.";
        }

        return arrayString;
    }

    private void AssertSlotsActive(params TileUnitSlots[] slots)
    {
        foreach (TileUnitSlots slot in allUnitSlots)
        {
            Assert.AreEqual(slots.Contains(slot), slot.gameObject.activeInHierarchy, $"{slot.name} is active.");
        }
    }
}
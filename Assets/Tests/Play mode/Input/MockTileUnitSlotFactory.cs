using LittleKingdom.Board;
using Moq;
using System;
using System.Linq;
using Zenject;

namespace LittleKingdom.PlayModeTests.Factories
{
    public class MockTileUnitSlotFactory : IFactory<ITileUnitSlot>
    {
        public static Action<ITileUnitSlot> ShowAvailabilityCallback;
        public static Action<ITileUnitSlot> HideAvailabilityCallback;

        private readonly DiContainer container;
        private readonly TileUnitSlot unitSlotPrefab;

        public MockTileUnitSlotFactory(DiContainer container, TileUnitSlot unitSlotPrefab)
        {
            this.container = container;
            this.unitSlotPrefab = unitSlotPrefab;
        }

        public ITileUnitSlot Create()
        {
            TileUnitSlot unitSlotMono = container.InstantiatePrefabForComponent<TileUnitSlot>(unitSlotPrefab);

            Mock<ITileUnitSlot> unitSlot = new();
            unitSlot.Setup(s => s.ShowAvailability()).Callback(() => ShowAvailabilityCallback(unitSlot.Object));
            unitSlot.Setup(s => s.HideAvailability()).Callback(() => HideAvailabilityCallback(unitSlot.Object));
            unitSlot.Setup(s => s.Transform).Returns(unitSlotMono.transform);
            unitSlot.SetupAllProperties();
            return unitSlot.Object;
        }
    }
}
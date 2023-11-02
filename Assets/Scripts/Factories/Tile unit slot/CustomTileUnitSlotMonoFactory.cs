using LittleKingdom.Board;
using Zenject;

namespace LittleKingdom.Factories
{
    public class CustomTileUnitSlotMonoFactory : IFactory<ITileUnitSlot>
    {
        private readonly DiContainer container;
        private readonly TileUnitSlot unitSlot;

        public CustomTileUnitSlotMonoFactory(DiContainer container, TileUnitSlot unitSlot)
        {
            this.container = container;
            this.unitSlot = unitSlot;
        }

        public ITileUnitSlot Create() =>
            container.InstantiatePrefabForComponent<TileUnitSlot>(unitSlot);
    }
}
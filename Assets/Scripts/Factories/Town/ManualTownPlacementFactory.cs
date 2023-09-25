using Zenject;

namespace LittleKingdom.Factories
{
    public class ManualTownPlacementFactory : IFactory<ITownPlacement>
    {
        private readonly DiContainer container;
        private readonly TickableManager tickableManager;

        public ManualTownPlacementFactory(DiContainer container, TickableManager tickableManager)
        {
            this.container = container;
            this.tickableManager = tickableManager;
        }

        public ITownPlacement Create()
        {
            ManualTownPlacement placement = container.Instantiate<ManualTownPlacement>();
            tickableManager.Add(placement);
            return placement;
        }
    }
}
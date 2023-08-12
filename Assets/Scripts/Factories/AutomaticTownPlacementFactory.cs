using Zenject;

namespace LittleKingdom.Factories
{
    public class AutomaticTownPlacementFactory : IFactory<ITownPlacement>
    {
        private readonly DiContainer container;

        public AutomaticTownPlacementFactory(DiContainer container) =>
            this.container = container;

        public ITownPlacement Create() =>
            container.Instantiate<AutomaticTownPlacement>();
    }
}
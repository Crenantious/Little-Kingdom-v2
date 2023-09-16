using LittleKingdom.Loading;
using Zenject;

namespace LittleKingdom.Factories
{
    public class PlayerFactory : PlaceholderFactory<IPlayer>
    {
        private readonly DiContainer container;
        private readonly Player player;
        private TurnOrder turnOrder;

        public PlayerFactory(DiContainer container, Player player, TurnOrder turnOrder)
        {
            this.container = container;
            this.player = player;
            this.turnOrder = turnOrder;
        }

        public override IPlayer Create()
        {
            Player newPlayer = container.InstantiatePrefabForComponent<Player>(player);
            newPlayer.Initialise(turnOrder.Count);
            return newPlayer;
        }
    }
}
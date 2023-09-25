using LittleKingdom.CharacterTurns;
using Zenject;

namespace LittleKingdom.Factories
{
    public class CustomPlayerMonoFactory : IFactory<IPlayer>
    {
        private readonly DiContainer container;
        private readonly Player player;
        private readonly CharacterTurnOrder turnOrder;

        public CustomPlayerMonoFactory(DiContainer container, Player player, CharacterTurnOrder turnOrder)
        {
            this.container = container;
            this.player = player;
            this.turnOrder = turnOrder;
        }

        public IPlayer Create()
        {
            Player newPlayer = container.InstantiatePrefabForComponent<Player>(player);
            newPlayer.Initialise(turnOrder.Count);
            return newPlayer;
        }
    }
}
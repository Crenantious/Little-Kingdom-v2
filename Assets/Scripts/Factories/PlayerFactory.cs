using UnityEngine;
using Zenject;

namespace LittleKingdom.Factories
{
    public class PlayerFactory : PlaceholderFactory<IPlayer>
    {
        private readonly DiContainer container;
        private readonly Player player;

        public PlayerFactory(DiContainer container, Player player)
        {
            this.container = container;
            this.player = player;
        }

        public override IPlayer Create() =>
            container.InstantiatePrefabForComponent<IPlayer>(player);
    }
}
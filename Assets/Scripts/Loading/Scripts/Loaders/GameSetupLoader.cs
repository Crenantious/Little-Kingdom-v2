using LittleKingdom.Factories;
using LittleKingdom.Loading;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class GameSetupLoader : Loader<GameSetupLC>
    {
        private IReferences references;
        private PlayerFactory playerFactory;

        [Inject]
        public void Construct(IReferences references, PlayerFactory playerFactory)
        {
            Dependencies = new();
            this.references = references;
            this.playerFactory = playerFactory;
        }

        public override void Load(GameSetupLC config)
        {
            references.GameState = GameState.StandardInGame;

            for (int i = 0; i < config.PlayerCount; i++)
            {
                TurnManager.AddPlayer(playerFactory.Create());
            }
        }

        public override void Unload()
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}
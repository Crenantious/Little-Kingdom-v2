using LittleKingdom.Factories;
using Zenject;

namespace LittleKingdom.Loading
{
    public class GameSetupLoader : Loader<GameSetupLC>
    {
        private IReferences references;
        private PlayerFactory playerFactory;

        [Inject]
        public void Construct(IReferences references, PlayerFactory playerFactory)
        {
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

        public void Unload()
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}
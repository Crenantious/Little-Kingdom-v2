using LittleKingdom.Factories;
using LittleKingdom.Input;
using Zenject;

namespace LittleKingdom.Loading
{
    public class GameSetupLoader : Loader<GameSetupLC>
    {
        private IReferences references;
        private PlayerFactory playerFactory;
        private StandardInput standardInput;
        private TurnOrder turnOrder;

        [Inject]
        public void Construct(IReferences references, PlayerFactory playerFactory, StandardInput standardInput,
            TurnOrder turnOrder)
        {
            this.references = references;
            this.playerFactory = playerFactory;
            this.standardInput = standardInput;
            this.turnOrder = turnOrder;
        }

        public override void Load(GameSetupLC config)
        {
            references.GameState = GameState.StandardInGame;
            standardInput.Enable();

            for (int i = 0; i < config.PlayerCount; i++)
            {
                turnOrder.AddPlayer(playerFactory.Create());
            }
        }

        public void Unload()
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}
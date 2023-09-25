using LittleKingdom.CharacterTurns;
using LittleKingdom.Factories;
using LittleKingdom.Input;
using Zenject;

namespace LittleKingdom.Loading
{
    public class GameSetupLoader : Loader<GameSetupLC>
    {
        private IReferences references;
        private CustomPlayerMonoFactory playerFactory;
        private StandardInput standardInput;
        private CharacterTurnOrder turnOrder;

        [Inject]
        public void Construct(IReferences references, CustomPlayerMonoFactory playerFactory, StandardInput standardInput,
            CharacterTurnOrder turnOrder)
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
                CharacterTurnOrder.AddCharacter(playerFactory.Create());
            }
        }

        public void Unload()
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}
using LittleKingdom.CharacterTurns;
using Zenject;

namespace LittleKingdom.Factories
{
    public class CustomCharacterTurnFactory : IFactory<ICharacter, ICharacterTurn>
    {
        private readonly DiContainer container;

        public CustomCharacterTurnFactory(DiContainer container) =>
            this.container = container;

        public ICharacterTurn Create(ICharacter character)
        {
            var turn = container.Instantiate<CharacterTurn>();
            turn.Initialise(character);
            return turn;
        }
    }
}
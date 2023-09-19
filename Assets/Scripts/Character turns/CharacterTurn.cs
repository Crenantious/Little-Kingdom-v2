namespace LittleKingdom.CharacterTurns
{
    public class CharacterTurn : ICharacterTurn
    {
        private readonly ICharacter character;

        public CharacterTurn(ICharacter character) =>
            this.character = character;

        public void Begin()
        {

        }

        public void End()
        {

        }
    }
}
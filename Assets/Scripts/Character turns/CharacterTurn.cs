using LittleKingdom.Buildings;

namespace LittleKingdom.CharacterTurns
{
    public class CharacterTurn : ICharacterTurn
    {
        private readonly PlayerHUD hud;

        private ICharacter character;

        public CharacterTurn(PlayerHUD hud) =>
            this.hud = hud;

        /// <summary>
        /// Only to be called from a factory.
        /// </summary>
        public void Initialise(ICharacter character) =>
            this.character = character;

        public void Begin()
        {
            hud.Show(character);
        }

        public void End()
        {

        }
    }
}
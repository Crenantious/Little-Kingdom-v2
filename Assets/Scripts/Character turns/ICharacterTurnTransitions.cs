namespace LittleKingdom.CharacterTurns
{
    public interface ICharacterTurnTransitions
    {
        public void BeginFirstTurn();

        public void EndCurrentTurn();
    }
}
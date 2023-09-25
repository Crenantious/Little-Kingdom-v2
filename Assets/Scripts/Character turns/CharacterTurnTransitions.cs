using Assets.Scripts.Exceptions;

namespace LittleKingdom.CharacterTurns
{
    public class CharacterTurnTransitions : ICharacterTurnTransitions
    {
        private const string NoCharactersRegisteredError = "No characters registered with {0} thus cannot start any character turns.";
        private const string NoCharacterTurnStartedError = "No character turn started thus none to end.";

        private readonly CharacterTurnOrder turnOrder;

        public CharacterTurnTransitions(CharacterTurnOrder turnOrder)
        {
            this.turnOrder = turnOrder;
            turnOrder.ShouldWrap = true;
        }

        public void BeginFirstTurn()
        {
            // Throws if none registered.
            VerifyCharactersRegistered();

            turnOrder.Reset();
            turnOrder.MoveNext();
            turnOrder.Current.Turn.Begin();
        }

        public void EndCurrentTurn()
        {
            // Throws is none exist.
            VerifyAnActiveTurnExists();

            turnOrder.Current.Turn.End();
            turnOrder.MoveNext();
            turnOrder.Current.Turn.Begin();
        }

        private void VerifyCharactersRegistered()
        {
            if (turnOrder.Count == 0)
                throw new NoCharactersRegisteredException(NoCharactersRegisteredError.FormatConst(nameof(CharacterTurnTransitions)));
        }

        private void VerifyAnActiveTurnExists()
        {
            if (turnOrder.Current is null)
                throw new NoCharacterTurnStartedException(NoCharacterTurnStartedError);
        }
    }
}
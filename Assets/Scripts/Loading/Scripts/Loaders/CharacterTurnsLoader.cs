using LittleKingdom.CharacterTurns;
using Zenject;

namespace LittleKingdom.Loading
{
    public class CharacterTurnsLoader : Loader<CharacterTurnsLC>
    {
        private CharacterTurnTransitions transitions;

        [Inject]
        public void Construct(CharacterTurnTransitions transitions) =>
            this.transitions = transitions;

        public override void Load(CharacterTurnsLC config) =>
            transitions.BeginFirstTurn();

        public void Unload()
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}
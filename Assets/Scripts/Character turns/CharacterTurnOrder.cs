using System.Collections;
using System.Collections.Generic;

namespace LittleKingdom.CharacterTurns
{
    public class CharacterTurnOrder : IEnumerator<ICharacter>
    {
        private static readonly List<ICharacter> characters = new();

        private int characterIndex = -1;

        /// <summary>
        /// Defaults to <see langword="null"/>, must call <see cref="MoveNext"/> first.
        /// </summary>
        public ICharacter Current { get; private set; }

        ///<inheritdoc cref="Current"/>
        object IEnumerator.Current => Current;

        public IEnumerator<ICharacter> GetEnumerator() => this;

        public int Count => characters.Count;

        /// <summary>
        /// When the last character has been reached:<br/>
        /// If true, <see cref="MoveNext"/> will move to the first character and return true.<br/>
        /// If false, <see cref="MoveNext"/> will move to a null character and return false.
        /// </summary>
        public bool ShouldWrap { get; set; }

        public static void AddCharacter(ICharacter character) =>
            characters.Add(character);

        public static void RemoveAllCharacters() =>
            characters.Clear();

        public bool MoveNext()
        {
            characterIndex++;
            if (characterIndex >= characters.Count)
            {
                if (ShouldWrap)
                    characterIndex = 0;
                else
                {
                    Current = null;
                    return false;
                }
            }

            Current = characters[characterIndex];
            return true;
        }

        /// <summary>
        /// Sets the enumeration to the starting state.
        /// </summary>
        public void Reset()
        {
            characterIndex = -1;
            Current = null;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
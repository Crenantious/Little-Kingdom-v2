using System;

namespace Assets.Scripts.Exceptions
{
    public class NoCharacterTurnStartedException : Exception
    {
        public NoCharacterTurnStartedException(string message) : base(message) { }
    }
}
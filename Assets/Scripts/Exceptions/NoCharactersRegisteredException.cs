using System;

namespace Assets.Scripts.Exceptions
{
    public class NoCharactersRegisteredException : Exception
    {
        public NoCharactersRegisteredException(string message) : base(message) { }
    }
}
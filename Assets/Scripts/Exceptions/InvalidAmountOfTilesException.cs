using System;

namespace Assets.Scripts.Exceptions
{
    public class InvalidAmountOfTilesException : Exception
    {
        public InvalidAmountOfTilesException(string message) : base(message) { }
    }
}
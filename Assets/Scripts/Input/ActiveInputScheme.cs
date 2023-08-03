using System.Collections.Generic;

namespace LittleKingdom.Input
{
    public static class ActiveInputScheme
    {
        private readonly static Stack<IInputScheme> activeSchemes = new();

        /// <summary>
        /// Enables <paramref name="inputScheme"/>, and disables the previous.
        /// </summary>
        public static void Set(IInputScheme inputScheme)
        {
            if (activeSchemes.TryPeek(out IInputScheme result))
                result.Disable();

            activeSchemes.Push(inputScheme);
            inputScheme.Enable();
        }

        /// <summary>
        /// Disables the current scheme, and enables the previous.
        /// </summary>
        public static void SetPrevious()
        {
            if (activeSchemes.Count < 2)
                return;

            activeSchemes.Pop();
            activeSchemes.Peek().Enable();
        }
    }
}
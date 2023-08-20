using UnityEngine;

namespace LittleKingdom.Input
{
    public interface IStandardInput : IInputScheme
    {
        /// <summary>
        /// Called when a tap is registered on the screen. Mouse click and release, touch and release etc.
        /// </summary>
        public event SimpleEventHandler PointerTap;

        public Vector2 GetPointerPosition();
    }
}
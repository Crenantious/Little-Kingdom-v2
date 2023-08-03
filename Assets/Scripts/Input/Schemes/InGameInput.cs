using UnityEngine;
using UnityEngine.InputSystem;
using static LittleKingdom.Input.Inputs;

namespace LittleKingdom.Input
{
    public class InGameInput : IInputScheme
    {
        private InGameActions actions;

        public delegate void PointerTapHandler();

        /// <summary>
        /// Called when a tap is registered on the screen. Mouse click, touch etc.
        /// </summary>
        public event PointerTapHandler PointerTap;

        public InGameInput(Inputs inputs)
        {

            actions = inputs.InGame;
            actions.PointerTap.performed += OnPointerTap;
        }

        /// <inheritdoc/>
        public void Enable() =>
            actions.Enable();

        /// <inheritdoc/>
        public void Disable() =>
            actions.Disable();

        public Vector2 GetPointerPosition() =>
            actions.Point.ReadValue<Vector2>();
        private void OnPointerTap(InputAction.CallbackContext context)
        {
            PointerTap.Invoke();
        }
    }
}
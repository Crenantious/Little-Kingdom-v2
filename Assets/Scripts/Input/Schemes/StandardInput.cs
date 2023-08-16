using UnityEngine;
using UnityEngine.InputSystem;
using static LittleKingdom.Input.Inputs;

namespace LittleKingdom.Input
{
    public class StandardInput : IInputScheme
    {
        private StandardActions actions;

        /// <summary>
        /// Called when a tap is registered on the screen. Mouse click and release, touch and release etc.
        /// </summary>
        public event SimpleEventHandler PointerTap;

        public StandardInput(Inputs inputs)
        {
            actions = inputs.Standard;
            actions.PointerTap.performed += OnPointerTap;
        }

        public void Enable() =>
            actions.Enable();

        public void Disable() =>
            actions.Disable();

        public Vector2 GetPointerPosition() =>
            actions.PointerPosition.ReadValue<Vector2>();

        private void OnPointerTap(InputAction.CallbackContext context)
        {
            PointerTap.Invoke();
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using static LittleKingdom.Input.Inputs;

namespace LittleKingdom.Input
{
    public class StandardInput : IInputScheme
    {
        private StandardActions actions;

        public event SimpleEventHandler PointerTap;
        public event SimpleEventHandler PointerPress;
        public event SimpleEventHandler PointerRelease;

        public StandardInput(Inputs inputs)
        {
            actions = inputs.Standard;
            actions.PointerTap.performed += OnPointerTap;
            actions.PointerPress.performed += OnPointerPress;
            actions.PointerRelease.performed += OnPointerRelease;
        }

        public void Enable() =>
            actions.Enable();

        public void Disable() =>
            actions.Disable();

        public Vector2 GetPointerPosition() =>
            actions.PointerPosition.ReadValue<Vector2>();

        private void OnPointerTap(InputAction.CallbackContext context) =>
            PointerTap?.Invoke();

        private void OnPointerPress(InputAction.CallbackContext context) =>
            PointerPress?.Invoke();

        private void OnPointerRelease(InputAction.CallbackContext context) =>
            PointerRelease?.Invoke();
    }
}
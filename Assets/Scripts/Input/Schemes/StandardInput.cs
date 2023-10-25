using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static LittleKingdom.Input.Inputs;

namespace LittleKingdom.Input
{
    public class StandardInput : IInputScheme
    {
        private StandardActions actions;

        public event SimpleEventHandler PointerPressAndRelease;
        public event SimpleEventHandler PointerPress;
        public event SimpleEventHandler PointerRelease;
        public event SimpleEventHandler PointerMoved;

        public StandardInput(Inputs inputs)
        {
            actions = inputs.Standard;
            actions.PointerPressAndRelease.performed += OnPointerPressAndRelease;
            actions.PointerPress.performed += OnPointerPress;
            actions.PointerRelease.performed += OnPointerRelease;
            actions.PointerPosition.performed += OnPointerMoved;
        }

        public void Enable() =>
            actions.Enable();

        public void Disable() =>
            actions.Disable();

        public Vector2 GetPointerPosition() =>
            actions.PointerPosition.ReadValue<Vector2>();

        private void OnPointerPressAndRelease(InputAction.CallbackContext context) =>
            PointerPressAndRelease?.Invoke();

        private void OnPointerPress(InputAction.CallbackContext context) =>
            PointerPress?.Invoke();

        private void OnPointerRelease(InputAction.CallbackContext context) =>
            PointerRelease?.Invoke();

        private void OnPointerMoved(InputAction.CallbackContext context) =>
            PointerMoved?.Invoke();
    }
}
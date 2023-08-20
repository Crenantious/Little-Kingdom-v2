using UnityEngine;
using UnityEngine.InputSystem;
using static LittleKingdom.Input.Inputs;

namespace LittleKingdom.Input
{
    public class StandardInput : IStandardInput
    {
        private StandardActions actions;

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
            PointerTap?.Invoke();
        }
    }
}
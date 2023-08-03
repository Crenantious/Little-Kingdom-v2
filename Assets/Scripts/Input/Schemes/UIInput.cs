using UnityEngine;
using UnityEngine.InputSystem;
using static LittleKingdom.Input.Inputs;

namespace LittleKingdom.Input
{
    public class UIInput : IInputScheme
    {
        private UIActions actions;

        public delegate void PointerTapHandler();
        public event PointerTapHandler PointerTap;

        public UIInput(Inputs inputs)
        {

            actions = inputs.UI;
            //actions.PointerTap.performed += OnPointerTap;
        }

        public void Enable() =>
            actions.Enable();

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
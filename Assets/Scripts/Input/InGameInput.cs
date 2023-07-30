using UnityEngine;
using Zenject;
using static LittleKingdom.Input.Inputs;

namespace LittleKingdom.Input
{
    public class InGameInput : IInputScheme
    {
        private InGameActions actions;

        [Inject]
        public InGameInput(Inputs inputs) =>
            actions = inputs.InGame;

        public void Enable() =>
            actions.Enable();

        public Vector2 GetPointerPosition() =>
            actions.Point.ReadValue<Vector2>();
    }
}
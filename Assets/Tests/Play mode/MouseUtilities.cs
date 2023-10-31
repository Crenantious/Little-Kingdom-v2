using LittleKingdom.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LittleKingdom.PlayModeTests.Utilities
{
    public class MouseUtilities
    {
        private readonly Mouse mouse;
        private readonly InputTestFixture inputTestFixture;
        private readonly Camera camera;
        private readonly Inputs.StandardActions actions;
        private readonly PointerOverObjectTracker pointerOverObjectTracker;

        public MouseUtilities(InputTestFixture inputTestFixture,
                              Camera camera, Inputs.StandardActions actions,
                              PointerOverObjectTracker pointerOverObjectTracker = null)
        {
            this.inputTestFixture = inputTestFixture;
            this.camera = camera;
            this.actions = actions;
            this.pointerOverObjectTracker = pointerOverObjectTracker;
            mouse = InputSystem.AddDevice<Mouse>();
            actions.Enable();
        }

        public void MoveTo(Vector3 position, bool tickTracker = true)
        {
            inputTestFixture.Move(mouse.position, position);

            if (tickTracker)
                pointerOverObjectTracker?.FixedTick();
        }

        public void MoveTo(GameObject gameObject, Vector3? offset = null, bool tickTracker = true)
        {
            offset ??= Vector3.zero;
            Vector3 position = camera.WorldToScreenPoint(gameObject.transform.position + (Vector3)offset);
            MoveTo(position, tickTracker);
        }

        public void Press() =>
            inputTestFixture.Press(mouse.leftButton);

        public void Release() =>
            inputTestFixture.Release(mouse.leftButton);

        public void PressOn(GameObject gameObject, bool tickTracker = false)
        {
            MoveTo(gameObject, tickTracker: tickTracker);
            Press();
        }

        public void ReleaseOn(GameObject gameObject, bool tickTracker = false)
        {
            MoveTo(gameObject, tickTracker: tickTracker);
            Release();
        }

        public void PressAndReleaseOn(GameObject gameObject, bool tickTracker = false)
        {
            MoveTo(gameObject, tickTracker: tickTracker);
            Press();
            Release();
        }

        public void PressOffScreen(bool tickTracker = false)
        {
            // Must be disabled to avoid notifying the object tracker that the mouse was released.
            // It would not get this notification at runtime.
            actions.Disable();
            MoveTo(new Vector3(-1, -1, -1), tickTracker);
            Press();
            actions.Enable();
        }

        public void ReleaseOffScreen(bool tickTracker = false)
        {
            // Must be disabled to avoid notifying the object tracker that the mouse was released.
            // It would not get this notification at runtime.
            actions.Disable();
            MoveTo(new Vector3(-1, -1, -1), tickTracker);
            Release();
            actions.Enable();
        }
    }
}
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

        public MouseUtilities(InputTestFixture inputTestFixture,
                              Camera camera, Inputs.StandardActions actions)
        {
            this.inputTestFixture = inputTestFixture;
            this.camera = camera;
            this.actions = actions;

            mouse = InputSystem.AddDevice<Mouse>();
            actions.Enable();
        }

        public void MoveTo(Vector3 position) =>
            inputTestFixture.Move(mouse.position, position);

        public void MoveTo(GameObject gameObject) =>
            MoveTo(camera.WorldToScreenPoint(gameObject.transform.position));

        public void Press() =>
            inputTestFixture.Press(mouse.leftButton);

        public void Release() =>
            inputTestFixture.Release(mouse.leftButton);

        public void PressOn(GameObject gameObject)
        {
            MoveTo(gameObject);
            Press();
        }

        public void ReleaseOn(GameObject gameObject)
        {
            MoveTo(gameObject);
            Release();
        }

        public void PressAndReleaseOn(GameObject gameObject)
        {
            MoveTo(gameObject);
            Press();
            Release();
        }

        public void PressOffScreen()
        {
            // Must be disabled to avoid notifying the object tracker that the mouse was released.
            // It would not get this notification at runtime.
            actions.Disable();
            MoveTo(new Vector3(-1, -1, -1));
            Press();
            actions.Enable();
        }

        public void ReleaseOffScreen()
        {
            // Must be disabled to avoid notifying the object tracker that the mouse was released.
            // It would not get this notification at runtime.
            actions.Disable();
            MoveTo(new Vector3(-1, -1, -1));
            Release();
            actions.Enable();
        }
    }
}
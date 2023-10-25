using UnityEngine;

namespace LittleKingdom.Input
{
    public class PointerObjectHoverTracker
    {
        private readonly StandardInput input;
        private readonly RaycastFromPointer raycastFromPointer;

        public GameObject HoveredObject { get; private set; }

        public event SimpleEventHandler<GameObject> ObjectEntered;
        public event SimpleEventHandler<GameObject> ObjectExited;

        public PointerObjectHoverTracker(StandardInput input, RaycastFromPointer raycastFromPointer)
        {
            this.input = input;
            this.raycastFromPointer = raycastFromPointer;
            this.input.PointerMoved += OnPointerMoved;
        }

        private void OnPointerMoved()
        {
            GameObject newHoveredObject = GetObjectUnderPointer();

            if (HoveredObject == newHoveredObject)
                return;

            InvokeEvents(newHoveredObject);
            HoveredObject = newHoveredObject;
        }

        private GameObject GetObjectUnderPointer()
        {
            if (raycastFromPointer.IsPointerOverUIElement())
                return null;

            return raycastFromPointer.CastTo3D(out RaycastHit hit) ?
                hit.collider.gameObject : null;
        }

        private void InvokeEvents(GameObject objectUnderPointer)
        {
            if (HoveredObject != null)
                ObjectExited.Invoke(HoveredObject);

            if (objectUnderPointer != null)
                ObjectEntered.Invoke(objectUnderPointer);
        }
    }
}
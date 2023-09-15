using UnityEngine;

namespace LittleKingdom.Input
{
    // TODO: JR - it's possible for the user to press inside,
    // release outside, press outside then release inside.
    // Currently this acts as though they pressed and release inside.
    // Not sure how to detect this, probably just ignore it.
    public class SelectedObjectTracker
    {
        private readonly StandardInput input;
        private readonly RaycastFromPointer raycastFromPointer;

        private Selectable objectPressedOn;
        private Selectable objectReleasedOn;
        private bool isPointerPressed = false;

        public Selectable Selected { get; private set; }

        public event SimpleEventHandler<Selectable> ObjectSelected;
        public event SimpleEventHandler<Selectable> ObjectDeselected;

        public SelectedObjectTracker(StandardInput input, RaycastFromPointer raycastFromPointer)
        {
            this.input = input;
            this.raycastFromPointer = raycastFromPointer;
            this.input.PointerPress += OnPointerPress;
            this.input.PointerRelease += OnPointerRelease;
        }

        private void OnPointerPress()
        {
            if (raycastFromPointer.IsPointerOverUIElement())
                return;

            objectPressedOn = GetSelectableUnderPointer();
            isPointerPressed = true;
        }

        private void OnPointerRelease()
        {
            // This means the user pressed the pointer outside the
            // game window then released it inside, so we ignore it.
            if (isPointerPressed is false)
                return;

            if (raycastFromPointer.IsPointerOverUIElement())
            {
                objectPressedOn = null;
                return;
            }

            objectReleasedOn = GetSelectableUnderPointer();

            DetermineSelectedObject();
            isPointerPressed = false;
        }

        private Selectable GetSelectableUnderPointer() =>
            raycastFromPointer.CastTo3D(out RaycastHit hit) &&
                IsSelectable(hit.collider.gameObject, out Selectable selectable) ?
                selectable :
                null;

        private static bool IsSelectable(GameObject gameObject, out Selectable selectable)
        {
            selectable = gameObject.GetComponent<Selectable>();
            return selectable != null;
        }

        private void DetermineSelectedObject()
        {
            if (IsThereASelectedObject())
            {
                if (WasPressedOrReleasedOn(Selected))
                    return;
                DeselectObject();
            }

            if (objectPressedOn != null && objectPressedOn == objectReleasedOn)
                SelectObject();
        }

        private void SelectObject()
        {
            Selected = objectPressedOn;
            ObjectSelected?.Invoke(Selected);
            Debug.Log("Selected: " + Selected.name);
        }

        private void DeselectObject()
        {
            ObjectDeselected?.Invoke(Selected);
            Selected = null;
        }

        private bool IsThereASelectedObject() => Selected != null;

        private bool WasPressedOrReleasedOn(Selectable selectable) =>
            objectPressedOn == selectable || objectReleasedOn == selectable;
    }
}
using LittleKingdom.Board;
using LittleKingdom.Input;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Units
{
    public class UnitMovementSelector
    {
        private PointerOverObjectTracker pointerHover;
        private Tile tile;
        private TileUnitSlot unitSlot;

        [Inject]
        public void Construct(SelectedObjectTracker pointerSelection, PointerOverObjectTracker pointerHover)
        {
            this.pointerHover = pointerHover;

            pointerSelection.ObjectSelected += OnObjectSelected;
            pointerSelection.ObjectDeselected += OnObjectDeselected;
        }

        private void OnObjectSelected(Selectable selectable)
        {
            if (selectable.TryGetComponent(out Unit _))
            {
                pointerHover.ObjectEntered += OnObjectEnter;
                pointerHover.ObjectExited += OnObjectExit;
                pointerHover.SetMode(PointerOverObjectTracker.Mode.TrackMany);
            }
        }

        private void OnObjectDeselected(Selectable selectable)
        {
            if (selectable.TryGetComponent(out Unit _))
            {
                pointerHover.ObjectEntered -= OnObjectEnter;
                pointerHover.ObjectExited -= OnObjectExit;
                pointerHover.SetMode(PointerOverObjectTracker.Mode.TrackFirst);
            }
        }

        private void OnObjectEnter(GameObject gameObject)
        {
            if (tile && gameObject.TryGetComponent(out unitSlot))
                unitSlot.ShowAvailability();

            else if (gameObject.TryGetComponent(out tile))
                tile.UnitSlots.gameObject.SetActive(true);
        }

        private void OnObjectExit(GameObject gameObject)
        {
            if (gameObject == unitSlot)
                unitSlot.HideAvailability();

            else if (gameObject == tile)
                tile.UnitSlots.gameObject.SetActive(false);
        }
    }
}
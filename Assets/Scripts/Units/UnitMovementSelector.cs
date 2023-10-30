using LittleKingdom.Board;
using LittleKingdom.Input;
using System;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Units
{
    public class UnitMovementSelector : MonoBehaviour
    {
        private Tile hoveredTile;
        private Unit selectedUnit;

        [Inject]
        public void Construct(SelectedObjectTracker selectionTracker, PointerOverObjectTracker hoverTracker)
        {
            selectionTracker.ObjectSelected += OnObjectSelected;
            selectionTracker.ObjectDeselected += OnObjectDeselected;
            hoverTracker.ObjectEntered += ObjectEntered;
            hoverTracker.ObjectExited += ObjectExited;
        }

        private void OnObjectSelected(Selectable selectable)
        {
            if (selectable.TryGetComponent(out Unit unit))
                selectedUnit = unit;
        }

        private void OnObjectDeselected(Selectable selectable)
        {
            if (selectable.TryGetComponent(out Unit _))
                selectedUnit = null;
        }

        private void ObjectEntered(GameObject gameObject)
        {
            if (selectedUnit != null && gameObject.TryGetComponent(out TileUnitSlots slots))
                slots.gameObject.SetActive(true);
        }

        private void ObjectExited(GameObject args)
        {
            if (selectedUnit != null && gameObject.TryGetComponent(out TileUnitSlots slots))
                slots.gameObject.SetActive(false);
        }
    }
}
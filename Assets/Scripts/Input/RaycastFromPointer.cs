using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LittleKingdom.Input
{
    // TODO: JR - probably use an interface for testing. Or a prefab for the EventSystem.
    public class RaycastFromPointer
    {
        private readonly StandardInput input;
        private readonly IReferences references;

        public RaycastFromPointer(StandardInput input, IReferences references)
        {
            this.input = input;
            this.references = references;
        }

        /// <summary>
        /// Raycasts from the pointer position.
        /// </summary>
        /// <param name="hit">The collision result, if any.</param>
        /// <param name="ignoreUI">If true, UI elements are ignored. Thus <see cref="GameObject"/>s can be selected through UI elements.<br/>
        /// If false and the pointer is over a UI element, a collision will not occur.</param>
        /// <returns>Weather or not a collision occurred.</returns>
        public bool CastTo3D(out RaycastHit hit, bool ignoreUI = false, float maxDistance = 100)
        {
            // This means the pointer is over a UI element.
            if (ignoreUI is false && IsPointerOverUIElement())
            {
                hit = new();
                return false;
            }

            Ray ray = references.ActiveCamera.ScreenPointToRay(input.GetPointerPosition());
            return Physics.Raycast(ray, out hit, maxDistance);
        }

        /// <summary>
        /// Raycasts from the pointer to all UI elements.
        /// </summary>
        /// <param name="results">All collision results with UI elements that are under the pointer.</param>
        /// <returns>Weather or not a collision occurred.</returns>
        public bool CastToUI(List<RaycastResult> results)
        {
            PointerEventData eventDataCurrentPosition = new(EventSystem.current)
            {
                position = input.GetPointerPosition()
            };
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public bool IsPointerOverUIElement() => CastToUI(new());
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Input
{
    public class InputUtility
    {
        private readonly Dictionary<GameObject, int> clickThroughObjectsPreviousLayer = new();

        private readonly IReferences references;

        public InputUtility(IReferences references) =>
            this.references = references;

        /// <summary>
        /// Caches the current layer then sets the layer to ignore raycasts.
        /// </summary>
        public void EnableClickThrough(GameObject gameObject)
        {
            clickThroughObjectsPreviousLayer[gameObject] = gameObject.layer;
            gameObject.layer = references.IgnoreRaycastLayer;
        }

        /// <summary>
        /// The layer is set to that cached from <see cref="EnableClickThrough(GameObject)"/> if there is one,
        /// default otherwise.
        /// </summary>
        public void DisableClickThrough(GameObject gameObject)
        {
            int layer = clickThroughObjectsPreviousLayer.ContainsKey(gameObject) ?
                    clickThroughObjectsPreviousLayer[gameObject] :
                    references.DefaultLayer;

            gameObject.layer = layer;
        }

        public bool RaycastFromPointer(Vector2 position, out RaycastHit hit)
        {
            Ray ray = references.ActiveCamera.ScreenPointToRay(position);
            return Physics.Raycast(ray, out hit, 100);
        }
    }
}
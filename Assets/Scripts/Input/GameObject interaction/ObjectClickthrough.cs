using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Input
{
    public class ObjectClickthrough
    {
        private readonly Dictionary<GameObject, int> clickThroughObjectsPreviousLayer = new();

        private readonly IReferences references;

        public ObjectClickthrough(IReferences references) =>
            this.references = references;

        /// <summary>
        /// Caches the current layer then sets the layer to ignore raycasts.
        /// </summary>
        public void Enable(GameObject gameObject)
        {
            clickThroughObjectsPreviousLayer[gameObject] = gameObject.layer;
            gameObject.layer = references.IgnoreRaycastLayer;
        }

        /// <summary>
        /// The layer is set to that cached from <see cref="Enable(GameObject)"/> if there is one,
        /// default otherwise.
        /// </summary>
        public void Disable(GameObject gameObject)
        {
            int layer = clickThroughObjectsPreviousLayer.ContainsKey(gameObject) ?
                    clickThroughObjectsPreviousLayer[gameObject] :
                    references.DefaultLayer;

            gameObject.layer = layer;
        }
    }
}
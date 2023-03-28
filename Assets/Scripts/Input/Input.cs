using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom
{
    public class Input
    {
        private readonly Dictionary<GameObject, int> clickThroughObjectsPreviousLayer = new();

        /// <summary>
        /// Caches the current layer then sets the layer to ignore raycasts.
        /// </summary>
        public void EnableClickThrough(GameObject gameObject)
        {
            clickThroughObjectsPreviousLayer[gameObject] = gameObject.layer;
            gameObject.layer = References.IgnoreRaycastLayer;
        }

        /// <summary>
        /// The layer is set to that cached from <see cref="EnableClickThrough(GameObject)"/> if there is one,
        /// default otherwise.
        /// </summary>
        public void DisableClickThrough(GameObject gameObject)
        {
            int layer = clickThroughObjectsPreviousLayer.ContainsKey(gameObject) ?
                    clickThroughObjectsPreviousLayer[gameObject] :
                    References.DefaultLayer;

            gameObject.layer = layer;
        }
    }
}
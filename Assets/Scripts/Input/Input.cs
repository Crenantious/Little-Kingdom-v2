using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Input
{
    public static class Input
    {
        private static readonly Dictionary<GameObject, int> clickThroughObjectsPreviousLayer = new();

        /// <summary>
        /// Caches the current layer then sets the layer to ignore raycasts.
        /// </summary>
        public static void EnableClickThrough(GameObject gameObject)
        {
            clickThroughObjectsPreviousLayer[gameObject] = gameObject.layer;
            gameObject.layer = References.IgnoreRaycastLayer;
        }

        /// <summary>
        /// The layer is set to that cached from <see cref="EnableClickThrough(GameObject)"/> if there is one,
        /// default otherwise.
        /// </summary>
        public static void DisableClickThrough(GameObject gameObject)
        {
            int layer = clickThroughObjectsPreviousLayer.ContainsKey(gameObject) ?
                    clickThroughObjectsPreviousLayer[gameObject] :
                    References.DefaultLayer;

            gameObject.layer = layer;
        }
    }
}
using LittleKingdom;
using System;
using UnityEngine;

namespace PlayModeTests
{
    public class TestUtilities
    {
        /// <summary>
        /// Loads the object with <paramref name="name"/> from assets. This is not an instance of the object.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static GameObject LoadPrefab(string name) =>
            AssetUtilities.GetAsset<GameObject>(name, "t: prefab");
    }
}
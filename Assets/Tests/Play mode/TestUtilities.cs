using System;
using UnityEditor;
using UnityEngine;

namespace PlayModeTests
{
    public class TestUtilities
    {
        public static GameObject LoadPrefab(string name)
        {
            var guids = AssetDatabase.FindAssets($"{name} t:GameObject");
            if (guids.Length == 0)
                throw new ArgumentException($"A prefab could not be found with the name {name}.");

            return AssetDatabase.LoadAssetAtPath<GameObject>(
                AssetDatabase.GUIDToAssetPath(guids[0]));
        }
    }
}
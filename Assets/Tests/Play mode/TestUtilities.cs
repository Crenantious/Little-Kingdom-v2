using UnityEditor;
using UnityEngine;

namespace PlayModeTests
{
    public static class TestUtilities
    {
        public static GameObject LoadPrefab(string name) =>
            AssetDatabase.LoadAssetAtPath<GameObject>(
                AssetDatabase.GUIDToAssetPath(
                    AssetDatabase.FindAssets($"{name} t:GameObject")[0]));
    }
}
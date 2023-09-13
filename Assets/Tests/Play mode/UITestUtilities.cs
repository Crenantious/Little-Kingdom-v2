using LittleKingdom;
using UnityEditor;
using UnityEngine;

namespace PlayModeTests
{
    public static class UITestUtilities
    {
        private const string NoAssetsFoundError = "Unable to load asset of type {0} with name {1}.";

        public const string UIDocumentPath = "Graphics/UI/UI toolkit/";

        public static T GetAsset<T>(string name, params string[] paths) where T : Object
        {
            string[] guids = paths.Length == 0 ?
                             AssetDatabase.FindAssets(name) :
                             AssetDatabase.FindAssets(name, paths);

            if (guids.Length == 0)
            {
                Debug.LogError(NoAssetsFoundError.FormatConst(nameof(T), name));
                return null;
            }

            return AssetDatabase.LoadAssetAtPath<T>(
                AssetDatabase.GUIDToAssetPath(guids[0]));
        }
    }
}
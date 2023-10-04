using Assets.Scripts.Exceptions;
using UnityEditor;
using UnityEngine;

namespace LittleKingdom
{
    public static class AssetUtilities
    {
        public const string UIDocumentPath = "Graphics/UI/UI toolkit/";

        public static T GetAsset<T>(string name, params string[] paths) where T : Object
        {
            bool wasFound = TryGetAsset<T>(name, out T asset, paths);

            if (wasFound is false)
                throw new AssetFoundException(typeof(T), name);

            return asset;
        }

        public static bool TryGetAsset<T>(string name, out T asset, params string[] paths) where T : Object
        {
            string[] guids = paths.Length == 0 ?
                             AssetDatabase.FindAssets(name) :
                             AssetDatabase.FindAssets(name, paths);

            bool wasFound = guids.Length != 0;

            asset = wasFound ?
                    AssetDatabase.LoadAssetAtPath<T>(
                        AssetDatabase.GUIDToAssetPath(guids[0])) :
                    null;
            return wasFound;
        }
    }
}
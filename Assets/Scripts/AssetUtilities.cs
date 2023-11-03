using Assets.Scripts.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LittleKingdom
{
    public static class AssetUtilities
    {
        public const string UIDocumentPath = "Graphics/UI/UI toolkit/";

        public static T GetAsset<T>(string name, string selectionParameters = "", params string[] paths) where T : Object
        {
            bool wasFound = TryGetAsset<T>(name, out T asset, selectionParameters, paths);

            if (wasFound is false)
                throw new AssetFoundException(typeof(T), name);

            return asset;
        }

        public static bool TryGetAsset<T>(string name, out T asset, string selectionParameters = "", params string[] paths) where T : Object
        {
            string[] guids = paths.Length == 0 ?
                             AssetDatabase.FindAssets($"{name} {selectionParameters}") :
                             AssetDatabase.FindAssets($"{name} {selectionParameters}", paths);

            // AssetDatabase.FindAssets searches using substrings so we need to filter for the exact name.
            IEnumerable<string> assetPaths = guids.Select(g => AssetDatabase.GUIDToAssetPath(g))
                                                  .Where(p => Path.GetFileNameWithoutExtension(p) == name);
            bool wasFound = assetPaths.Count() != 0;

            asset = wasFound ?
                    AssetDatabase.LoadAssetAtPath<T>(assetPaths.ElementAt(0)) :
                    null;
            return wasFound;
        }

        /// <summary>
        /// Loads the first prefab with <paramref name="name"/> from assets. This is not an instance of the object.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static GameObject LoadPrefab(string name, params string[] paths) =>
            GetAsset<GameObject>(name, "t: prefab", paths);

        /// <summary>
        /// Tries to load the first prefab with <paramref name="name"/> from assets. This is not an instance of the object.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static bool TryLoadPrefab(string name, out GameObject asset, params string[] paths) =>
            TryGetAsset(name, out asset, "t: prefab", paths);
    }
}
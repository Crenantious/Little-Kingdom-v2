using UnityEditor;
using UnityEngine.UIElements;

namespace PlayModeTests
{
    public static class UITestUtilities
    {
        private static readonly string[] UIDocumentPath = new string[] { "Graphics/UI/UI toolkit/" };

        public static UIDocument GetUIDocument(string name) =>
            AssetDatabase.LoadAssetAtPath<UIDocument>(
                AssetDatabase.GUIDToAssetPath(
                    AssetDatabase.FindAssets(name, UIDocumentPath)[0]));
    }
}
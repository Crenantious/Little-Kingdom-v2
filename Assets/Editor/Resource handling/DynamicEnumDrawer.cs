using LittleKingdom.DataStructures;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LittleKingdom.Editor
{
    [CustomPropertyDrawer(typeof(DynamicEnum), true)]
    public class DynamicEnumDrawer : PropertyDrawer
    {
        private const string ValuesFolder = "Assets/Editor/Dynamic enum values";

        private bool isInitialised = false;
        private DynamicEnumValues values;

        private void Initialise(SerializedProperty property)
        {
            string valuesAssetName = property.type;

            if (TryGetValues(valuesAssetName) is false)
                CreateValues(valuesAssetName);

            isInitialised = true;
        }

        private bool TryGetValues(string assetName) =>
            EditorUtilities.TryGetAsset(assetName, out values, ValuesFolder);

        private void CreateValues(string assetName)
        {
            string path = $"{ValuesFolder}/{assetName}.asset";

            values = ScriptableObject.CreateInstance<DynamicEnumValues>();
            values.name = assetName;
            AssetDatabase.CreateAsset(values, path);
            AssetDatabase.SaveAssets();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (isInitialised is false)
                Initialise(property);

            SetIndex(property, EditorGUI.Popup(position, GetIndex(property), values.Values));
        }

        private static int GetIndex(SerializedProperty property) =>
            property.FindPropertyRelative("index").intValue;

        private static void SetIndex(SerializedProperty property, int index) =>
            property.FindPropertyRelative("index").intValue = index;
    }
}
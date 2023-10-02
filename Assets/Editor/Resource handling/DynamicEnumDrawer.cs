using LittleKingdom.DataStructures;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LittleKingdom.Editor
{
    [CustomPropertyDrawer(typeof(DynamicEnum), true)]
    public class DynamicEnumDrawer : PropertyDrawer
    {
        private const string ValuesFolder = "Assets/Editor/Dynamic enum values";
        private const string NoValuesFoundOption = "No values found";
        private const int EditButtonWidth = 25;

        private bool isInitialised = false;
        private DynamicEnumValues values;
        private readonly Sprite icon = EditorUtilities.GetAsset<Sprite>("Free Flat Pen Icon");

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

            position.width -= EditButtonWidth;
            CreateValuesPopup(position, property, label);

            position.x += position.width;
            position.width = EditButtonWidth;

            if (GUI.Button(position, icon.texture))
                EditEnumValues();
        }

        private void CreateValuesPopup(Rect position, SerializedProperty property, GUIContent label)
        {
            GUIContent[] options = GetPopupOptions(label).ToArray();
            int currentIndex = GetValueIndex(property);
            int newIndex = EditorGUI.Popup(position, label, currentIndex, options);
            SetIndex(property, newIndex);
        }

        private IEnumerable<GUIContent> GetPopupOptions(GUIContent label)
        {
            var options = from string value in values.Values
                          select CreatePopupOption(label, value);

            return options.Count() == 0 ?
                new GUIContent[] { CreatePopupOption(label, NoValuesFoundOption) } :
                options;
        }

        private static GUIContent CreatePopupOption(GUIContent label, string value) =>
            new(label) { text = value };

        private void EditEnumValues() =>
            EditDynamicEnumValuesEditorWindow.Show(values);

        private static int GetValueIndex(SerializedProperty property) =>
            property.FindPropertyRelative("index").intValue;

        private static void SetIndex(SerializedProperty property, int index) =>
            property.FindPropertyRelative("index").intValue = index;
    }
}
using LittleKingdom.DataStructures;
using System;
using UnityEditor;
using UnityEngine;

namespace LittleKingdom.Editor
{
    public class DynamicEnumDrawerCommon
    {
        private const string ValuesFolder = "Assets/Editor/Dynamic enum values";

        // TODO: JR - implement this.
        private const string NoValuesFoundOption = "No values found";
        private const int ValuesButtonWidth = 100;
        private const int EditButtonWidth = 25;

        private bool isInitialised = false;
        private readonly Sprite icon = AssetUtilities.GetAsset<Sprite>("Free Flat Pen Icon");

        public DynamicEnumValues ValuesAsset { get; private set; }

        private void Initialise(SerializedProperty property)
        {
            string enumName = property.FindPropertyRelative("enumName").stringValue;

            if (TryGetValuesAsset(enumName) is false)
                CreateValuesAsset(enumName);

            isInitialised = true;
        }

        private bool TryGetValuesAsset(string assetName)
        {
            var success = AssetUtilities.TryGetAsset(assetName, out DynamicEnumValues values, ValuesFolder);
            ValuesAsset = values;
            return success;
        }

        private void CreateValuesAsset(string assetName)
        {
            string path = $"{ValuesFolder}/{assetName}.asset";

            ValuesAsset = ScriptableObject.CreateInstance<DynamicEnumValues>();
            ValuesAsset.name = assetName;
            AssetDatabase.CreateAsset(ValuesAsset, path);
            AssetDatabase.SaveAssets();
        }

        public void OnGUI(Rect position, SerializedProperty property, GUIContent label,
            Func<int, bool> isItemSelected, Action<int> onItemSelected)
        {
            if (isInitialised is false)
                Initialise(property);

            position = EditorGUI.PrefixLabel(position, label);

            position.width = ValuesButtonWidth;
            DrawMenuSection(position, isItemSelected, onItemSelected);

            position.x += position.width;
            position.width = EditButtonWidth;

            if (GUI.Button(position, icon.texture))
                EditEnumValues();
        }

        private void DrawMenuSection(Rect position, Func<int, bool> isItemSelected, Action<int> onItemSelected)
        {
            if (ValuesAsset.Values.Count == 0)
                EditorGUI.LabelField(position, NoValuesFoundOption);
            else if (EditorGUI.DropdownButton(position, new GUIContent("Select values"), FocusType.Passive))
                CreateValuesMenu(position, isItemSelected, onItemSelected);
        }

        private void CreateValuesMenu(Rect position, Func<int, bool> isItemSelected, Action<int> onItemSelected)
        {
            GenericMenu menu = new();
            for (int i = 0; i < ValuesAsset.Values.Count; i++)
            {
                AddMenuItem(menu, i, isItemSelected, onItemSelected);
            }

            menu.DropDown(position);
        }

        private void AddMenuItem(GenericMenu menu, int i, Func<int, bool> isItemSelected, Action<int> onItemSelected) =>
            menu.AddItem(CreateMenuOption(ValuesAsset.Values[i]),
                         isItemSelected(i),
                         () => onItemSelected(i));

        private GUIContent CreateMenuOption(string value) =>
            new() { text = value };

        private void EditEnumValues() =>
            EditDynamicEnumValuesWindow.Show(ValuesAsset);
    }
}
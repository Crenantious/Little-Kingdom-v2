using LittleKingdom.DataStructures;
using UnityEditor;
using UnityEngine;
using static LittleKingdom.Editor.EditDynamicEnumValuesWindowPosition;

namespace LittleKingdom.Editor
{
    public class EditDynamicEnumValuesWindow : EditorWindow
    {
        private const string ValuesLoadedInformation = "Enum values must be distinct. Any duplicates will be removed on save.";
        private const string ValuesNotLoadedInformation = "Unable to load enum values. The asset may have been deleted.";
        private const string EmptyEnumNameInformation = "The given enum name is empty. This is an error.";

        private string informationLabelText = string.Empty;
        private string enumName = "";
        private string valuesAssetPath = "";
        private bool isEditing = false;
        private DynamicEnumValues valuesAsset;
        private SerializedObject serializedValues;

        public static void Show(DynamicEnumValues values)
        {
            string valuesAssetPath = AssetDatabase.GetAssetPath(values);

            if (TryShowExistingWindow(valuesAssetPath))
                return;
            ShowNewWindow(valuesAssetPath);
        }

        private static bool TryShowExistingWindow(string valuesAssetPath)
        {
            foreach (var window in UnityEngine.Resources.FindObjectsOfTypeAll<EditDynamicEnumValuesWindow>())
            {
                if (window.valuesAssetPath == valuesAssetPath)
                {
                    window.Show();
                    window.Focus();
                    return true;
                }
            }
            return false;
        }

        private static void ShowNewWindow(string valuesAssetPath)
        {
            // With rect
            var window = CreateInstance<EditDynamicEnumValuesWindow>();
            window.valuesAssetPath = valuesAssetPath;
            window.Show();
            window.Focus();
            window.Initialise();
        }

        private void OnEnable() =>
            Initialise();

        private void Initialise()
        {
            saveChangesMessage = "Save changes?";
            valuesAsset = AssetDatabase.LoadAssetAtPath<DynamicEnumValues>(valuesAssetPath);
            enumName = valuesAssetPath.Split("/")[^1];
            position = LoadPosition(enumName);

            SetTitle();
            SetInformationLabelText();
            InitialiseValuesAsset();
        }

        private void InitialiseValuesAsset()
        {
            if (valuesAsset)
            {
                serializedValues = new SerializedObject(valuesAsset);

                if (isEditing is false)
                {
                    valuesAsset.BeginEdit();
                    isEditing = true;
                }
            }
        }

        private void SetTitle() =>
            titleContent = new("Edit dynamic enum" + (string.IsNullOrEmpty(enumName) ? "" : $": {enumName}"));

        private void SetInformationLabelText() =>
            informationLabelText = string.IsNullOrEmpty(enumName) ? EmptyEnumNameInformation :
                (valuesAsset == false ? ValuesNotLoadedInformation : ValuesLoadedInformation);

        private void OnGUI()
        {
            EditorGUILayout.LabelField(informationLabelText);

            if (valuesAsset)
                DrawValuesAssetGUI();
        }

        private void DrawValuesAssetGUI()
        {
            serializedValues.Update();

            EditorGUILayout.PropertyField(serializedValues.FindProperty("editingValues"), new GUIContent("Values"));

            if (GUILayout.Button("Save"))
                SaveValues();

            serializedValues.ApplyModifiedProperties();
        }

        public override void SaveChanges()
        {
            SaveValues();
            base.SaveChanges();
        }

        public override void DiscardChanges()
        {
            DiscardValues();
            base.DiscardChanges();
        }

        private void SaveValues() =>
            valuesAsset.ApplyEdit();

        private void DiscardValues() =>
            valuesAsset.EndEdit();

        private void OnInspectorUpdate()
        {
            if (valuesAsset)
                hasUnsavedChanges = valuesAsset.HasUnsavedChanges();
        }

        private void OnDestroy()
        {
            SavePosition(position, enumName);

            if (valuesAsset)
                DiscardValues();
        }
    }
}
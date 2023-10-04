using LittleKingdom.DataStructures;
using UnityEditor;
using UnityEngine;

public class EditDynamicEnumValuesWindow : EditorWindow
{
    private bool isInitialised = false;
    //private Editor valuesEditor;
    private DynamicEnumValues values;
    private SerializedObject serializedValues;

    public static void Show(DynamicEnumValues values)
    {
        EditDynamicEnumValuesWindow window = GetWindow<EditDynamicEnumValuesWindow>();
        window.Initialise(values);
    }

    private void Initialise(DynamicEnumValues values)
    {
        if (isInitialised)
            return;

        this.values = values;
        serializedValues = new SerializedObject(values);
        //valuesEditor = Editor.CreateEditor(values);
        titleContent = new($"Edit {values.name} values");

        values.BeginEdit();

        isInitialised = true;
    }

    private void OnGUI()
    {
        serializedValues.Update();
        //valuesEditor.OnInspectorGUI();
        var v = serializedValues.FindProperty("values");
        EditorGUILayout.PropertyField(v);

        if (GUILayout.Button("Save"))
            values.ApplyEdit();

        serializedValues.ApplyModifiedProperties();
    }

    private void OnDestroy()
    {
        values.EndEdit();
    }
}
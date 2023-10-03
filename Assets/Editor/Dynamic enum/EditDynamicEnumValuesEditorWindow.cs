using LittleKingdom.DataStructures;
using UnityEditor;

public class EditDynamicEnumValuesEditorWindow : EditorWindow
{
    private Editor valuesEditor;

    public static void Show(DynamicEnumValues values)
    {
        EditDynamicEnumValuesEditorWindow window = GetWindow<EditDynamicEnumValuesEditorWindow>();
        window.valuesEditor = Editor.CreateEditor(values);
        window.titleContent = new($"Edit {values.name} values");
    }

    private void OnGUI()
    {
        valuesEditor.OnInspectorGUI();
    }
}
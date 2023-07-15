using UnityEngine;
using UnityEditor;

namespace LittleKingdom
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class ScriptableObjectDrawer : PropertyDrawer
    {
        private UnityEditor.Editor editor = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue)
            {
                DrawWithNotNullObject(position, property, label);
                return;
            }

            DrawWithNullObject(position, property, label);
        }

        private void DrawWithNullObject(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        private void DrawWithNotNullObject(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);

            EditorGUI.indentLevel++;

            EditorGUI.PropertyField(position, property, label, true);

            if (property.isExpanded)
            {
                if (!editor)
                    UnityEditor.Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);
                editor.OnInspectorGUI();
            }

            EditorGUI.indentLevel--;
        }
    }
}
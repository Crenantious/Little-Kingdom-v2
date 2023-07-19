using UnityEngine;
using UnityEditor;
using LittleKingdom.Attributes;

namespace LittleKingdom
{
    [CustomPropertyDrawer(typeof(DisplayDrawerAttribute), true)]
    public class DisplayDrawerAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue)
            {
                DrawWithNotNullObject(position, property, label);
                return;
            }

            DrawWithNullObject(position, property, label);
        }

        private void DrawWithNullObject(Rect position, SerializedProperty property, GUIContent label) =>
            EditorGUI.PropertyField(position, property, label, true);

        private void DrawWithNotNullObject(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(position, property, label, true);

            if (property.isExpanded)
                UnityEditor.Editor.CreateEditor(property.objectReferenceValue).OnInspectorGUI();

            EditorGUI.indentLevel--;
        }
    }
}
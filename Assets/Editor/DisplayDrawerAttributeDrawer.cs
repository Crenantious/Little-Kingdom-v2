using UnityEngine;
using UnityEditor;
using LittleKingdom.Attributes;
using System;

namespace LittleKingdom.Editor
{
    [CustomPropertyDrawer(typeof(DisplayDrawerAttribute), true)]
    public class DisplayDrawerAttributeDrawer : PropertyDrawer
    {
        public static event Action<UnityEngine.Object> OnPropertyObjectChange;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue)
            {
                DrawWithNotNullObject(position, property, label);
                return;
            }

            DrawWithNullObject(position, property, label);
        }

        private void DrawWithNotNullObject(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
            EditorGUI.indentLevel++;

            var previousObjectValue = property.objectReferenceValue;
            EditorGUI.PropertyField(position, property, label, true);
            if (previousObjectValue != property.objectReferenceValue)
                OnPropertyObjectChange?.Invoke(property.objectReferenceValue);

            // If the object is unassigned, the reference will be updated after creating the property field so the check must be done here.
            if (!property.objectReferenceValue)
                return;

            if (property.isExpanded)
                UnityEditor.Editor.CreateEditor(property.objectReferenceValue).OnInspectorGUI();

            EditorGUI.indentLevel--;
        }

        private void DrawWithNullObject(Rect position, SerializedProperty property, GUIContent label) =>
            EditorGUI.PropertyField(position, property, label, true);
    }
}
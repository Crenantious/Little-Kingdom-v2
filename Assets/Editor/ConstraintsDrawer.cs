using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using LittleKingdom.Attributes;
using System.Runtime.Serialization;

namespace Editor
{
    [CustomPropertyDrawer(typeof(AllowDerivedAttribute))]
    public class ConstraintsDrawer : PropertyDrawer
    {
        private static readonly string missingAttributeTooltip = $"Fields with the {nameof(AllowDerivedAttribute)} " +
                $"must also contain the {nameof(SerializeReference)}Attribute.";
        private static readonly GUIStyle attributeMissingStyle = new();
        private static readonly GUIContent typeNameContent = new("Set type");
        
        private string[] derivedTypesNames;
        private IEnumerable<Type> derivedTypes;
        private bool isInitialised = false;
        private bool isValid = true;
        private SerializedProperty property;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            isValid ?
            EditorGUI.GetPropertyHeight(property, label, true) :
            EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.property = property;
            if (isInitialised is false)
                Initialise();

            if (fieldInfo.GetCustomAttributes(typeof(SerializeReference), false).Length == 0)
            {
                isValid = false;
                DrawInvalidProperty(position, label);
                return;
            }

            DrawValidProperty(position,  label);
        }

        private void Initialise()
        {
            derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => fieldInfo.FieldType.IsAssignableFrom(t) &&
                            t.ContainsGenericParameters is false &&
                            t.IsInterface is false);

            derivedTypesNames = derivedTypes.Select(t => t.Name).ToArray();
            attributeMissingStyle.normal.textColor = Color.red;
            isInitialised = true;
        }

        private static void DrawInvalidProperty(Rect position, GUIContent label) =>
            EditorGUI.LabelField(position,
                new GUIContent(label.text, missingAttributeTooltip),
                attributeMissingStyle);

        private void DrawValidProperty(Rect position, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();

            if (EditorGUI.DropdownButton(
                GetButtonRect(position,  label, typeNameContent),
                typeNameContent,
                FocusType.Passive))
            {
                ShowMenu();
                Event.current.Use();
            }

            EditorGUI.EndChangeCheck();

            EditorGUI.PropertyField(position, property, label, true);
        }

        private Rect GetButtonRect(Rect position, GUIContent label, GUIContent typeNameContent)
        {
            float buttonWidth = 10f + GUI.skin.button.CalcSize(typeNameContent).x;
            float buttonHeight = EditorGUI.GetPropertyHeight(property, label, false);
            Rect buttonRect = new(position.x + position.width - buttonWidth, position.y, buttonWidth, buttonHeight);
            return buttonRect;
        }

        private void ShowMenu()
        {
            GenericMenu context = new();

            int selectedItemIndex = GetSelectedMenuItemIndex();

            for (int i = 0; i < derivedTypesNames.Length; ++i)
            {
                int index = i;
                context.AddItem(
                    new GUIContent(derivedTypesNames[i]),
                    selectedItemIndex == index,
                    () => SetObjectAsTypeInstance(index));
            }

            context.ShowAsContext();
        }

        private int GetSelectedMenuItemIndex()
        {
            if (property.managedReferenceValue == null)
                return -1;

            Type currentType = property.managedReferenceValue == null ?
                fieldInfo.FieldType :
                property.managedReferenceValue.GetType();

            for (int i = 0; i < derivedTypes.Count(); i++)
            {
                if (derivedTypes.ElementAt(i) == currentType)
                    return i;
            }

            return -1;
        }

        private void SetObjectAsTypeInstance(int index)
        {
            Type typeToChangeTo = derivedTypes.ElementAt(index);

            property.serializedObject.UpdateIfRequiredOrScript();

            //This means the user is has selected to set the type to the current type, so nothing is needed to be done.
            if (property.managedReferenceValue != null && property.managedReferenceValue.GetType() == typeToChangeTo)
                return;

            Undo.RegisterCompleteObjectUndo(property.serializedObject.targetObject, "Create instance of " + typeToChangeTo);

            property.managedReferenceValue = FormatterServices.GetUninitializedObject(typeToChangeTo);
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
using UnityEngine;
using UnityEditor;
using System;
using LittleKingdom.Attributes;
using System.Linq;
using LittleKingdom.Constraints;

namespace LittleKingdom.Editor
{
    [CustomPropertyDrawer(typeof(AllowDerivedAttribute))]
    public class AllowDerivedClassDrawer : PropertyDrawer
    {
        private static readonly string missingAttributeTooltip = $"Fields with the {nameof(AllowDerivedAttribute)} " +
                $"must also contain the {nameof(SerializeReference)}Attribute.";
        private static readonly GUIStyle attributeMissingStyle = new();
        private static readonly GUIContent typeNameContent = new("Set type");

        private bool isInitialised = false;
        private bool isValid = true;
        private SerializedProperty property;
        private Rect buttonRect;
        private Rect propertyRect;
        private Vector2 dropdownClickPosition;
        private Rect dropdownRect;
        Type fieldType;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            isValid ?
            EditorGUI.GetPropertyHeight(property, label, true) :
            EditorGUIUtility.singleLineHeight;

        private void Initialise(SerializedProperty property)
        {
            fieldType = fieldInfo.FieldType.IsArray ?
                fieldInfo.FieldType.GetElementType() :
                fieldInfo.FieldType;

            this.property = property;
            attributeMissingStyle.normal.textColor = Color.red;
            isInitialised = true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            propertyRect = position;
            if (isInitialised is false)
                Initialise(property);

            if (isValid is false || fieldInfo.GetCustomAttributes(typeof(SerializeReference), false).Length == 0)
            {
                isValid = false;
                DrawInvalidProperty(position, label);
                return;
            }

            DrawValidProperty(position, label);
        }

        private static void DrawInvalidProperty(Rect position, GUIContent label) =>
            EditorGUI.LabelField(position,
                new GUIContent(label.text, missingAttributeTooltip),
                attributeMissingStyle);

        private void DrawValidProperty(Rect position, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            if(EditorGUI.DropdownButton(GetButtonRect(position, label, typeNameContent), typeNameContent, FocusType.Passive))
            {
                ConfigureGenericsEditorWindow window = EditorWindow.CreateInstance<ConfigureGenericsEditorWindow>();
                window.Setup(fieldType, DerivedTypeSelected);
                window.Show();
            }
            EditorGUI.EndChangeCheck();

            EditorGUI.PropertyField(position, property, label, true);
        }

        private Rect GetButtonRect(Rect position, GUIContent label, GUIContent typeNameContent)
        {
            float x = position.x + GUI.skin.button.CalcSize(typeNameContent).x + 10f;
            Rect buttonRect = new(x, position.y, position.x + position.width - x, position.height);
            return buttonRect;
        }

        private void DerivedTypeSelected(Type selectedType)
        {
            property.serializedObject.UpdateIfRequiredOrScript();

            //This means the user is has selected to set the type to the current type, so nothing is needed to be done.
            if (property.managedReferenceValue != null && property.managedReferenceValue.GetType() == selectedType)
                return;

            Undo.RegisterCompleteObjectUndo(property.serializedObject.targetObject, "Create instance of " + selectedType);

            //property.managedReferenceValue = FormatterServices.GetUninitializedObject(selectedType);
            Debug.Log(property);
            Debug.Log(property.serializedObject.FindProperty("value"));
            Debug.Log(property.FindPropertyRelative("value4"));
            Debug.Log(property.FindPropertyRelative("value5"));
            Debug.Log(property.FindPropertyRelative("value6"));
            Debug.Log(property.FindPropertyRelative("value7"));
            Debug.Log(property.FindPropertyRelative("constraint"));
            //((DerivedTypeContainer)GetParent(property)).OnTypeConfigured();
            //Debug.Log(property.FindPropertyRelative("value").boxedValue);
            //property.FindPropertyRelative("value").boxedValue = FormatterServices.GetUninitializedObject(selectedType);
            property.serializedObject.ApplyModifiedProperties();
        }

        public object GetParent(SerializedProperty prop)
        {
            string path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            string[] elements = path.Split('.');
            foreach (string element in elements.Take(elements.Length - 1))
            {
                if (element.Contains("["))
                {
                    string elementName = element.Substring(0, element.IndexOf("["));
                    int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    //obj = GetValue(obj, elementName, index);
                }
                else
                {
                    //obj = GetValue(obj, element);
                }
            }
            return obj;
        }
        //private void DerivedTypeSelected(Popup<Type>.SelectedOptionInfo optionInfo)
        //{
        //    Type typeToChangeTo = optionInfo.Value;
        //    property.serializedObject.UpdateIfRequiredOrScript();

        //    //This means the user is has selected to set the type to the current type, so nothing is needed to be done.
        //    if (property.managedReferenceValue != null && property.managedReferenceValue.GetType() == typeToChangeTo)
        //        return;

        //    //TODO: JR - implement a feature that allows objects with generic parameters to be created.
        //    //Would likely have to be recursive as those generics could take generics.
        //    //Probably have a pop-up window appear to select the wanted type, then another if that type has a generic, and so on.

        //    if (typeToChangeTo.ContainsGenericParameters)
        //    {
        //        Debug.Log("Generic");
        //        ConfigureGenericsEditorWindow window = EditorWindow.CreateInstance<ConfigureGenericsEditorWindow>();
        //        window.Setup(typeToChangeTo, this);
        //        window.Show();
        //        //PopupWindow.Show(GetPopupRect(), new ChooseClassGenericsPopup(typeToChangeTo));
        //        //typeToChangeTo = typeToChangeTo.MakeGenericType(typeof(LittleKingdom.Events.GameStateChangedEvent));
        //    }
        //    else
        //    {
        //        Undo.RegisterCompleteObjectUndo(property.serializedObject.targetObject, "Create instance of " + typeToChangeTo);

        //        property.managedReferenceValue = FormatterServices.GetUninitializedObject(typeToChangeTo);
        //        property.serializedObject.ApplyModifiedProperties();
        //    }
        //}

        //public void SetupGenerics(IEnumerable<Type> genericTypes)
        //{

        //}
    }
}
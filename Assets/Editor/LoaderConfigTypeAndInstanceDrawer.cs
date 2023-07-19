using UnityEngine;
using UnityEditor;
using LittleKingdom.Loading;
using System;

namespace LittleKingdom
{
    [CustomPropertyDrawer(typeof(LoaderConfigTypeAndInstance), true)]
    public class LoaderConfigTypeAndInstanceDrawer : PropertyDrawer
    {
        private const string configTypeAssemblyQualifiedName = "LittleKingdom.Loading.{0}, Scripts";

        private bool isInitialised = false;
        private SerializedProperty configProperty;
        private SerializedProperty configTypeNameProperty;

        private void Initialise(SerializedProperty property)
        {
            configProperty = property.FindPropertyRelative("config");
            configTypeNameProperty = property.FindPropertyRelative("configTypeName");
            isInitialised = true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.boxedValue is null)
                return;

            if (isInitialised is false)
                Initialise(property);

            DrawAndUpdateConfig(position, property);
        }

        private void DrawAndUpdateConfig(Rect position, SerializedProperty property)
        {
            property.serializedObject.Update();
            configProperty.objectReferenceValue = DrawConfigField(position, property);
            property.serializedObject.ApplyModifiedProperties();
        }

        private LoaderConfig DrawConfigField(Rect position, SerializedProperty property) =>
            (LoaderConfig)EditorGUI.ObjectField(position, configProperty.objectReferenceValue, GetConfigType(), true);

        private Type GetConfigType() =>
            Type.GetType(
                configTypeAssemblyQualifiedName.FormatConst(
                    configTypeNameProperty.stringValue));
    }
}
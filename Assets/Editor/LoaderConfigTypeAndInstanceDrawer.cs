using System;
using UnityEngine;
using UnityEditor;
using LittleKingdom.Loading;

namespace LittleKingdom
{
    [CustomPropertyDrawer(typeof(LoaderConfigTypeAndInstance), true)]
    public class LoaderConfigTypeAndInstanceDrawer : PropertyDrawer
    {
        private SerializedProperty property;
        private SerializedProperty configProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.boxedValue is null)
                return;

            this.property = property;
            DrawAndUpdateConfig(position);
        }

        private void DrawAndUpdateConfig(Rect position)
        {
            UpdateProperties();
            configProperty.objectReferenceValue = DrawConfigField(position);
            SaveProperties();
        }

        private void UpdateProperties()
        {
            property.serializedObject.Update();
            configProperty = property.FindPropertyRelative("config");
        }

        private void SaveProperties() =>
            property.serializedObject.ApplyModifiedProperties();

        private LoaderConfig DrawConfigField(Rect position) =>
            (LoaderConfig)EditorGUI.ObjectField(position, configProperty.objectReferenceValue, GetConfigType(), true);

        private Type GetConfigType() =>
            ((LoaderConfigTypeAndInstance)property.boxedValue).ConfigType;
    }
}
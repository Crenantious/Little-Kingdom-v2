using LittleKingdom.DataStructures;
using UnityEditor;
using UnityEngine;

namespace LittleKingdom.Editor
{
    [CustomPropertyDrawer(typeof(DynamicEnum), true)]
    public class DynamicEnumDrawer : PropertyDrawer
    {
        private readonly DynamicEnumDrawerCommon commonDrawer = new();

        private SerializedProperty property;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.property = property;
            commonDrawer.OnGUI(position, property, label, IsValueSelected, OnValueSelected);
        }

        private bool IsValueSelected(int index) =>
            GetValue() == commonDrawer.ValuesAsset.Values[index];

        private void OnValueSelected(int index)
        {
            property.serializedObject.Update();
            SetValue(commonDrawer.ValuesAsset.Values[index]);
            property.serializedObject.ApplyModifiedProperties();
        }

        private string GetValue() =>
            property.FindPropertyRelative("value").stringValue;
    
        private void SetValue(string value) =>
            property.FindPropertyRelative("value").stringValue = value;
    }
}
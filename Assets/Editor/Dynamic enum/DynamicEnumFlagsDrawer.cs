using LittleKingdom.DataStructures;
using UnityEditor;
using UnityEngine;

namespace LittleKingdom.Editor
{
    [CustomPropertyDrawer(typeof(DynamicEnumFlags), true)]
    public class DynamicEnumFlagsDrawer : PropertyDrawer
    {
        private readonly DynamicEnumDrawerCommon commonDrawer = new();

        private SerializedProperty property;
        private SerializedProperty selectedValues;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.property = property;
            selectedValues = property.FindPropertyRelative("values");

            commonDrawer.OnGUI(position, property, label, IsValueSelected, OnValueSelected);
        }

        private bool IsValueSelected(int index)
        {
            string value = commonDrawer.EnumValues.Values[index];
            return  GetValueIndex(value) != -1;
        }

        private void OnValueSelected(int index)
        {
            property.serializedObject.Update();

            string value = commonDrawer.EnumValues.Values[index];
            int valueIndex = GetValueIndex(value);

            if (valueIndex == -1)
                AddSelectedValue(value);
            else
                RemoveSelectedValue(valueIndex);

            property.serializedObject.ApplyModifiedProperties();
        }

        private void AddSelectedValue(string value)
        {
            selectedValues.arraySize++;
            selectedValues.GetArrayElementAtIndex(selectedValues.arraySize - 1).stringValue = value;
        }

        private void RemoveSelectedValue(int index)
        {
            string lastElemet = selectedValues.GetArrayElementAtIndex(selectedValues.arraySize - 1).stringValue;
            selectedValues.GetArrayElementAtIndex(index).stringValue = lastElemet;
            selectedValues.arraySize--;
        }

        private int GetValueIndex(string value)
        {
            for (int i = 0; i < selectedValues.arraySize; i++)
            {
                if (selectedValues.GetArrayElementAtIndex(i).stringValue == value)
                    return i;
            }
            return -1;
        }
    }
}
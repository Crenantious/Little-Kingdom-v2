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
            selectedValues = property.FindPropertyRelative("valueIds");

            commonDrawer.OnGUI(position, property, label, IsValueSelected, OnValueSelected);
        }

        private bool IsValueSelected(int index)
        {
            string value = commonDrawer.EnumValues.Values[index];
            return GetValueIndex(value) != -1;
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
            selectedValues.GetArrayElementAtIndex(selectedValues.arraySize - 1).intValue = GetId(value);
        }
 
        private void RemoveSelectedValue(int index)
        {
            int lastElemet = selectedValues.GetArrayElementAtIndex(selectedValues.arraySize - 1).intValue;
            selectedValues.GetArrayElementAtIndex(index).intValue = lastElemet;
            selectedValues.arraySize--;
        }

        private int GetValueIndex(string value)
        {
            int id = GetId(value);
            Debug.Log(id);
            for (int i = 0; i < selectedValues.arraySize; i++)
            {
                Debug.Log(selectedValues.GetArrayElementAtIndex(i).intValue);
                if (selectedValues.GetArrayElementAtIndex(i).intValue == id)
                    return i;
            }
            return -1;
        }

        private int GetId(string value) =>
            commonDrawer.EnumValues.GetId(value);
    }
}
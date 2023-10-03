using LittleKingdom.Resources;
using System;
using UnityEditor;
using UnityEngine;

namespace LittleKingdom.Editor
{
    [CustomPropertyDrawer(typeof(GetResourceHoldersFromUnitTypes), true)]
    public class GetResourceHoldersFromUnitTypesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            throw new NotImplementedException();
        }
    }
}
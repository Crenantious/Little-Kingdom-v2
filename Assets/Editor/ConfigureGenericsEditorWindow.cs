using UnityEngine;
using UnityEditor;
using System;

namespace LittleKingdom.Editor
{
    public class ConfigureGenericsEditorWindow : EditorWindow
    {
        private Type baseType;
        private GenericTypeDisplay genericTypeDisplay;
        private Action<Type> typeConstructedCallback;

        public void Setup(Type baseType, Action<Type> typeConstructedCallback)
        {
            this.baseType = baseType;
            this.typeConstructedCallback = typeConstructedCallback;
            genericTypeDisplay = new(new TypeInfo(baseType));
        }

        public void OnGUI()
        {
            GUILayout.Label($"Create {baseType}", EditorStyles.boldLabel);
            GUILayout.Space(5);

            EditorGUI.BeginChangeCheck();
            genericTypeDisplay.Display();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Confirm"))
            {
                if (genericTypeDisplay.Validate())
                    typeConstructedCallback(genericTypeDisplay.GetConstructedGeneric());
                else
                    genericTypeDisplay.DisplayErrors();
            }
            else if (GUILayout.Button("Cancel"))
                Close();

            GUILayout.EndHorizontal();
            EditorGUI.EndChangeCheck();
        }
    }
}
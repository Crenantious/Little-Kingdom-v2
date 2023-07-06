using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LittleKingdom.Editor
{
    internal class GenericTypeHeaderDisplay
    {
        private readonly Popup<Type> popup;
        private readonly TypePopupInfo popupInfo;
        private readonly Color errorColour = Color.red;
        private readonly Action<Type> popupMenuCallback;

        private bool displayAsError = false;
        private bool ShouldDisplayFoldout = false;

        public bool IsFoldoutActive { get; private set; } = false;
        public Type SelectedGenericParameterType { get; private set; } = null;

        /// <summary>
        /// Gets the intersection of all types derived from each constraint.
        /// </summary>
        /// <param name="constraints">The parents that all valid types must derived from.</param>
        public GenericTypeHeaderDisplay(TypeInfo typeInfo, Action<Type> popupMenuCallback)
        {
            popupInfo = new(typeInfo);
            popup = new Popup<Type>(popupInfo.Options, GenericTypeSelected, popupInfo.TypeInfo.DerivedTypes.ToArray());
            this.popupMenuCallback = popupMenuCallback;
        }

        public void Display()
        {
            GUILayout.BeginHorizontal();
            DisplayHeader();
            DisplayPopup();
            GUILayout.EndHorizontal();
        }

        public void DisplayErrors() =>
            displayAsError = SelectedGenericParameterType == null;

        private void DisplayHeader()
        {

            if (ShouldDisplayFoldout)
                IsFoldoutActive = EditorGUILayout.Foldout(IsFoldoutActive, popupInfo.Name);
            else
            {
                Color textColour = GUI.contentColor;
                GUI.contentColor = displayAsError ? errorColour : textColour;
                EditorGUILayout.LabelField(popupInfo.Name);
                GUI.contentColor = textColour;
            }
        }

        private void DisplayPopup() =>
            popup.Display();

        private void GenericTypeSelected(Popup<Type>.SelectedOptionInfo info)
        {
            displayAsError = false;
            ShouldDisplayFoldout = info.Value.ContainsGenericParameters;
            SelectedGenericParameterType = info.Value;
            popupMenuCallback(info.Value);
        }
    }
}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//namespace LittleKingdom.Editor
//{
//    internal class GenericTypeHeaderDisplay
//    {
//        private readonly Popup<Type> popup;

//        private TypePopupInfo popupInfo;
//        private bool displayErrors = false;
//        private bool isGenericParameterSelected = false;
//        private List<GenericTypeDisplay> genericParameterDisplays = new();
//        private bool IsFoldoutActive = false;
//        private Color errorColour = Color.red;
//        private bool isTopLevel = false;
//        //public int SelectedIndex { get; } = -1;

//        /// <summary>
//        /// Gets the intersection of all types derived from each constraint.
//        /// </summary>
//        /// <param name="constraints">The parents that all valid types must derived from.</param>
//        public GenericTypeDisplay(TypeInfo typeInfo, bool isTopLevel = false)
//        {
//            popupInfo = new(typeInfo);
//            popup = new Popup<Type>(popupInfo.Options, GenericParameterTypeSelected, popupInfo.TypeInfo.DerivedTypes.ToArray());
//            this.isTopLevel = isTopLevel;
//            //errorStyle.normal.textColor = Color.red;
//        }

//        public void Display()
//        {
//            GUILayout.BeginHorizontal();
//            if (isTopLevel is false)
//            {
//                DisplayHeader();
//                DisplayPopup();
//            }
//            GUILayout.EndHorizontal();
//            if (IsFoldoutActive && genericParameterDisplays.Any())
//                DisplayGenericParameters(isTopLevel ? 0 : 1);
//        }

//        public bool Validate()
//        {
//            foreach (GenericTypeDisplay display in genericParameterDisplays)
//            {
//                if (display.Validate() is false)
//                    return false;
//            }
//            return isGenericParameterSelected;
//        }

//        public void DisplayErrors()
//        {
//            displayErrors = true;
//            ForEachGenericParameter(d => d.DisplayErrors());
//        }

//        public Type GetConstructedGeneric()
//        {
//            Type[] genericParameters = new Type[genericParameterDisplays.Count()];

//            for (int i = 0; i < genericParameterDisplays.Count; i++)
//            {
//                GenericTypeDisplay display = genericParameterDisplays.ElementAt(i);
//                genericParameters[i] = display.GetConstructedGeneric();
//            }

//            return popupInfo.TypeInfo.Type.MakeGenericType(genericParameters);
//        }

//        private void StopDisplayingErrors()
//        {
//            displayErrors = false;
//        }

//        private void DisplayHeader()
//        {

//            if (isGenericParameterSelected && genericParameterDisplays.Any())
//                IsFoldoutActive = EditorGUILayout.Foldout(IsFoldoutActive, popupInfo.Name);
//            else
//            {
//                Color textColour = GUI.contentColor;
//                GUI.contentColor = displayErrors ? errorColour : textColour;
//                EditorGUILayout.LabelField(popupInfo.Name);
//                GUI.contentColor = textColour;
//            }
//        }

//        private void DisplayPopup() =>
//            popup.Display();

//        private void DisplayGenericParameters(int indentChange)
//        {
//            EditorGUI.indentLevel += indentChange;
//            ForEachGenericParameter(d => d.Display());
//            EditorGUI.indentLevel -= indentChange;
//        }

//        private void CreateGenericParameterDisplays(Type selectedType)
//        {
//            genericParameterDisplays.Clear();
//            foreach (Type genericParameter in selectedType.GetGenericArguments())
//            {
//                genericParameterDisplays.Add(new (new TypeInfo(genericParameter)));
//            }
//        }

//        private void GenericParameterTypeSelected(Popup<Type>.SelectedOptionInfo info)
//        {
//            isGenericParameterSelected = true;
//            StopDisplayingErrors();
//            CreateGenericParameterDisplays(info.Value);
//        }

//        private void ForEachGenericParameter(Action<GenericTypeDisplay> action)
//        {
//            foreach(GenericTypeDisplay display in genericParameterDisplays)
//            {
//                action(display);
//            }
//        }
//    }
//}
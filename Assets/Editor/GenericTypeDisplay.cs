using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace LittleKingdom.Editor
{
    internal class GenericTypeDisplay
    {
        private readonly GenericTypeHeaderDisplay headerDisplay;
        private readonly List<GenericTypeDisplay> genericParameterDisplays = new();
        private readonly bool isTopLevel;
        private readonly int indentLevelChange;

        private Type generic;

        /// <summary>
        /// Gets the intersection of all types derived from each constraint.
        /// </summary>
        /// <param name="constraints">The parents that all valid types must derived from.</param>
        public GenericTypeDisplay(TypeInfo genericInfo, bool isTopLevel = false)
        {
            headerDisplay = new(genericInfo, CreateGenericParameterDisplays);
            generic = genericInfo.Type;
            this.isTopLevel = isTopLevel;
            indentLevelChange = isTopLevel ? 0 : 1;
            if (isTopLevel)
                CreateGenericParameterDisplays(genericInfo.Type);
        }

        public void Display()
        {
            if (isTopLevel is false)
                headerDisplay.Display();
            if (isTopLevel || headerDisplay.IsFoldoutActive)
                DisplayGenericParameters();
        }

        public bool Validate()
        {
            foreach (GenericTypeDisplay display in genericParameterDisplays)
            {
                if (display.Validate() is false)
                    return false;
            }
            return isTopLevel || headerDisplay.SelectedGenericParameterType != null;
        }

        public void DisplayErrors()
        {
            headerDisplay.DisplayErrors();
            ForEachGenericParameter(d => d.DisplayErrors());
        }

        public Type GetConstructedGeneric()
        {
            Type generic = headerDisplay.SelectedGenericParameterType;
            if (genericParameterDisplays.Any() is false)
                return generic;

            List<Type> genericParameters = new();

            foreach (GenericTypeDisplay display in genericParameterDisplays)
            {
                genericParameters.Add(display.GetConstructedGeneric());
            }

            return generic.MakeGenericType(genericParameters.ToArray());
        }

        private void DisplayGenericParameters()
        {
            EditorGUI.indentLevel += indentLevelChange;
            ForEachGenericParameter(d => d.Display());
            EditorGUI.indentLevel -= indentLevelChange;
        }

        private void CreateGenericParameterDisplays(Type selectedType)
        {
            genericParameterDisplays.Clear();
            foreach (Type genericParameter in selectedType.GetGenericArguments())
            {
                genericParameterDisplays.Add(new(new TypeInfo(genericParameter)));
            }
        }

        private void ForEachGenericParameter(Action<GenericTypeDisplay> action)
        {
            foreach (GenericTypeDisplay display in genericParameterDisplays)
            {
                action(display);
            }
        }
    }
}
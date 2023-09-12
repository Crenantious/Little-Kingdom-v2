using LittleKingdom.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;

namespace LittleKingdom.Editor
{
    [CustomEditor(typeof(LoaderTransitions))]
    public class LoaderTransitionsEditor : UnityEditor.Editor
    {
        private const string NoLoadersError = "No loaders found.";

        private readonly string defaultInitialLoader = nameof(GameSetupLoader);

        private List<string> loaderTypes;

        private void OnEnable()
        {
            Type baseType = typeof(ILoader);
            loaderTypes = Assembly.GetAssembly(baseType)
                .GetTypes()
                .Where(t =>
                    baseType.IsAssignableFrom(t) &&
                    t != baseType &&
                    t.IsAbstract is false &&
                    t.IsInterface is false &&
                    // t is created via Activator so it cannot inherit Object.
                    typeof(UnityEngine.Object).IsAssignableFrom(t) is false)
                .OrderBy(t => t.Name)
                .Select(t => t.Name)
                .ToList();
        }

        public override VisualElement CreateInspectorGUI()
        {
            EnsureInitialLoaderIsSet();
            VisualElement inspector = new();

            if (loaderTypes.Any())
                inspector.Add(CreateDropdown());
            else
                inspector.Add(CreateNoTypesLabel());

            return inspector;
        }

        private Label CreateNoTypesLabel() =>
            new(NoLoadersError);

        private void EnsureInitialLoaderIsSet()
        {
            SerializedProperty initialLoader = GetInitalLoaderProperty();
            if (string.IsNullOrEmpty(initialLoader.stringValue))
                SetInitialLoader(initialLoader.stringValue);
        }

        private DropdownField CreateDropdown()
        {
            DropdownField dropdown = new("Initial loader", loaderTypes, GetInitialDropdownSelection());
            dropdown.RegisterValueChangedCallback(OnSelection);
            return dropdown;
        }

        private string GetInitialDropdownSelection()
        {
            SerializedProperty initialLoader = GetInitalLoaderProperty();
            string loader = string.IsNullOrEmpty(initialLoader.stringValue) ?
                defaultInitialLoader : initialLoader.stringValue;

            return loaderTypes.Where(t => t == loader).Any() ?
                   loader : loaderTypes[0];
        }

        private void OnSelection(ChangeEvent<string> option) =>
            SetInitialLoader(option.newValue);

        private void SetInitialLoader(string typeName)
        {
            serializedObject.Update();
            GetInitalLoaderProperty().stringValue = typeName;
            serializedObject.ApplyModifiedProperties();
        }

        private SerializedProperty GetInitalLoaderProperty() =>
            serializedObject.FindProperty("initialLoaderTypeName");
    }
}
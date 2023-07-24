using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using LittleKingdom.Loading;

namespace LittleKingdom
{
    [CustomEditor(typeof(LoaderProfiles))]
    public class LoaderProfilesEditor : UnityEditor.Editor
    {
        private readonly string profileAssetPath = "Assets/Scripts/Loading/Profiles";

        private VisualElement inspector;
        private IEnumerable<Type> configTypes;

        [SerializeField] private VisualTreeAsset visualTree;

        private void OnEnable()
        {
            Type baseType = typeof(LoaderConfig);
            configTypes = Assembly.GetAssembly(baseType)
                .GetTypes()
                .Where(t => baseType.IsAssignableFrom(t) && t != baseType)
                .OrderBy(t => t.Name);

            UpdateProfileConfigs(GetProfileFromSerializedObject());
        }

        public override VisualElement CreateInspectorGUI()
        {
            inspector = new();
            visualTree.CloneTree(inspector);

            ConfigureCreateProfileButton();
            AddProfilePropertyField();

            return inspector;
        }

        private void ConfigureCreateProfileButton()
        {
            Button createProfileButton = inspector.Query<Button>("CreateProfile").First();
            createProfileButton.clicked += OnCreateProfileButton;
        }

        private void AddProfilePropertyField()
        {
            SerializedProperty currentProfileProperty = GetCurrentProfileProperty();
            var a = new PropertyField(currentProfileProperty);
            a.BindProperty(currentProfileProperty);
            a.RegisterValueChangeCallback(c => UpdateProfileConfigs(GetProfileFromProperty(c.changedProperty)));
            inspector.Add(a);
        }

        private void OnCreateProfileButton()
        {
            string name = AskForProfileName();
            if (string.IsNullOrEmpty(name))
                return;
            CreateProfile(name);
        }

        private string AskForProfileName() =>
            EditorUtility.SaveFilePanelInProject("Profile name", "Loader profile", "asset", "Enter a profile name", profileAssetPath);

        private void CreateProfile(string path)
        {
            LoaderProfile profile = CreateInstance<LoaderProfile>();
            UpdateProfileConfigs(profile);
            AssetDatabase.CreateAsset(profile, path);
            AssetDatabase.SaveAssets();
        }

        // TODO: JR - update all profiles on domain reload.
        private void UpdateProfileConfigs(LoaderProfile profile)
        {
            List<LoaderConfigTypeAndInstance> configs = new();

            foreach (Type configType in configTypes)
                configs.Add(new(configType, profile.GetConfig(configType)));

            profile.configs = configs;
        }

        private LoaderProfile GetProfileFromSerializedObject() =>
            GetProfileFromProperty(GetCurrentProfileProperty());

        private SerializedProperty GetCurrentProfileProperty() =>
            serializedObject.FindProperty("current");

        private LoaderProfile GetProfileFromProperty(SerializedProperty property) =>
            property.objectReferenceValue as LoaderProfile;
    }
}
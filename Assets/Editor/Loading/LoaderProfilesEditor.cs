using LittleKingdom.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom
{
    // TODO: JR - add a button to update all profile configs.
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

            DisplayDrawerAttributeDrawer.OnPropertyObjectChange += o => UpdateProfileConfigs();
            UpdateProfileConfigs();
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
            PropertyField profileField = new(currentProfileProperty);
            profileField.BindProperty(currentProfileProperty);
            inspector.Add(profileField);
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

        private void UpdateProfileConfigs()
        {
            LoaderProfile profile = GetProfileFromSerializedObject();
            if (profile)
                UpdateProfileConfigs(profile);
        }

        private void UpdateProfileConfigs(LoaderProfile profile)
        {
            List<LoaderConfigTypeAndInstance> configs = new();

            foreach (Type configType in configTypes)
                configs.Add(new(configType, profile.GetConfig(configType)));

            profile.SetConfigs(configs);
        }

        private LoaderProfile GetProfileFromSerializedObject() =>
            GetProfileFromProperty(GetCurrentProfileProperty());

        private SerializedProperty GetCurrentProfileProperty() =>
            serializedObject.FindProperty("Current");

        private LoaderProfile GetProfileFromProperty(SerializedProperty property) =>
            property.objectReferenceValue as LoaderProfile;
    }
}
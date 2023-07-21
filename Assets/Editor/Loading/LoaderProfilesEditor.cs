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
                .Where(t => baseType.IsAssignableFrom(t))
                .OrderBy(t => t.Name);
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

        private void AddProfilePropertyField() =>
            inspector.Add(new PropertyField(serializedObject.FindProperty("current")));

        private void OnCreateProfileButton()
        {
            string name = AskForProfileName();
            CreateProfile(name);
        }

        private string AskForProfileName() =>
            // name cannot not be null/empty.
            EditorUtility.SaveFilePanelInProject("Profile name", "Loader profile", "asset", "Enter a profile name", profileAssetPath);

        private void CreateProfile(string path)
        {
            LoaderProfile profile = CreateInstance<LoaderProfile>();
            UpdateConfigs(profile);
            AssetDatabase.CreateAsset(profile, path);
            AssetDatabase.SaveAssets();
        }

        // TODO: JR - update for all profiles on domain reload.
        private void UpdateConfigs(LoaderProfile profile)
        {
            List<LoaderConfigTypeAndInstance> configs = new();

            foreach (Type configType in configTypes)
            {
                configs.Add(new(configType, profile.GetConfig(configType)));
                profile.configs = configs;
            }
        }
    }
}
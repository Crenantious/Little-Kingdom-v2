using LittleKingdom.Loading;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LittleKingdom
{
    [CustomEditor(typeof(LoaderProfiles))]
    public class LoaderProfilesEditor : UnityEditor.Editor
    {
        private readonly string profileAssetPath = "Assets/Scripts/Loading/Profiles";

        private VisualElement inspector;

        [SerializeField] private VisualTreeAsset visualTree;

        public override VisualElement CreateInspectorGUI()
        {
            inspector = new();
            visualTree.CloneTree(inspector);

            ConfigureCreateProfileButton();

            if (serializedObject.FindProperty("current") != default)
            {
                AddProfilePropertyField();
            }

            return inspector;
        }

        private void ConfigureCreateProfileButton()
        {
            Button createProfileButton = inspector.Query<Button>("CreateProfile").First();
            createProfileButton.clicked += OnCreateProfileButton;
        }

        private void AddProfilePropertyField()
        {
            inspector.Add(new PropertyField(serializedObject.FindProperty("current")));
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
            AssetDatabase.CreateAsset(profile, path);
            AssetDatabase.SaveAssets();
        }
    }
}
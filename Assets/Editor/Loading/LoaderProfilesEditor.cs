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
        private readonly string configPath = "Assets/Scripts/Loading/Configs/{0}.asset";

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
            createProfileButton.clicked += CreateProfile;
        }

        private void AddProfilePropertyField()
        {
            inspector.Add(new PropertyField(serializedObject.FindProperty("current")));
        }

        private void CreateProfile()
        {
            LoaderProfile profile = CreateInstance<LoaderProfile>();
            profile.name = "placeholder";
            AssetDatabase.CreateAsset(profile, string.Format(configPath, profile.name));
            AssetDatabase.SaveAssets();
        }
    }
}
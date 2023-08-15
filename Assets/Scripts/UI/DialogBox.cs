using LittleKingdom.Input;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace LittleKingdom.UI
{
    // TODO: JR - refactor to use the UIContainer system.
    public class DialogBox : MonoBehaviour
    {
        [SerializeField] private UIDocument document;
        [SerializeField] private StyleSheet optionStyleSheet;
        [SerializeField] private GameObject UIElements;
        
        private UIInput uiInput;

        [Inject]
        public void Construct(UIInput uiInput) =>
            this.uiInput = uiInput;

        /// <summary>
        /// Open a box on the screen with a title and buttons.
        /// </summary>
        /// <param name="callback">Called when any option button is pressed. The name of the option is passed in.</param>
        /// <param name="options">The name of each option button, to be displayed in order: left to right.</param>
        public void Open(string title, Action<string> callback, params string[] options)
        {
            OpenCommon(title);

            foreach (string option in options)
            {
                CreateButton(option, callback);
            }
        }

        /// <summary>
        /// Open a box on the screen with a title and buttons.
        /// </summary>
        /// <param name="options">The name of each option button with a corresponding action to be called when said option is pressed,
        /// the name of the option is passed in.<br/>
        /// Displayed in order: left to right.</param>
        public void Open(string title, params (string name, Action<string> callback)[] options)
        {
            OpenCommon(title);

            foreach ((string name, Action<string> callback) option in options)
            {
                CreateButton(option.name, option.callback);
            }
        }

        private void OpenCommon(string message)
        {
            uiInput.SetActive();
            UIElements.SetActive(true);
            GetTitle().text = message;
            GetOptionsContainer().Clear();
        }

        private void CreateButton(string name, Action<string> callback)
        {
            Button button = new();
            button.styleSheets.Add(optionStyleSheet);
            button.text = name;
            button.clicked += () => ButtonCallback(callback, name);
            GetOptionsContainer().Add(button);
        }

        private void ButtonCallback(Action<string> callback, string option)
        {
            ActiveInputScheme.SetPrevious();
            UIElements.SetActive(false);
            callback?.Invoke(option);
        }

        private VisualElement GetOptionsContainer() =>
            document.rootVisualElement.Q("OptionsContainer");

        private Label GetTitle() =>
            document.rootVisualElement.Q("Title") as Label;
    }
}
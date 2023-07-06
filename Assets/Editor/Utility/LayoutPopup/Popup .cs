using System;
using UnityEditor;
using UnityEngine;

namespace LittleKingdom.Editor
{
    internal class Popup
    {

        protected readonly string Label = string.Empty;
        protected readonly string[] OptionNames;
        protected readonly Action<SelectedOptionInfo> Callback;

        protected int SelectedIndex { get; private set; } = -1;

        public float Width { get; private set; } = 20;

        public record SelectedOptionInfo(int Index, string Name);

        public Popup(string label, string[] optionNames, Action<SelectedOptionInfo> callback)
        {
            Label = label;
            OptionNames = optionNames;
            Callback = callback;
        }

        public Popup(string[] optionNames, Action<SelectedOptionInfo> callback)
        {
            OptionNames = optionNames;
            Callback = callback;
        }

        public void Display() =>
            CheckNewItemSelected(DisplayPopup());
            
        public void Display(Rect rect) =>
            CheckNewItemSelected(DisplayPopup(rect));

        public void Display(Vector2 position, float maxWidth = -1)
        {
            if (maxWidth >= 0)
                Width = Math.Max(Width, maxWidth);
            CheckNewItemSelected(DisplayPopup(new(position.x, position.y, Width, EditorGUIUtility.singleLineHeight)));
        }

        private void CheckNewItemSelected(int newlySelectedIndex)
        {
            if (newlySelectedIndex != SelectedIndex)
            {
                OnNewItemSelected(newlySelectedIndex);
            }
        }

        private int DisplayPopup() =>
            Label == string.Empty ?
                EditorGUILayout.Popup(SelectedIndex, OptionNames) :
                EditorGUILayout.Popup(Label, SelectedIndex, OptionNames);

        private int DisplayPopup(Rect position) =>
            Label == string.Empty ?
                EditorGUI.Popup(position, SelectedIndex, OptionNames) :
                EditorGUI.Popup(position, Label, SelectedIndex, OptionNames);

        protected void OnNewItemSelected(int index)
        {
            SelectedIndex = index;

            GUIContent labelContent = new(OptionNames[SelectedIndex]);
            Width = GUI.skin.button.CalcSize(labelContent).x;


            InvokeCallback();
        }

        protected virtual void InvokeCallback() =>
            Callback.Invoke(GetSelectedItemInfo());

        protected SelectedOptionInfo GetSelectedItemInfo() =>
            new(SelectedIndex, OptionNames[SelectedIndex]);
    }
}
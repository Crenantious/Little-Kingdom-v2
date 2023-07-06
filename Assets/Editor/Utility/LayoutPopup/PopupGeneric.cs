using System;

namespace LittleKingdom.Editor
{
    internal class Popup<OptionValue> : Popup
    {
        private readonly OptionValue[] optionValues;

        public new record SelectedOptionInfo(int Index, string Name, OptionValue Value) :
            Popup.SelectedOptionInfo(Index, Name);

        public Popup(string label, string[] options, Action<SelectedOptionInfo> callback, OptionValue[] optionValues) :
            base(label, options, (info) => callback.Invoke(GetOptionInfo(info, optionValues)))
        {
            this.optionValues = optionValues;
        }

        public Popup(string[] options, Action<SelectedOptionInfo> callback, OptionValue[] optionValues) :
            base(options, (info) => callback.Invoke(GetOptionInfo(info, optionValues)))
        {
            this.optionValues = optionValues;
        }

        private static SelectedOptionInfo GetOptionInfo(Popup.SelectedOptionInfo info, OptionValue[] optionValues) =>
            new(info.Index, info.Name, optionValues[info.Index]);
            //(SelectedOptionInfo)info with { Value = optionValues[info.Index] };

        //protected override void OnItemSelected() =>
        //    callback.Invoke((SelectedOptionInfo)GetSelectedItemInfo() with { Value = optionValues[selectedIndex] });
    }
}
using System;
using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    public interface IUIContainer
    {
        public void Show(VisualTreeAsset document, Action teardown);
        public void Hide();
    }

    public interface IUIContainter<TData>
    {
        public void Show(TData data);
        public void Hide();
    }
}
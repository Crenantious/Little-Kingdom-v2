using UnityEngine.UIElements;

namespace LittleKingdom.UI
{
    public interface IUIContainer
    {
        public void Show(VisualTreeAsset document);
        public void Hide();
    }

    public interface IUIContainter<TData> where TData : UIContainerData
    {
        public void Show(TData data);
        public void Hide();
    }
}
namespace LittleKingdom.UI
{
    public interface IInfoPanelDisplay<TInfo> where TInfo : DisplayInfo
    {
        public void Display(TInfo infoPanel);
    }
}
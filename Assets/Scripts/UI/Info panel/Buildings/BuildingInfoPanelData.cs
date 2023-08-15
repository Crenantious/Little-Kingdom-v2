using System;

namespace LittleKingdom.UI
{
    public record BuildingInfoPanelData(string Title, int BuildingLevel, string Description, Action UpgradeCallback) : UIContainerData();
}
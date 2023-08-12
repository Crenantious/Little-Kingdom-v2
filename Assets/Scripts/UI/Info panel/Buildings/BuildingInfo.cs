using System;

namespace LittleKingdom.UI
{
    public record BuildingInfo(string Title, int BuildingLevel, string Description, Action UpgradeCallback) : DisplayInfo(Title, Description);
}
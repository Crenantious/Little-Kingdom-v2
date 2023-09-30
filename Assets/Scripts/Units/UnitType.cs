using System;

namespace LittleKingdom.Units
{
    // TODO: JR - remove when deemed redundant. Likely an Editor will handle this as a user
    // can define the types thus no code will need to be changed.
    public enum UnitType
    {
        Worker,
        Guard,
        Thief
    }

    [Flags]
    public enum UnitTypeFlags
    {
        Worker = 1,
        Guard = 1 << 1,
        Thief = 1 << 2,
    }
}
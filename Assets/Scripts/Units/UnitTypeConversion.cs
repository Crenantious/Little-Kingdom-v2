using System.Collections.Generic;

namespace LittleKingdom.Units
{
    // TODO: JR - remove when deemed redundant. Likely an Editor will handle this as a user
    // can define the types thus no code will need to be changed.
    public class UnitTypeConversion
    {
        private readonly Dictionary<UnitType, UnitTypeFlags> UnitTypeToUnitTypeFlags = new()
        {
            { UnitType.Worker, UnitTypeFlags.Worker},
            { UnitType.Guard, UnitTypeFlags.Guard},
            { UnitType.Thief, UnitTypeFlags.Thief},
        };

        private readonly Dictionary<UnitTypeFlags, UnitType> UnitTypeFlagsToUnitType = new()
        {
            { UnitTypeFlags.Worker, UnitType.Worker},
            { UnitTypeFlags.Guard, UnitType.Guard},
            { UnitTypeFlags.Thief, UnitType.Thief},
        };

        public UnitTypeFlags ToFlags(UnitType unitType) =>
            UnitTypeToUnitTypeFlags[unitType];

        public UnitType FromFlags(UnitTypeFlags unitType) =>
            UnitTypeFlagsToUnitType[unitType];
    }
}
using System;

namespace LittleKingdom.DataStructures
{
    [Serializable]
    public class UnitType : DynamicEnum
    {
        public UnitType() : base("UnitType")
        {

        }
    }

    [Serializable]
    public class UnitTypeFlags : DynamicEnumFlags
    {
        public UnitTypeFlags() : base("UnitType")
        {

        }
    }
}
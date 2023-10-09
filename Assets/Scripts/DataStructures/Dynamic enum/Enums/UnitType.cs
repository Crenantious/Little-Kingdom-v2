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

    [Serializable]
    public class UnitTypea : DynamicEnum
    {
        public UnitTypea() : base("UnitTypea")
        {

        }
    }

    [Serializable]
    public class UnitTypeaFlags : DynamicEnumFlags
    {
        public UnitTypeaFlags() : base("UnitTypea")
        {

        }
    }
}
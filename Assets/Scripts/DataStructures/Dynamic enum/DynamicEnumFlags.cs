using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.DataStructures
{
    /// <inheritdoc cref="DynamicEnum"/>
    [Serializable]
    public abstract class DynamicEnumFlags
    {
        private const string EnumNameError = "Must set an enum name";

        // For use in a PropertyDrawer
        [SerializeField] private string enumName;

        /// <summary>
        /// This is the type of the associated <see cref="DynamicEnumValues"/>.
        /// </summary>
        [SerializeField] internal Type valuesType;

        /// <summary>
        /// Weather or not this class possesses the corresponding value in the associated <see cref="DynamicEnumValues"/>.
        /// </summary>
        [SerializeField] internal List<int> valueIds = new();

        public DynamicEnumFlags(string enumName)
        {
            this.enumName = enumName ?? throw new ArgumentNullException(nameof(enumName), EnumNameError);

#if UNITY_EDITOR
            DynamicEnumFlagsCleanup.Register(enumName, this);
#endif
        }

        public bool Equals(DynamicEnumFlags other)
        {
            DynamicEnumCommon.CheckInitialised(this);
            if (DynamicEnumCommon.IsComparisonValid(this, other) is false)
                return false;

            return valueIds == other.valueIds;
        }
    }
}
﻿using System;
using UnityEngine;

namespace LittleKingdom.DataStructures
{
    /// <summary>
    /// Represents an enum using string values controlled by a PropertyDrawer.<br/>
    /// Simply inherit this class to create a new enum type. No implementation is needed; the PropertyDrawer handles everything.
    /// </summary>
    [Serializable]
    public abstract class DynamicEnum
    {
        private const string EnumNameError = "Must set an enum name";

        // For use in a PropertyDrawer
        [SerializeField] private string enumName;

        // TODO: JR - register with a helper class that will fix the selected values when they enum values change.
        public DynamicEnum(string enumName) =>
            this.enumName = enumName ?? throw new ArgumentNullException(nameof(enumName), EnumNameError);

        /// <summary>
        /// This is the type of the associated <see cref="DynamicEnumValues"/>.
        /// </summary>
        [SerializeField] internal Type valuesType;

        /// <summary>
        /// The index of the value in the associated <see cref="DynamicEnumValues"/> this class possesses.
        /// </summary>
        [SerializeField] internal string value;

        public bool Equals(DynamicEnum other)
        {
            DynamicEnumCommon.CheckInitialised(this);
            if (DynamicEnumCommon.IsComparisonValid(this, other) is false)
                return false;

            return value == other.value;
        }
    }
}
using System;
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
        /// <summary>
        /// The index of the enum values list that's stored in the associated <see cref="DynamicEnumValues"/>.
        /// </summary>
        [SerializeField] private int index;
    }
}
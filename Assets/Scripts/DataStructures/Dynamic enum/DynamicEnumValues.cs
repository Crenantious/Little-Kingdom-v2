using UnityEngine;

namespace LittleKingdom.DataStructures
{
    public class DynamicEnumValues : ScriptableObject
    {
        // These are controlled by an PropertyDrawer.
        /// <summary>
        /// The values are strings for the enum to be dynamic.
        /// </summary>
        public string[] Values { get; set; } = new string[] { "No values found" };
    }
}
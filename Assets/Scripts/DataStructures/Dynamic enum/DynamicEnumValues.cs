using UnityEngine;

namespace LittleKingdom.DataStructures
{
    public class DynamicEnumValues : ScriptableObject
    {
        // These are controlled by an PropertyDrawer.
        /// <summary>
        /// The values are strings for the enum to be dynamic.
        /// </summary>
        [field: SerializeField] public string[] Values { get; set; } = new string[0];
    }
}
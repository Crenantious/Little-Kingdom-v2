using Codice.Client.Common.Threading;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.DataStructures
{
    public class DynamicEnumValues : ScriptableObject
    {
        private int currentId = 0;

        private readonly Dictionary<string, int> valueToId = new();
        private readonly HashSet<int> valueIds = new();

        // These are controlled by an PropertyDrawer.
        /// <summary>
        /// The values are strings for the enum to be dynamic.
        /// </summary>
        [SerializeField, HideInInspector] private List<string> values = new();

        public IReadOnlyList<string> Values { get; private set; }

        private List<string> oldValues = null;

        private void Awake() =>
            Values = values.AsReadOnly();

        public void BeginEdit()
        {
            oldValues = new(values);
        }

        public void ApplyEdit()
        {
            if (oldValues is null)
                throw new InvalidOperationException($"Must call {nameof(BeginEdit)} before {nameof(ApplyEdit)}.");

            List<string> newValues = new(values);
            List<string> removedValues = new(oldValues);
            values.Clear();

            foreach (string value in newValues)
            {
                if (IsValue(value))
                {
                    removedValues.Remove(value);
                    continue;
                }

                Add(value);
            }

            foreach (string value in removedValues)
            {
                Remove(value);
            }

            BeginEdit();
        }

        public void EndEdit()
        {
            values = oldValues;
            oldValues = null;
        }

        /// <returns>
        /// True: if the value was added<br/>
        /// False: if the value was not added because it already exists.
        /// </returns>
        public bool Add(string value)
        {
            if (valueToId.ContainsKey(value))
                return false;

            values.Add(value);
            valueIds.Add(currentId);
            valueToId.Add(value, currentId++);
            return true;
        }

        /// <returns>
        /// True: if the value was removed<br/>
        /// False: if the value was not removed because it does not exist.
        /// </returns>
        public bool Remove(string value)
        {
            if (valueToId.ContainsKey(value) is false)
                return false;

            int id = valueToId[value];
            values.Remove(value);
            valueIds.Remove(id);
            valueToId.Remove(value);
            return true;
        }

        /// <returns>The id if the value exists, -1 otherwise.</returns>
        public int GetId(string value)
        {
            if (valueToId.ContainsKey(value))
                return valueToId[value];
            return -1;
        }

        public bool IsValue(int id) =>
            valueIds.Contains(id);

        private bool IsValue(string value) =>
            valueToId.ContainsKey(value);
    }
}
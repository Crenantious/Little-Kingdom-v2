using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField, HideInInspector] private List<string> editingValues = null;

        public IReadOnlyList<string> Values => values.AsReadOnly();

        public void BeginEdit()
        {
            editingValues = new(values);
        }

        public void ApplyEdit()
        {
            ValidateBeganEditing(nameof(ApplyEdit));

            editingValues = editingValues.Distinct().ToList();
            string[] newValues = editingValues.Where(v => values.Contains(v) is false).ToArray();
            string[] removedValues = values.Where(v => editingValues.Contains(v) is false).ToArray();

            foreach (string value in newValues)
            {
                RegisterId(value);
            }

            foreach (string value in removedValues)
            {
                UnregisterId(value);
            }

            values = editingValues;
            BeginEdit();
        }

        public void EndEdit()
        {
            ValidateBeganEditing(nameof(EndEdit));
            editingValues = null;
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

        private void RegisterId(string value)
        {
            valueIds.Add(currentId);
            valueToId.Add(value, currentId++);
        }

        private void UnregisterId(string value)
        {
            valueIds.Remove(valueToId[value]);
            valueToId.Remove(value);
        }

        private bool IsValue(string value) =>
            valueToId.ContainsKey(value);

        private void ValidateBeganEditing(string methodName)
        {
            if (editingValues is null)
                throw new InvalidOperationException($"Must call {nameof(BeginEdit)} before {methodName}.");
        }
    }
}
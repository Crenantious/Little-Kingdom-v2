using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace LittleKingdom.Editor
{
    internal class TypePopupInfo
    {
        public TypeInfo TypeInfo { get; }

        public string Name { get; } = string.Empty;
        //public string[] DerivedTypesNames { get; }
        public string[] Options { get; }

        /// <summary>
        /// Gets the intersection of all types derived from each constraint.
        /// </summary>
        /// <param name="constraints">The parents that all valid types must derived from.</param>
        public TypePopupInfo(TypeInfo typeInfo)
        {
            TypeInfo = typeInfo;
            Name = GetUserReadableName(typeInfo.Type);
            //DerivedTypesNames = GetUserReadableNames(TypeInfo.DerivedTypes).ToArray();
            Options = GetUserReadableNames(typeInfo.DerivedTypes).ToArray();
        }

        protected static string GetUserReadableName(Type type)
        {
            string[] nameSplit = type.Name.Split("`");
            string name = type.Name.Split("`")[0];

            if (nameSplit.Count() > 1)
            {
                string genericParameters = string.Join(", ", type.GetGenericArguments().Select(t => t.ToString()));
                name += $"<{genericParameters}>";
            }

            return name;
        }

        private static List<string> GetUserReadableNames(IEnumerable<Type> types)
        {
            List<string> names = new();
            foreach (Type type in types)
            {
                names.Add(type.IsGenericType ?
                    GetUserReadableName(type) :
                    type.Name);
            }
            return names;
        }

        //private List<GenericTypeDisplay> CreateGenericParametersInfo(Type type)
        //{
        //    List<GenericTypeDisplay> genericParametersInfo = new();
        //    foreach (Type generic in type.GetGenericArguments())
        //    {
        //        genericParametersInfo.Add(new(generic));
        //    }
        //    return genericParametersInfo;
        //}
    }
}
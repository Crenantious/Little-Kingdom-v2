using System;
using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Editor
{
    internal class TypeInfo
    {
        public Type Type { get; }
        public IReadOnlyList<Type> DerivedTypes { get; }

        /// <summary>
        /// Gets the intersection of all types derived from each constraint.
        /// </summary>
        /// <param name="constraints">The parents that all valid types must derived from.</param>
        public TypeInfo(Type type, bool includeAbstract = false, bool includeInterfaces = false)
        {
            Type = type;
            Type[] constraints = type.IsGenericTypeParameter ?
                type.GetGenericParameterConstraints() :
                new Type[] { type };

            DerivedTypes = GetDerivedTypes(constraints.ToList(), includeAbstract, includeInterfaces).AsReadOnly();
        }

        private static List<Type> GetDerivedTypes(IEnumerable<Type> constraints, bool includeAbstract, bool includeInterfaces) =>
            AppDomain.CurrentDomain.GetAssemblies()
                 .SelectMany(a => a.GetTypes())
                 .Where(t => (includeAbstract || t.IsAbstract is false) &&
                             (includeInterfaces || t.IsInterface is false) &&
                             ValidateInheritance(t, constraints))
                 .ToList();

        private static List<string> GetUserReadableNames(IEnumerable<Type> types)
        {
            List<string> names = new();
            foreach (Type type in types)
            {
                names.Add(type.IsGenericType ?
                    GetUserReadableTypeName(type) : 
                    type.Name);
            }
            return names;
        }

        protected static string GetUserReadableTypeName(Type type)
        {
            string name = type.Name.Split("`")[0];
            string genericParameters = string.Join(", ", type.GetGenericArguments().Select(t => t.ToString()));
            name = $"{name}<{genericParameters}>";
            return name;
        }

        private static bool ValidateInheritance(Type type, IEnumerable<Type> constraints) =>
            constraints.Count(c => c.IsAssignableFrom(type)) == constraints.Count();
    }
}
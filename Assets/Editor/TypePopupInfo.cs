//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace LittleKingdom.Editor
//{
//    internal class TypePopupInfo
//    {
//        private TypeInfo typeInfo;

//        public string Name { get; } = string.Empty;
//        public IReadOnlyList<TypeInfo> GenericParameters { get; }
//        //public bool IsForGeneric { get; } = false;
//        public string[] DerivedTypesNames { get; }
//        public int SelectedIndex { get; set; } = -1;

//        /// <summary>
//        /// Gets the intersection of all types derived from each constraint.
//        /// </summary>
//        /// <param name="constraints">The parents that all valid types must derived from.</param>
//        public TypeMenuInfo(Type type)
//        {
//            typeInfo = new(type);
//            GenericParameters = CreateGenericParametersInfo(type);
//            Name = GetUserReadableName(type);
//            //Type = type;
//            //if (type != null)
//            //{
//            //    IsForGeneric = true;
//            //    UserReadableGenericName = GetUserReadableTypeName(type);
//            //}
//            DerivedTypesNames = GetUserReadableNames(typeInfo.DerivedTypes).ToArray();


//        }

//        private static List<string> GetUserReadableNames(IEnumerable<Type> types)
//        {
//            List<string> names = new();
//            foreach (Type type in types)
//            {
//                names.Add(type.IsGenericType ?
//                    GetUserReadableName(type) :
//                    type.Name);
//            }
//            return names;
//        }

//        protected static string GetUserReadableName(Type type)
//        {
//            string name = type.Name.Split("`")[0];
//            string genericParameters = string.Join(", ", type.GetGenericArguments().Select(t => t.ToString()));
//            name = $"{name}<{genericParameters}>";
//            return name;
//        }

//        private List<GenericTypeDisplay> CreateGenericParametersInfo(Type type)
//        {
//            List<GenericTypeDisplay> genericParametersInfo = new();
//            foreach (Type generic in type.GetGenericArguments())
//            {
//                genericParametersInfo.Add(new(generic));
//            }
//            return genericParametersInfo;
//        }
//    }
//}
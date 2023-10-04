using System.Collections.Generic;

namespace LittleKingdom.DataStructures
{
    // TODO: JR - add a class that loads every scene, thus every instance of DynamicEnumFlags,
    // outside of play mode, causing them to all be cleaned up since they call this class in their constructors.
    // Use IPreprocessBuildWithReport to do this.
    public static class DynamicEnumFlagsCleanup
    {
        private static readonly Dictionary<string, List<DynamicEnumFlags>> dynamicEnums = new();

        /// <summary>
        /// Set this <see langword="true"/> before beginning cleanup setup.<br/>
        /// While false, any calls to Register are ignored to increase performance.
        /// </summary>
        public static bool AllowRegistration { get; set; } = false;

        public static void Register(string enumName, DynamicEnumFlags dynamicEnum)
        {
            if (dynamicEnums.ContainsKey(enumName) is false)
                dynamicEnums.Add(enumName, new());
            //
            dynamicEnums[enumName].Add(dynamicEnum);
        }

        /// <summary>
        /// Ensures there are no references to deleted values.
        /// This only needs to be run once before a build. It should not an issue if it is not; a small excess of memory.
        /// </summary>
        public static void ValidateAll()
        {
            foreach (string enumName in dynamicEnums.Keys)
            {
                AssetUtilities.TryGetAsset(enumName, out DynamicEnumValues values);

                foreach (DynamicEnumFlags dynamicEnum in dynamicEnums[enumName])
                {
                    Validate(dynamicEnum, values);
                }
            }
        }

        private static void Validate(DynamicEnumFlags dynamicEnum, DynamicEnumValues values)
        {
            if (values == false)
            {
                dynamicEnum.valueIds.Clear();
                return;
            }

            RemoveMissingValueIds(dynamicEnum, values);
        }

        private static void RemoveMissingValueIds(DynamicEnumFlags dynamicEnum, DynamicEnumValues values)
        {
            List<int> toRemove = new();
            foreach (int value in dynamicEnum.valueIds)
            {
                if (values.IsValue(value) is false)
                    toRemove.Add(value);
            }

            dynamicEnum.valueIds.RemoveAll(i => toRemove.Contains(i));
        }
    }
}
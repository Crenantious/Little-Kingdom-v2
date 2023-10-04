using Assets.Scripts.Exceptions;
using System;
using UnityEngine;

namespace LittleKingdom.DataStructures
{
    public static class DynamicEnumCommon
    {
        private const string NotInitialisedError = "The dynamic enum has not been initialised." +
            "Please view it in the inspector and set a value before attempting to use it.";
        private const string ComparisonWarning = "{0} warning. Attempting to compare {1} with {2}; they will never be equal.";

        /// <exception cref="DynamicEnumInitialisationException"></exception>
        public static void CheckInitialised(DynamicEnum dynamicEnum)
        {
            if (dynamicEnum.valuesType is null || string.IsNullOrEmpty(dynamicEnum.value))
                throw new DynamicEnumInitialisationException(NotInitialisedError);
        }

        /// <exception cref="DynamicEnumInitialisationException"></exception>
        public static void CheckInitialised(DynamicEnumFlags dynamicEnum)
        {
            if (dynamicEnum.valuesType is null)
                throw new DynamicEnumInitialisationException(NotInitialisedError);
        }

        /// <returns>Weather or not the enums are for the same set of values.</returns>
        public static bool IsComparisonValid(DynamicEnum dynamicEnum1, DynamicEnum dynamicEnum2) =>
            IsComparisonValid(dynamicEnum1.GetType(), dynamicEnum2.GetType(), dynamicEnum1.valuesType, dynamicEnum2.valuesType);

        /// <inheritdoc cref="IsComparisonValid(DynamicEnum, DynamicEnum)"/>
        public static bool IsComparisonValid(DynamicEnumFlags dynamicEnum1, DynamicEnumFlags dynamicEnum2) =>
            IsComparisonValid(dynamicEnum1.GetType(), dynamicEnum2.GetType(), dynamicEnum1.valuesType, dynamicEnum2.valuesType);

        private static bool IsComparisonValid(Type dynamicEnum1, Type dynamicEnum2, Type valuesType1, Type valuesType2)
        {
            if (valuesType1 == valuesType2)
                return true;

            Debug.LogWarning(ComparisonWarning.FormatConst(
                nameof(DynamicEnum),
                dynamicEnum1.ToString(),
                dynamicEnum2.ToString()));
            return false;
        }
    }
}
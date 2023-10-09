using UnityEditor;
using UnityEngine;

namespace LittleKingdom.Editor
{
    internal static class EditDynamicEnumValuesWindowPosition
    {
        public static Rect LoadPosition(string enumName)
        {
            string positionString = EditorPrefs.GetString(GetPositionKey(enumName));
            if (string.IsNullOrEmpty(positionString))
                return GetDefaultPosition();

            string[] positionParts = positionString.Split(".");
            return new(float.Parse(positionParts[0]),
                       float.Parse(positionParts[1]),
                       float.Parse(positionParts[2]),
                       float.Parse(positionParts[3]));
        }

        public static void SavePosition(Rect position, string enumName)
        {
            string positionString = $"{position.x}.{position.y}.{position.width}.{position.height}";
            EditorPrefs.SetString(GetPositionKey(enumName), positionString);
        }

        private static Rect GetDefaultPosition() =>
            new(Screen.width / 2, Screen.height / 2, 300, 600);

        private static string GetPositionKey(string enumName) =>
            "Dynamic enum position " + enumName;
    }
}
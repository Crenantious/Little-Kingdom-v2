using LittleKingdom.DataStructures;
using UnityEngine;

namespace LittleKingdom.Extensions
{
    public static class GridUtility
    {
        public static TElement GetNearestElement<TElement>(this SizedGrid<TElement> grid,  Vector2 position)
        {
            Vector2 normalisedPosition = GetNormalisedPosition(grid, position);
            return GetElementFromPoint(grid, normalisedPosition);
        }

        private static Vector2 GetNormalisedPosition<TElement>(SizedGrid<TElement> grid, Vector2 position)
        {
            float x = position.x / grid.CellWidth;
            float y = position.y / grid.CellHeight;
            return new(x, y);
        }

        private static TElement GetElementFromPoint<TElement>(SizedGrid<TElement> grid, Vector2 position)
        {
            int tileColumn = (int)Mathf.Clamp(position.x, 0, grid.Width - 1);
            int tileRow = (int)Mathf.Clamp(position.y, 0, grid.Height - 1);

            return grid.Get(tileColumn, tileRow);
        }
    }
}
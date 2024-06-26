using LittleKingdom.DataStructures;
using UnityEngine;

namespace LittleKingdom.Extensions
{
    public static class GridUtility
    {
        /// <summary>
        /// Treats <paramref name="grid"/> as a physical grid with each element placed next to each other separated by their size.<br/>
        /// Gets the element that is closest to <paramref name="position"/>, this can be out of bound of the grid.
        /// </summary>
        public static TElement GetNearestElement<TElement>(this SizedGrid<TElement> grid, Vector2 position)
        {
            Vector2 normalisedPosition = GetNormalisedPosition(grid, position);
            (int column, int row) = GetNearestIndexFromPosition(grid, normalisedPosition);
            return grid.Get(column, row);
        }

        /// <summary>
        /// Treats <paramref name="grid"/> as a physical grid with each element placed next to each other separated by their size.<br/>
        /// Gets the index of the element that is closest to <paramref name="position"/>, this can be out of bound of the grid.
        /// </summary>
        public static (int column, int row) GetNearestIndex<TElement>(this SizedGrid<TElement> grid, Vector2 position)
        {
            Vector2 normalisedPosition = GetNormalisedPosition(grid, position);
            return GetNearestIndexFromPosition(grid, normalisedPosition);
        }

        private static Vector2 GetNormalisedPosition<TElement>(SizedGrid<TElement> grid, Vector2 position)
        {
            float x = position.x / grid.CellWidth;
            float y = position.y / grid.CellHeight;
            return new(x, y);
        }

        private static (int column, int row) GetNearestIndexFromPosition<TElement>(SizedGrid<TElement> grid, Vector2 position)
        {
            return ((int)Mathf.Clamp(position.x, 0, grid.Width - 1),
                    (int)Mathf.Clamp(position.y, 0, grid.Height - 1));
        }
    }
}
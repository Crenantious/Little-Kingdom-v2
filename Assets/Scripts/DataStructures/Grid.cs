using System;
using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.DataStructures
{
    public class Grid<TElement>
    {
        private readonly TElement[,] grid;
        private readonly Dictionary<TElement, (int column, int row)> elementPosition = new();

        public float Width { get; }
        public float Height { get; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            grid = new TElement[width, height];
        }

        /// <summary>
        /// Sets all elements based on <paramref name="callback"/>.
        /// </summary>
        /// <param name="callback">Params: column index, row index.<br/>Returns: the element to set at that position.</param>
        public void SetAll(Func<int, int, TElement> callback)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Set(i, j, callback(i, j));
                }
            }
        }

        public void Set(int column, int row, TElement element)
        {
            grid[column, row] = element;
            elementPosition[element] = (column, row);
        }

        /// <summary>
        /// Locates the first element matching <paramref name="element"/> and sets it to <see langword="default"/>.
        /// </summary>
        public void Clear(TElement element)
        {
            grid[elementPosition[element].column, elementPosition[element].row] = default;
            elementPosition.Remove(element);
        }

        /// <summary>
        /// Calls <see cref="Clear(TElement)"/> if <paramref name="element"/> exists.
        /// </summary>
        public void TryClear(TElement element)
        {
            if (elementPosition.ContainsKey(element))
                Clear(element);
        }

        /// <summary>
        /// Sets the element at the given position to <see langword="default"/>.
        /// </summary>
        public void ClearAt(int column, int row)
        {
            grid[column, row] = default;
        }

        public TElement Get(int column, int row) =>
            grid[column, row];

        public IEnumerable<TElement> GetEnumerable() =>
            grid.Cast<TElement>();
    }
}
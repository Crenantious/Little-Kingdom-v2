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

        public void Clear(TElement element)
        {
            grid[elementPosition[element].column, elementPosition[element].row] = default;
            elementPosition.Remove(element);
        }

        public void TryClear(TElement element)
        {
            if (elementPosition.ContainsKey(element))
                Clear(element);
        }

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
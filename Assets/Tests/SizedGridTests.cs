using NUnit.Framework;
using LittleKingdom.DataStructures;
using LittleKingdom.Extensions;

namespace SizedGridTests
{
    internal class SizedGridTests
    {
        private const int width = 4;
        private const int height = 4;
        private const int cellWidth = 2;
        private const int cellHeight = 2;

        private SizedGrid<int> grid;

        [SetUp]
        public void SetUp()
        {
            grid = new(width, height, cellWidth, cellHeight);
            grid.SetAll(GetElement);
        }

        [Test]
        public void SetAllWorks_Correctly()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Assert.AreEqual(grid.Get(i, j), GetElement(i, j));
                }
            }
        }

        [Test]

        #region TestCases

        // Sides (outside grid)
        [TestCase(3, -1, 1, 0)]
        [TestCase(9, 3, 3, 1)]
        [TestCase(3, 9, 1, 3)]
        [TestCase(-1, 3, 0, 1)]

        // Corners (outside grid)
        [TestCase(-1, -1, 0, 0)]
        [TestCase(9, -1, 3, 0)]
        [TestCase(9, 9, 3, 3)]
        [TestCase(-1, 9, 0, 3)]

        // Sides (inside grid)
        [TestCase(3, 0, 1, 0)]
        [TestCase(8, 3, 3, 1)]
        [TestCase(3, 8, 1, 3)]
        [TestCase(0, 3, 0, 1)]

        // Corners (inside grid)
        [TestCase(0, 0, 0, 0)]
        [TestCase(8, 0, 3, 0)]
        [TestCase(8, 8, 3, 3)]
        [TestCase(0, 8, 0, 3)]

        // non-border (inside grid)
        [TestCase(2.5f, 2.5f, 1, 1)] // bottom-left of the tile's centre
        [TestCase(3.5f, 2.5f, 1, 1)] // bottom-right of the tile's centre
        [TestCase(3.5f, 3.5f, 1, 1)] // top-right of the tile's centre
        [TestCase(2.5f, 3.5f, 1, 1)] // top-left of the tile's centre
        [TestCase(3f, 3f, 1, 1)] // centre of the tile
        [TestCase(4, 4, 2, 2)] // border of the tile

        #endregion

        public void GetNearestElement_WorksCorrectly(float x, float y, int expectedColumn, int expectedRow)
        {
            Assert.AreEqual(grid.GetNearestElement(new(x, y)), GetElement(expectedColumn, expectedRow));
        }

        private int GetElement(int column, int row) => column * width + row;
    }
}
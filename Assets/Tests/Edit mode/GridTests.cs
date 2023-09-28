using LittleKingdom.Board;
using LittleKingdom.DataStructures;
using LittleKingdom.Extensions;
using LittleKingdom.Factories;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace GridTests
{
    public class GridTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.Bind<BoardGenerator>().AsSingle();
            Container.Bind<ITile>().To<Tile>().AsSingle();
            Container.BindFactory<ITileInfo, ITile, TileFactory>().FromFactory<MockTileFactory>();
            Container.Inject(this);
        }

        [Test]
        // Out of bounds
        [TestCase(-1, -1, 0, 0)]
        [TestCase(11, -1, 4, 0)]
        [TestCase(11, 11, 4, 4)]
        [TestCase(-1, 11, 0, 4)]

        // First element boundaries
        [TestCase(0, 0, 0, 0)]
        [TestCase(2, 0, 1, 0)]
        [TestCase(2, 2, 1, 1)]
        [TestCase(0, 2, 0, 1)]

        // Grid boundaries
        [TestCase(0, 0, 0, 0)]
        [TestCase(10, 0, 4, 0)]
        [TestCase(10, 10, 4, 4)]
        [TestCase(0, 10, 0, 4)]

        // Inside an element
        [TestCase(3, 3.5f, 1, 1)]
        public void CreateSizedGrid_GetNearestIndex_WorksCorrectly(float xPosition, float yPosition, int expectedColumn, int expectedRow)
        {
            SizedGrid<int> grid = new(5, 5, 2, 2);

            (int column, int row) = grid.GetNearestIndex(new Vector2(xPosition, yPosition));

            Assert.AreEqual(expectedColumn, column);
            Assert.AreEqual(expectedRow, row);
        }
    }
}
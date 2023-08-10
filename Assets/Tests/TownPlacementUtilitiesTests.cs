using LittleKingdom.Board;
using LittleKingdom.DataStructures;
using LittleKingdom.Factories;
using LittleKingdom.Input;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using Zenject;

namespace LittleKingdom.Tests
{
    [TestFixture]
    internal class TownPlacementUtilitiesTests : ZenjectUnitTestFixture
    {
        [Inject] private BoardGenerator boardGenerator;
        [Inject] private TownPlacementUtilities townPlacementUtilities;
        [Inject] private TileEntityAssignment tileEntityAssignment;

        private Mock<ITown> town;
        private List<ITileInfo> tileInfos;
        private IBoard board;

        [SetUp]
        public void CommonInstall()
        {
            Container.Bind<BoardGenerator>().AsSingle();
            Container.Bind<InGameInput>().AsSingle();
            Container.Bind<InputUtility>().AsSingle();
            Container.Bind<Inputs>().AsSingle();
            Container.Bind<TileEntityAssignment>().AsSingle();
            Container.Bind<TownPlacementUtilities>().AsSingle();
            Container.BindFactory<ITileInfo, ITile, TileFactory>().FromFactory<CustomTileFactory>();
            Container.Inject(this);

            town = new();

            Mock<ITileInfo> tileInfo = new();
            tileInfo.SetupGet(t => t.ResourceType).Returns(ResourceType.Metal);
            tileInfo.SetupGet(t => t.PercentOfBoard).Returns(100);
            tileInfos = new() { tileInfo.Object };
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(3, 0)]
        [TestCase(3, 3)]
        [TestCase(0, 3)]
        public void MoveTownToTile(int column, int row)
        {
            var a = new Grid<ITile>(2, 2);
            town.SetupAllProperties();
            town.Setup(t => t.Tiles).Returns(a);
            town.Setup(t => t.Width).Returns(2);
            town.Setup(t => t.Height).Returns(2);
            board = boardGenerator.Generate(5, 5, tileInfos);
            HashSet<(int, int)> townTiles = new()
            {
                (column, row),
                (column + 1, row),
                (column, row + 1),
                (column + 1, row + 1)
            };

            ITown townObject = town.Object;
            tileEntityAssignment.AssignTown(townObject, board.Tiles.Get(column, row));

            Assert.AreEqual(townObject.OriginTile, board.Tiles.Get(column, row));
            Assert.AreEqual(townObject, townObject.OriginTile.Town);

            for (int i = 0; i < board.Tiles.Width; i++)
            {
                for (int j = 0; j < board.Tiles.Height; j++)
                {
                    if (townTiles.Contains((i, j)))
                        Assert.AreEqual(board.Tiles.Get(i, j).Town, townObject);
                    else
                        Assert.AreEqual(default, board.Tiles.Get(i, j).Town);
                }
            }
        }
    }
}
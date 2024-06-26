using LittleKingdom;
using LittleKingdom.Board;
using LittleKingdom.DataStructures;
using LittleKingdom.Factories;
using LittleKingdom.Input;
using LittleKingdom.Resources;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Resources = LittleKingdom.Resources.Resources;

namespace TownTests
{
    [TestFixture]
    internal class TownPlacementTests : ZenjectUnitTestFixture
    {
        private const float TileWidth = 3;
        private const float TileHeight = 3;

        [Inject] readonly private BoardGenerator boardGenerator;
        [Inject] readonly private TownPlacementUtilities townPlacementUtilities;
        [Inject] readonly private TileEntityAssignment tileEntityAssignment;

        private Mock<ITown> town;
        private ITown townObject;
        private List<ITileInfo> tileInfos;
        private IBoard board;

        [SetUp]
        public void CommonInstall()
        {
            Mock<IReferences> references = new();
            references.SetupAllProperties();
            references.Setup(r => r.TileWidth).Returns(TileWidth);
            references.Setup(r => r.TileHeight).Returns(TileHeight);

            Container.Bind<Inputs>().AsSingle();
            Container.Bind<StandardInput>().AsSingle();
            Container.Bind<ObjectClickthrough>().AsSingle();
            Container.Bind<BoardGenerator>().AsSingle();
            Container.Bind<TileEntityAssignment>().AsSingle();
            Container.Bind<TownPlacementUtilities>().AsSingle();
            Container.Bind<RaycastFromPointer>().AsSingle();
            Container.BindInstance(references.Object).AsSingle();
            Container.BindFactory<ITileInfo, ITile, TileFactory>().FromFactory<MockTileFactory>();
            Container.Inject(this);

            town = new();
            town.SetupAllProperties();
            town.Setup(t => t.Tiles).Returns(new Grid<ITile>(2, 2));

            town.Setup(t => t.SetPosition(It.IsAny<Vector2>())).Callback((Vector2 p) => SetTownPosition(p));
            townObject = town.Object;

            Mock<ITileInfo> tileInfo = new();
            tileInfo.SetupGet(t => t.Resources).Returns(new Resources(ResourceType.Metal, 1));
            tileInfo.SetupGet(t => t.PercentOfBoard).Returns(100);
            tileInfos = new() { tileInfo.Object };

            board = boardGenerator.Generate(5, 5, tileInfos);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(3, 1)]
        [TestCase(3, 4)]
        [TestCase(0, 4)]
        public void TownTilesAssignment_CorrectAssignmentsOnly(int column, int row)
        {
            town.Setup(t => t.Width).Returns(2);
            town.Setup(t => t.Height).Returns(2);
            HashSet<(int, int)> townTiles = new()
            {
                (column, row),
                (column + 1, row),
                (column, row - 1),
                (column + 1, row - 1)
            };

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

        [Test]
        #region TestCases
        // Each section tests the following in order: x failure, y failure, x and y failure.
        // Bottom left
        [TestCase(-1, 1)]
        [TestCase(0, 0)]
        [TestCase(-1, 0)]

        // Bottom right
        [TestCase(4, 1)]
        [TestCase(3, 0)]
        [TestCase(4, 0)]

        // Top right
        [TestCase(4, 4)]
        [TestCase(3, 5)]
        [TestCase(4, 5)]

        // Top left
        [TestCase(-1, 4)]
        [TestCase(0, 5)]
        [TestCase(-1, 5)]
        #endregion
        public void TownTilesAssignment_ThrowsIndexOutOfRangeException(int column, int row)
        {
            town.Setup(t => t.Width).Returns(2);
            town.Setup(t => t.Height).Returns(2);

            Assert.Throws<IndexOutOfRangeException>(() => tileEntityAssignment.AssignTown(townObject, board.Tiles.Get(column, row)));
        }

        [Test]
        [TestCase(0, 0, 1, 1, 0, 0)]
        [TestCase(0, 0, 2, 3, 1.5f, -3)]
        [TestCase(4, 0, 2, 3, 1.5f, -3)]
        [TestCase(4, 4, 4, 4, 4.5f, -4.5f)]
        [TestCase(0, 4, 1, 0, 0, 1.5f)]
        public void TownPositioning(int selectedTileColumn, int selectedTileRow, int townWidth, int townHeight,
            float expectedXPositionOffset, float expectedYPositionOffset)
        {
            town.Setup(t => t.Width).Returns(townWidth);
            town.Setup(t => t.Height).Returns(townHeight);
            ITile originTile = board.Tiles.Get(selectedTileColumn, selectedTileRow);

            townPlacementUtilities.MoveTownToTile(townObject, originTile);

            Assert.AreEqual(expectedXPositionOffset, townObject.XPosition - originTile.XPosition);
            Assert.AreEqual(expectedYPositionOffset, townObject.YPosition - originTile.YPosition);
        }

        private void SetTownPosition(Vector2 position)
        {
            town.Setup(t => t.XPosition).Returns(position.x);
            town.Setup(t => t.YPosition).Returns(position.y);
        }
    }
}
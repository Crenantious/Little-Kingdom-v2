using Assets.Scripts.Exceptions;
using LittleKingdom;
using LittleKingdom.Board;
using LittleKingdom.Extensions;
using LittleKingdom.Factories;
using LittleKingdom.Resources;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class BoardTests : ZenjectUnitTestFixture
{
    [Inject] private readonly BoardGenerator boardGenerator;
    private readonly List<TileInfo> tileInfos = new();

    private Mock<IReferences> references;
    private IEnumerable<ResourceType> resourceTypes;
    private IBoard board;

    [SetUp]
    public void CommonInstall()
    {
        references = new();

        Container.Bind<BoardGenerator>().AsSingle();
        Container.Bind<ITile>().To<TileMono>().AsSingle();
        Container.BindInstance(references.Object).AsSingle();
        Container.BindFactory<ITileInfo, ITile, TileFactory>().FromFactory<CustomTileFactory>();
        Container.Inject(this);

        resourceTypes = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>();
    }

    [Test]
    //The fractional component of tile amounts are less than 0.5.
    [TestCase(2, 5, 33, 33, 34)]
    //The fractional component of tile amounts are greater than 0.5.
    [TestCase(2, 5, 17, 17, 17, 17, 16, 16)]
    //The fractional component of tile amounts have values that are less than, and values that are greater than 0.5.
    [TestCase(2, 5, 22, 26, 27, 25)]
    public void CreateBoardWithGivenTileValues_IsCreatedWithCorrectTiles(int width, int height, params float[] percentages)
    {
        CreateBoard(width, height, percentages);

        AssertTilePercentages();
    }

    [Test]
    [TestCase(101)] //> 100% with one resource.
    [TestCase(30, 80)] //> 100% with two resources.
    [TestCase(99)] //< 100% with one resource.
    [TestCase(10, 20)] //< 100% with two resources.
    public void CreateBoardWithout100PercentTileCoverage_GetException(params float[] percentages)
    {
        try
        {
            CreateBoard(10, 10, percentages);
        }
        catch (Exception e)
        {
            Assert.AreSame(typeof(InvalidAmountOfTilesException), e.GetType());
        }
    }

    [Test]
    public void CreateBoard_TilesArePositionedCorrectly()
    {
        float tileWidth = 1.5f;
        float tileHeight = 1.5f;
        int BoardWidth = 3;
        int BoardHeight = 3;

        references.Setup(r => r.TileWidth).Returns(tileWidth);
        references.Setup(r => r.TileHeight).Returns(tileHeight);

        CreateBoard(BoardWidth, BoardHeight, 100);

        for (int i = 0; i < BoardWidth; i++)
        {
            for (int j = BoardHeight; j < 3; j++)
            {
                Assert.AreEqual(i * tileWidth + tileWidth / 2, board.Tiles.Get(i, j).XPosition);
                Assert.AreEqual(i * tileHeight + tileHeight / 2, board.Tiles.Get(i, j).YPosition);
            }
        }
    }

    [Test]
    #region TestCases
    // Bottom left corner
    [TestCase(0, 0, 1, 1, 0, 0)]
    [TestCase(0, 0, 1, 2, 0, 1)]
    [TestCase(0, 0, 1, 3, 0, 2)]
    [TestCase(0, 0, 2, 1, 0, 0)]
    [TestCase(0, 0, 3, 1, 0, 0)]

    // Bottom right corner
    [TestCase(5, 0, 1, 1, 4, 0)]
    [TestCase(5, 0, 1, 2, 4, 1)]
    [TestCase(5, 0, 1, 3, 4, 2)]
    [TestCase(5, 0, 2, 1, 3, 0)]
    [TestCase(5, 0, 3, 1, 2, 0)]

    // Top right corner
    [TestCase(5, 5, 1, 1, 4, 4)]
    [TestCase(5, 5, 1, 2, 4, 4)]
    [TestCase(5, 5, 1, 3, 4, 4)]
    [TestCase(5, 5, 2, 1, 3, 4)]
    [TestCase(5, 5, 3, 1, 2, 4)]

    // Top left corner
    [TestCase(0, 5, 1, 1, 0, 4)]
    [TestCase(0, 5, 1, 2, 0, 4)]
    [TestCase(0, 5, 1, 3, 0, 4)]
    [TestCase(0, 5, 2, 1, 0, 4)]
    [TestCase(0, 5, 3, 1, 0, 4)]
    #endregion
    public void CreateBoard_GetNearestTileToPointer_GotCorrectTile(float xPosition, float yPosition, int townWidth, int townHeight,
        int expectedTileColumn, int expectedTileRow)
    {
        references.Setup(r => r.TileWidth).Returns(1);
        references.Setup(r => r.TileHeight).Returns(1);
        Mock<ITown> town = new();
        town.Setup(t => t.Width).Returns(townWidth);
        town.Setup(t => t.Height).Returns(townHeight);

        CreateBoard(5, 5, 100);

        Assert.AreEqual(
            board.Tiles.Get(expectedTileColumn, expectedTileRow),
            board.GetTownOriginFromPointerPosition(town.Object, new(xPosition, yPosition)));
    }

    private void CreateBoard(int width, int height, params float[] percentagesOnBoard)
    {
        List<TileInfo> tileInfos = new();

        for (int i = 0; i < percentagesOnBoard.Length; i++)
        {
            tileInfos.Add(new TileInfo(null, resourceTypes.ElementAt(i), percentagesOnBoard[i]));
        }

        board = boardGenerator.Generate(width, height, tileInfos);
    }

    private void AssertTilePercentages()
    {
        foreach (TileInfo tileInfo in tileInfos)
        {
            float expectedTileAmount = board.Tiles.Width * board.Tiles.Height * tileInfo.PercentOfBoard / 100;
            int actualTileAmount = board.Tiles.GetEnumerable().Count(t => t.ResourceType == tileInfo.ResourceType);

            Assert.IsTrue(Mathf.FloorToInt(expectedTileAmount) == actualTileAmount ||
                          Mathf.CeilToInt(expectedTileAmount) == actualTileAmount);
        }
    }
}
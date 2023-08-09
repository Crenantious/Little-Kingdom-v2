using System.Collections.Generic;
using LittleKingdom.Board;
using NUnit.Framework;
using UnityEngine;
using System.Linq;
using System;
using Assets.Scripts.Exceptions;
using Zenject;
using LittleKingdom.Factories;

public class BoardTests : ZenjectUnitTestFixture
{
    [Inject] private readonly BoardGeneration boardGeneration;
    private readonly List<TileInfo> tileInfos = new();

    private IEnumerable<ResourceType> resourceTypes;
    private IBoard board;

    [SetUp]
    public void CommonInstall()
    {
        Container.BindFactory<TileInfo, Tile, TileFactory>().FromFactory<CustomTileFactory>();
        Container.Bind<BoardGeneration>().AsSingle();
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

    private void CreateBoard(int width, int height, params float[] percentagesOnBoard)
    {
        List<TileInfo> tileInfos = new();

        for (int i = 0; i < percentagesOnBoard.Length; i++)
        {
            tileInfos.Add(new TileInfo(null, resourceTypes.ElementAt(i), percentagesOnBoard[i]));
        }

        board = boardGeneration.Generate(width, height, tileInfos);
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
using Assets.Scripts.Exceptions;
using LittleKingdom.DataStructures;
using LittleKingdom.Factories;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleKingdom.Board
{
    public class BoardGenerator : IBoardGenerator
    {
        private readonly Dictionary<ITileInfo, int> remainingResourceTiles = new();
        private readonly TileFactory tileFactory;
        private readonly IReferences references;

        private SizedGrid<ITile> tiles;
        private float carryOverTiles = 0;
        private int totalTiles;
        private int remainingResourceTilesCount;

        public BoardGenerator(TileFactory tileFactory, IReferences references)
        {
            this.tileFactory = tileFactory;
            this.references = references;
        }

        public IBoard Generate(int widthInTiles, int heightInTiles, IEnumerable<ITileInfo> tileInfos)
        {
            totalTiles = widthInTiles * heightInTiles;
            tiles = new(widthInTiles, heightInTiles, references.TileWidth, references.TileHeight);

            InitialiseRemainingResources(tileInfos);
            CreateTiles();

            return references.Board = new Board(tiles);
        }

        private void InitialiseRemainingResources(IEnumerable<ITileInfo> tileInfos)
        {
            float totalPercent = 0;
            foreach (ITileInfo tileInfo in tileInfos)
            {
                totalPercent += tileInfo.PercentOfBoard;
                remainingResourceTiles[tileInfo] = GetTileAmount(tileInfo.PercentOfBoard);
            }

            remainingResourceTilesCount = remainingResourceTiles.Count;

            if (totalPercent != 100)
            {
                throw new InvalidAmountOfTilesException($"Was given a total tile coverage of {totalPercent}%, it must be exactly 100%.");
            }
        }

        private void CreateTiles()
        {
            for (int i = 0; i < tiles.Width; i++)
            {
                for (int j = 0; j < tiles.Height; j++)
                {
                    ITile tile = tileFactory.Create(GetRandomResource());
                    tile.Column = i;
                    tile.Row = j;
                    tiles.Set(i, j, tile);
                    tiles.Get(i, j).SetPos(new(references.TileWidth * i, references.TileHeight * j));
                }
            }
        }

        private ITileInfo GetRandomResource()
        {
            int index = Random.Range(0, remainingResourceTilesCount);
            ITileInfo resourceInfo = remainingResourceTiles.ElementAt(index).Key;

            remainingResourceTiles[resourceInfo]--;
            if (remainingResourceTiles[resourceInfo] == 0)
            {
                remainingResourceTiles.Remove(resourceInfo);
                remainingResourceTilesCount--;
            }

            return resourceInfo;
        }

        private int GetTileAmount(float percentOfBoard)
        {
            float actualAmount = totalTiles * percentOfBoard / 100 + carryOverTiles;
            int flooredAmount = (int)actualAmount;
            carryOverTiles = actualAmount - flooredAmount;
            return flooredAmount;
        }
    }
}
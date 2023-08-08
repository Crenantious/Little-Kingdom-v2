using Assets.Scripts.Exceptions;
using LittleKingdom.DataStructures;
using LittleKingdom.Factories;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleKingdom.Board
{
    public class BoardGeneration
    {
        private readonly Dictionary<TileInfo, int> remainingResourceTiles = new();
        private readonly TileFactory tileFactory;

        private SizedGrid<Tile> tiles;
        private float carryOverTiles = 0;
        private int totalTiles;
        private int remainingResourceTilesCount;

        public BoardGeneration(TileFactory tileFactory) =>
           this.tileFactory = tileFactory;

        public IBoard Generate(int widthInTiles, int heightInTiles, IEnumerable<TileInfo> tileInfos)
        {
            totalTiles = widthInTiles * heightInTiles;
            tiles = new(widthInTiles, heightInTiles, Tile.Width, Tile.Height);

            InitialiseRemainingResources(tileInfos);
            CreateTiles();

            return Installer.Board = new Board(tiles);
        }

        private void InitialiseRemainingResources(IEnumerable<TileInfo> tileInfos)
        {
            float totalPercent = 0;
            foreach (TileInfo tileInfo in tileInfos)
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
                    tiles.Set(i, j, tileFactory.Create(GetRandomResource()));
                    tiles.Get(i, j).transform.position = new(Tile.Width * i, 0, Tile.Height * j);
                }
            }
        }

        private TileInfo GetRandomResource()
        {
            int index = Random.Range(0, remainingResourceTilesCount);
            TileInfo resourceInfo = remainingResourceTiles.ElementAt(index).Key;

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
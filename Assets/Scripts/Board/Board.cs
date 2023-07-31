using Assets.Scripts.Exceptions;
using LittleKingdom.DataStructures;
using LittleKingdom.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Board
{
    public class Board
    {
        public Grid<Tile> Tiles { get; private set; }

        private readonly Random random = new();
        private readonly Dictionary<string, int> remainingResourceTiles = new();

        private float carryOverTiles = 0;
        private int totalTiles;

        public void Create(int widthInTiles, int heightInTiles, IEnumerable<TileInfo> tileInfos)
        {
            totalTiles = widthInTiles * heightInTiles;
            Tiles = new(widthInTiles, heightInTiles);

            InitialiseRemainingResources(tileInfos);
            CreateTiles();
        }

        private void InitialiseRemainingResources(IEnumerable<TileInfo> tileInfos)
        {
            float totalPercent = 0;
            foreach (TileInfo tileInfo in tileInfos)
            {
                totalPercent += tileInfo.PercentOfBoard;
                remainingResourceTiles[tileInfo.ResourceName] = GetTileAmount(tileInfo.PercentOfBoard);
            }

            if (totalPercent != 100)
            {
                throw new InvalidAmountOfTilesException($"Was given a total tile coverage of {totalPercent}%, it must be exactly 100%.");
            }
        }

        private void CreateTiles()
        {
            for (int i = 0; i < Tiles.Width; i++)
            {
                for (int j = 0; j < Tiles.Height; j++)
                {
                    string resourceName = GetRandomResource();
                    Tiles.Set(i, j, TileFactory.Create(resourceName));
                }
            }
        }

        private string GetRandomResource()
        {
            int index = random.Next(remainingResourceTiles.Count());
            string resourceName = remainingResourceTiles.ElementAt(index).Key;

            remainingResourceTiles[resourceName]--;
            if (remainingResourceTiles[resourceName] == 0)
            {
                remainingResourceTiles.Remove(resourceName);
            }

            return resourceName;
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
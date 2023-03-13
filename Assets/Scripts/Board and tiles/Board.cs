using LittleKingdom.Tiles;
using System;
using System.Collections.Generic;

namespace LittleKingdom.Board
{
	public class Board
	{
		public Tile[,] Tiles { get; private set; }
		private List<(string resourceName, int remainingTiles)> tilesInfo;
		private float carryOverTiles = 0;
		private int totalTiles;

		public void Create(int widthInTiles, int heightInTiles)
		{
			totalTiles = widthInTiles * heightInTiles;
			Tile[] tiles = new Tile[totalTiles];

			foreach(TileInfo tileInfo in References.TilesInfo.Tiles)
            {
				tilesInfo.Add((tileInfo.ResourceName, GetTileAmount(tileInfo.PercentOfBoard)));
            }

			Random random = new();
			int tilesInfoIndex;

			for (int i = 0; i < tiles.Length; i++)
			{
				tilesInfoIndex = random.Next(tilesInfo.Count);
				//tilesInfo[tilesInfoIndex].remainingTiles--;

			}
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
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Board
{
	public class BoardMono : MonoBehaviour
	{
		private Board board;
		private TileMono[,] tiles;
		private int width;
		private int height;
		private float tileWidth;
		private float tileHeight;
		
		public void Initialise(Board board)
        {
			this.board = board;
            MeshRenderer meshRenderer = PrefabReferences.Tile.GetComponent<MeshRenderer>();
            tileWidth = meshRenderer.bounds.size.x;
            tileHeight = meshRenderer.bounds.size.z;
        }

	    private void Start()
        {
            //board.Create();

            width = board.Tiles.GetLength(0);
            height = board.Tiles.GetLength(1);
            tiles = new TileMono[width, height];

            CreateMonoTiles();
        }

        private void CreateMonoTiles()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tiles[i, j] = TileFactory.CreateMono(board.Tiles[i, j]);
                    tiles[i, j].transform.position = new(tileWidth * i, 0, tileHeight * j);
                }
            }
        }
    }
}
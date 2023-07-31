using LittleKingdom.DataStructures;
using LittleKingdom.Factories;
using Mono.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace LittleKingdom.Board
{
	public class BoardMono : MonoBehaviour
	{
        private readonly Board board = new();

        //TODO: move these into settings when it is set up, so this class does not need to be a MonoBehaviour.
        [SerializeField] private int width;
        [SerializeField] private int height;

        public SizedGrid<TileMono> Tiles { get; private set; }
        public float TileWidth { get; private set; }
        public float TileHeight { get; private set; }

		private void Awake()
        {
            MeshRenderer meshRenderer = PrefabReferences.Tile.GetComponent<MeshRenderer>();
            TileWidth = meshRenderer.bounds.size.x;
            TileHeight = meshRenderer.bounds.size.z;
            Tiles = new(width, height, TileWidth, TileHeight);
        }

	    public void Create()
        {
            board.Create(width, height, References.TilesInfo.Tiles);
            CreateMonoTiles();
        }

        private void CreateMonoTiles()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Tiles.Set(i, j, TileFactory.CreateMono(board.Tiles.Get(i, j)));
                    Tiles.Get(i, j).transform.position = new(TileWidth * i, 0, TileHeight * j);
                }
            }
        }
    }
}
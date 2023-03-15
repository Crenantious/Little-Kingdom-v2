using LittleKingdom.Factories;
using UnityEngine;

namespace LittleKingdom.Board
{
	public class BoardMono : MonoBehaviour
	{
        private readonly Board board = new();

        //TODO: move these into settings when it is set up, so this class does not need to be a MonoBehaviour.
        [SerializeField] private int width;
        [SerializeField] private int height;

		private TileMono[,] tiles;
		private float tileWidth;
		private float tileHeight;
		
		private void Awake()
        {
            MeshRenderer meshRenderer = PrefabReferences.Tile.GetComponent<MeshRenderer>();
            tileWidth = meshRenderer.bounds.size.x;
            tileHeight = meshRenderer.bounds.size.z;
            tiles = new TileMono[width, height];
        }

	    private void Start()
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
                    tiles[i, j] = TileFactory.CreateMono(board.Tiles[i, j]);
                    tiles[i, j].transform.position = new(tileWidth * i, 0, tileHeight * j);
                }
            }
        }
    }
}
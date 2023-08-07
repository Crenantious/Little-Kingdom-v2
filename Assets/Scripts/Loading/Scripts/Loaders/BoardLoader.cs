using LittleKingdom.Board;
using LittleKingdom.Loading;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class BoardLoader : Loader<BoardLC>
    {
        [SerializeField] private GameSetupLoader gameSetupLoader;

        private BoardGeneration boardGeneration;

        [Inject]
        public void Construct(BoardGeneration boardGeneration) =>
            this.boardGeneration = boardGeneration;

        private void Awake() => Dependencies.Add(gameSetupLoader);

        public override void Load(BoardLC config) =>
            boardGeneration.Generate(config.WidthInTiles, config.HeightInTiles, config.TileInfo);

        public override void Unload()
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}
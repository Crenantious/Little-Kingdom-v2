using LittleKingdom.Board;
using LittleKingdom.Loading;
using UnityEngine;
using Zenject;

namespace LittleKingdom
{
    public class BoardLoader : Loader<BoardLC>
    {
        [SerializeField] private GameSetupLoader gameSetupLoader;

        private IBoardGenerator boardGenerator;

        [Inject]
        public void Construct(IBoardGenerator boardGenerator) =>
            this.boardGenerator = boardGenerator;

        private void Awake() => Dependencies.Add(gameSetupLoader);

        public override void Load(BoardLC config) =>
            boardGenerator.Generate(config.WidthInTiles, config.HeightInTiles, config.TileInfo);

        public override void Unload()
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}
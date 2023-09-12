using LittleKingdom.Board;
using Zenject;

namespace LittleKingdom.Loading
{
    public class BoardLoader : Loader<BoardLC>
    {
        private IBoardGenerator boardGenerator;

        [Inject]
        public void Construct(IBoardGenerator boardGenerator)
        {
            this.boardGenerator = boardGenerator;
            AddDependency<GameSetupLoader>();
        }

        public override void Load(BoardLC config) =>
            boardGenerator.Generate(config.WidthInTiles, config.HeightInTiles, config.TileInfo);

        public void Unload()
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}
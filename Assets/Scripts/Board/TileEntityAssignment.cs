namespace LittleKingdom.Board
{
    public class TileEntityAssignment
    {
        // TODO: refactor such that BoardMono does not exist; Iboard will, for the sake of testing.
        private readonly Board board;

        public TileEntityAssignment(Board board)
        {
            this.board = board;
        }

        /// <summary>
        /// Assigns the town to each tile within it and those to it.
        /// </summary>
        public void AssignTown(ITown town)
        {
            for (int column = town.OriginTile.Column; column < town.OriginTile.Column + town.Width - 1; column++)
            {
                for (int row = town.OriginTile.Row; row < town.OriginTile.Row + town.Height - 1; row++)
                {
                    Tile tile = board.Tiles.Get(column, row);
                    tile.Town = town;
                    town.Tiles.Set(column, row, tile);
                }
            }
        }
    }
}
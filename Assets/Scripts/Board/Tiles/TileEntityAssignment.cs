namespace LittleKingdom.Board
{
    public class TileEntityAssignment
    {
        private IReferences references;

        public TileEntityAssignment(IReferences references) =>
            this.references = references;

        /// <summary>
        /// Assigns the town to each tile within it and those to it.
        /// </summary>
        public void AssignTown(ITown town, ITile origin)
        {
            town.OriginTile = origin;
            origin.Town = town;

            for (int column = 0; column < town.Width; column++)
            {
                for (int row = 0; row < town.Height; row++)
                {
                    ITile tile = references.Board.Tiles.Get(column + town.OriginTile.Column, row + town.OriginTile.Row);
                    tile.Town = town;
                    town.Tiles.Set(column, row, tile);
                }
            }
        }
    }
}
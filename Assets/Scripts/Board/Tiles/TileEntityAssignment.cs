namespace LittleKingdom.Board
{
    public class TileEntityAssignment
    {
        private readonly IReferences references;

        public TileEntityAssignment(IReferences references) =>
            this.references = references;

        public void AssignTown(ITown town, ITile origin)
        {
            town.OriginTile = origin;
            origin.Town = town;

            for (int column = 0; column < town.Width; column++)
            {
                for (int row = 0; row < town.Height; row++)
                {
                    ITile tile = references.Board.Tiles.Get(town.OriginTile.Column + column, town.OriginTile.Row - row);
                    tile.Town = town;
                    town.Tiles.Set(column, row, tile);
                }
            }
        }
    }
}
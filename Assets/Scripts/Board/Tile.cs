namespace LittleKingdom.Board
{
    public class Tile
    {
        // TODO: JR - make this a flag enum.
        /// <summary>
        /// The resource that this tile produces.
        /// </summary>
        public string ResourceName { get; private set; }

        /// <summary>
        /// The town the occupies this tile.
        /// </summary>
        public Town Town { get; set; }

        public Tile(string resourceName)
        {
            ResourceName = resourceName;
        }
    }
}
namespace LittleKingdom.Board
{
    public class Tile
    {
        public string ResourceName { get; private set; }

        public Tile(string resourceType)
        {
            ResourceName = resourceType;
        }
    }
}
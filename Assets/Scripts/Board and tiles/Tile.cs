public class Tile
{
    public string ResourceType { get; private set; }

    public Tile(string resourceType)
    {
        ResourceType = resourceType;
    }
}
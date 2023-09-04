namespace LittleKingdom.Resources
{
    /// <summary>
    /// Represents the maximum resources to halt movement. This is not guaranteed to be the amount halted at time of resolution
    /// since other requests may have resolved first.
    /// </summary>
    public record HaltResourcesRequest(IHoldResources From, IHoldResources To, Resources Resources);
}
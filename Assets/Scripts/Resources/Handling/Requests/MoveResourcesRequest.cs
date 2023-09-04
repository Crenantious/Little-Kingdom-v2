namespace LittleKingdom.Resources
{
    /// <summary>
    /// Represents the maximum resources to move. This is not guaranteed to be the amount moved at time of resolution
    /// since other requests may have resolved first.
    /// </summary>
    public record MoveResourcesRequest(IHoldResources From, IHoldResources To, Resources Resources);
}
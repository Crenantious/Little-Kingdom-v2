namespace LittleKingdom.Resources
{
    public record MoveResourcesRequest(IHoldResources From, IHoldResources To, Resources Resources);
}
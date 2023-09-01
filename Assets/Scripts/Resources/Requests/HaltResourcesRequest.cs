namespace LittleKingdom.Resources
{
    public record HaltResourcesRequest(IHoldResources From, IHoldResources To, ResourceAmounts Resources);
}
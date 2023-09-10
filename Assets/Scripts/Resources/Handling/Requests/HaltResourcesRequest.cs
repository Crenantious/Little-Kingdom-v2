using System;

namespace LittleKingdom.Resources
{
    /// <summary>
    /// Represents the maximum resources to halt movement of. This is not guaranteed to be the amount halted at time of resolution
    /// since other requests may have resolved first.
    /// </summary>
    public record HaltResourcesRequest(IHoldResources From, IHoldResources To, Resources Resources) :
        ValidatableRecord()
    {
        public override void Validate()
        {
            if (Resources is null)
                throw new ArgumentNullException(nameof(Resources), "Must specify resources to halt.");
        }
    }
}
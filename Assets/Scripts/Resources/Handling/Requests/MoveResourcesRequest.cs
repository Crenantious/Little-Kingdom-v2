using System;

namespace LittleKingdom.Resources
{
    /// <summary>
    /// Represents the maximum resources to move. This is not guaranteed to be the amount moved at time of resolution
    /// since other requests may have resolved first.
    /// </summary>
    public record MoveResourcesRequest(IHoldResources From, IHoldResources To, Resources Resources) :
        ValidatableRecord()
    {
        public override void Validate()
        {
            if (From is null)
                throw new ArgumentNullException(nameof(From), "Must specify an object to move resources from.");
            if (To is null)
                throw new ArgumentNullException(nameof(To), "Must specify an object to move resources to.");
            if (Resources is null)
                throw new ArgumentNullException(nameof(Resources), "Must specify the resources to move.");
        }
    }
}
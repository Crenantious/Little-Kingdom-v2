using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Resources
{
    public class ResourceHandlingUtilities : IResourceHandlingUtilities
    {
        /// <inheritdoc/>
        public void AccountForAvailableResources(MoveResourcesRequest moveRequest) =>
            moveRequest.Resources.ClampMin(moveRequest.From.Resources);

        /// <inheritdoc/>
        public void AccountForHoldingCapacity(MoveResourcesRequest moveRequest) =>
            moveRequest.Resources.ClampMin(Resources.Subtract(moveRequest.To.ResourcesCapacity, moveRequest.To.Resources));

        /// <inheritdoc/>
        public IEnumerable<HaltResourcesRequest> GetMatchingHaltRequests(
            MoveResourcesRequest moveRequest, IEnumerable<HaltResourcesRequest> haltRequests) =>
            // If From or To are null, they are valid for any movement request.
            haltRequests.Where(r =>
                (r.From == null || r.From == moveRequest.From) &&
                (r.To == null || r.To == moveRequest.To));
    }
}
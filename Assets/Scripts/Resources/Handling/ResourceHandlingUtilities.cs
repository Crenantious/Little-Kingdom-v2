using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Resources
{
    public class ResourceHandlingUtilities : IResourceHandlingUtilities
    {
        public void AccountForHoldCapacity(MoveResourcesRequest moveRequest)
        {
            moveRequest.Resources.ClampMin(moveRequest.From.Resources);
            moveRequest.Resources.ClampMin(Resources.Subtract(moveRequest.To.ResourcesCapactiy, moveRequest.To.Resources));
        }

        public IEnumerable<HaltResourcesRequest> GetMatchingHaltRequests(
            MoveResourcesRequest moveRequest, IEnumerable<HaltResourcesRequest> haltRequests) =>
            // If From or To are null, they are valid for any movement request.
            haltRequests.Where(r =>
                (r.From == null || r.From == moveRequest.From) &&
                (r.To == null || r.To == moveRequest.To));
    }
}
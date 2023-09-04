using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Resources
{
    public class ResolveHaltRequests
    {
        public void Resolve(List<HaltResourcesRequest> haltRequests, List<MoveResourcesRequest> moveRequests)
        {
            foreach (MoveResourcesRequest moveRequest in moveRequests)
            {
                AccountForHoldCapacity(moveRequest);
                ResolveHaltRequestsFor(moveRequest, haltRequests);
            }
        }

        private static void AccountForHoldCapacity(MoveResourcesRequest moveRequest)
        {
            // Resources cannot be moved if they would exceed either the amount
            // the From holder has, or the amount the To holder can carry.
            moveRequest.Resources.ClampMin(moveRequest.From.Resources);
            moveRequest.Resources.ClampMin(Resources.Subtract(moveRequest.To.ResourcesCapactiy, moveRequest.To.Resources));
        }

        // This and GetMatchingHaltRequests could be collapsed into one method do the haltRequests are not iterated twice.
        // haltRequests could also be stored in a better data structure
        // (multiple lists and dictionaries) to avoid constant iteration.
        private void ResolveHaltRequestsFor(MoveResourcesRequest moveRequest, List<HaltResourcesRequest> haltRequests)
        {
            foreach (HaltResourcesRequest haltRequest in GetMatchingHaltRequests(moveRequest, haltRequests))
            {
                var reduction = Resources.ClampMin(moveRequest.Resources, haltRequest.Resources);
                moveRequest.Resources.Subtract(reduction);
                haltRequest.Resources.Subtract(reduction);
                if (moveRequest.Resources.Total == 0)
                    return;
            }
        }

        private IEnumerable<HaltResourcesRequest> GetMatchingHaltRequests(
            MoveResourcesRequest moveRequest, List<HaltResourcesRequest> haltRequests) =>
            // If From or To are null, they are valid for any movement request.
            haltRequests.Where(r =>
                (r.From == null || r.From == moveRequest.From) &&
                (r.To == null || r.To == moveRequest.To));
    }
}
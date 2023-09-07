using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public class ResolveHaltRequests
    {
        private readonly IResourceHandlingUtilities utilities;

        public ResolveHaltRequests(IResourceHandlingUtilities utilities) => this.utilities = utilities;

        /// <summary>
        /// Removes resources from <paramref name="moveRequests"/> to account for those halted by <paramref name="haltRequests"/>.<br/>
        /// Also removes those resources from <paramref name="haltRequests"/> to reflect their remaining potential.
        /// </summary>
        public void Resolve(IEnumerable<HaltResourcesRequest> haltRequests, IEnumerable<MoveResourcesRequest> moveRequests)
        {
            foreach (MoveResourcesRequest moveRequest in moveRequests)
            {
                utilities.AccountForHoldCapacity(moveRequest);
                ResolveHaltRequestsFor(moveRequest, haltRequests);
            }
        }

        // This and GetMatchingHaltRequests could be collapsed into one method do the haltRequests are not iterated twice.
        // haltRequests could also be stored in a better data structure
        // (multiple lists and dictionaries) to avoid constant iteration.
        private void ResolveHaltRequestsFor(MoveResourcesRequest moveRequest, IEnumerable<HaltResourcesRequest> haltRequests)
        {
            foreach (HaltResourcesRequest haltRequest in utilities.GetMatchingHaltRequests(moveRequest, haltRequests))
            {
                var reduction = Resources.ClampMin(moveRequest.Resources, haltRequest.Resources);
                moveRequest.Resources.Subtract(reduction);
                haltRequest.Resources.Subtract(reduction);
                if (moveRequest.Resources.Total == 0)
                    return;
            }
        }
    }
}
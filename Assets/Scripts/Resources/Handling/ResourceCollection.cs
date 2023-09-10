using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public class ResourceCollection
    {
        private readonly ResourceRequests<IHaltResources, HaltResourcesRequest> haltResourcesRequests;
        private readonly ResourceRequests<IMoveResources, MoveResourcesRequest> moveResourcesRequests;
        private readonly ResolveHaltRequests resolveHaltRequests;
        private readonly ResolveMoveRequests resolveMoveRequests;
        private readonly IResourceHandlingUtilities utilities;

        public ResourceCollection(ResourceRequests<IHaltResources, HaltResourcesRequest> haltResourcesRequests,
            ResourceRequests<IMoveResources, MoveResourcesRequest> moveResourcesRequests,
            ResolveHaltRequests resolveHaltRequests, ResolveMoveRequests resolveMoveRequests,
            IResourceHandlingUtilities utilities)
        {
            this.haltResourcesRequests = haltResourcesRequests;
            this.moveResourcesRequests = moveResourcesRequests;
            this.resolveHaltRequests = resolveHaltRequests;
            this.resolveMoveRequests = resolveMoveRequests;
            this.utilities = utilities;
        }

        public void CollectFor(IPlayer player)
        {
            IEnumerable<HaltResourcesRequest> haltRequests = haltResourcesRequests.GetRequests(player);
            IEnumerable<MoveResourcesRequest> moveRequests = moveResourcesRequests.GetRequests(player);
            AccountMoveRequestsForHolderResources(moveRequests);
            resolveHaltRequests.Resolve(haltRequests, moveRequests);
            resolveMoveRequests.Resolve(moveRequests);
        }

        private void AccountMoveRequestsForHolderResources(IEnumerable<MoveResourcesRequest> requests)
        {
            foreach(var request in requests)
            {
                utilities.AccountForAvailableResources(request);
                utilities.AccountForHoldingCapacity(request);
            }
        }
    }
}
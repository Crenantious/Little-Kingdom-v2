using System;
using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public class ResourceCollection
    {
        private readonly ResourceRequests<IHaltResources, HaltResourcesRequest> haltResourcesRequests;
        private readonly ResourceRequests<IMoveResources, MoveResourcesRequest> moveResourcesRequests;
        private readonly ResolveHaltRequests resolveHaltRequests;
        private readonly ResolveMoveRequests resolveMoveRequests;

        public ResourceCollection(ResourceRequests<IHaltResources, HaltResourcesRequest> haltResourcesRequests,
            ResourceRequests<IMoveResources, MoveResourcesRequest> moveResourcesRequests,
            ResolveHaltRequests resolveHaltRequests, ResolveMoveRequests resolveMoveRequests)
        {
            this.haltResourcesRequests = haltResourcesRequests;
            this.moveResourcesRequests = moveResourcesRequests;
            this.resolveHaltRequests = resolveHaltRequests;
            this.resolveMoveRequests = resolveMoveRequests;
        }

        public void CollectFor(IPlayer player)
        {
            List<HaltResourcesRequest> haltRequests = haltResourcesRequests.GetRequests(player);
            List<MoveResourcesRequest> moveRequests = moveResourcesRequests.GetRequests(player);
            resolveHaltRequests.Resolve(haltRequests, moveRequests);
            resolveMoveRequests.Resolve(moveRequests);
        }
    }
}
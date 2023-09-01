using System;
using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Resources
{
    public class ResourceCollection
    {
        private readonly Dictionary<Type, List<IProduceResources>> producers = new();
        private readonly Dictionary<Type, List<IMoveResources>> movers = new();
        private readonly Dictionary<Type, List<IHaltResources>> halters = new();
        private readonly List<HaltResourcesRequest> haltRequests = new();
        private readonly List<MoveResourcesRequest> moveRequests = new();

        public ResourceCollectionOrder ResourceCollectionOrder { get; }

        public ResourceCollection(ResourceCollectionOrder collectionOrder) =>
            ResourceCollectionOrder = collectionOrder;

        public void RegisterHandler(IProduceResources handler) =>
            AddHandler(handler, producers);

        public void RegisterHandler(IMoveResources handler) =>
            AddHandler(handler, movers);

        public void RegisterHandler(IHaltResources handler) =>
            AddHandler(handler, halters);

        public void CollectFor(IPlayer player)
        {
            PopulateHaltRequests(player);
            ResolveMoveRequestsFor(player);
        }

        private void PopulateHaltRequests(IPlayer player)
        {
            haltRequests.Clear();

            foreach (Type type in ResourceCollectionOrder.Halters)
            {
                foreach (IHaltResources halter in halters[type])
                {
                    if (halter.Player is null || halter.Player == player)
                    {
                        haltRequests.Concat(halter.GetHaltRequests());
                    }
                }
            }
        }

        private void ResolveMoveRequestsFor(IPlayer player)
        {
            moveRequests.Clear();

            foreach (Type moverType in ResourceCollectionOrder.Movers)
            {
                foreach (IMoveResources mover in movers[moverType])
                {
                    if (mover.Player is null || mover.Player == player)
                        ResolveMoveRequestsFor(mover);
                }
            }
        }

        private void ResolveMoveRequestsFor(IMoveResources mover)
        {
            foreach (MoveResourcesRequest moveRequest in mover.GetMoveRequests())
            {
                AccountResourcesForHoldCapacity(moveRequest);
                ResolveHaltRequestsFor(moveRequest);

                if (moveRequest.Resources.Total > 0)
                    MoveResources(moveRequest);
            }
        }

        private static void AccountResourcesForHoldCapacity(MoveResourcesRequest moveRequest)
        {
            // Resources cannot be moved if they would exceed either the amount
            // the From holder has, or the amount the To holder can carry.
            moveRequest.Resources.ClampMin(moveRequest.From.Resources);
            moveRequest.Resources.ClampMin(ResourceAmounts.Subtract(moveRequest.To.ResourcesCapactiy, moveRequest.To.Resources));
        }

        private void ResolveHaltRequestsFor(MoveResourcesRequest moveRequest)
        {
            foreach (HaltResourcesRequest haltRequest in GetMatchingHaltRequests(moveRequest))
            {
                var reduction = ResourceAmounts.ClampMin(moveRequest.Resources, haltRequest.Resources);
                moveRequest.Resources.Subtract(reduction);
                haltRequest.Resources.Subtract(reduction);
                if (moveRequest.Resources.Total == 0)
                    return;
            }
        }

        private IEnumerable<HaltResourcesRequest> GetMatchingHaltRequests(MoveResourcesRequest moveRequest) =>
            // If From or To are null, they are valid for any movement request.
            haltRequests.Where(r =>
                (r.From == null || r.From == moveRequest.From) &&
                (r.To == null || r.To == moveRequest.To));

        private static void MoveResources(MoveResourcesRequest moveRequest)
        {
            moveRequest.From.Resources.Subtract(moveRequest.Resources);
            moveRequest.To.Resources.Add(moveRequest.Resources);
        }

        private void AddHandler<T>(T handler, Dictionary<Type, List<T>> dict)
        {
            Type handlerType = handler.GetType();
            if (dict.ContainsKey(handlerType) is false)
                dict[handlerType] = new();
            dict[handlerType].Add(handler);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Resources
{
    public class ResourceRequests<THandler, TRequest>
        where THandler : IHandleResources<TRequest>
    {
        protected ResourceCollectionOrder ResourceCollectionOrder { get; }
        protected Dictionary<Type, List<THandler>> Requests { get; } = new();

        public ResourceRequests(ResourceCollectionOrder collectionOrder) =>
            ResourceCollectionOrder = collectionOrder;

        public void RegisterHandler(THandler handler) =>
            AddHandler(handler, Requests);

        public List<TRequest> GetRequests(IPlayer player)
        {
            List<TRequest> moveRequests = new();

            foreach (Type type in ResourceCollectionOrder.Movers)
            {
                foreach (THandler mover in Requests[type])
                {
                    if (mover.Player is null || mover.Player == player)
                    {
                        moveRequests.Concat(mover.GetRequests());
                    }
                }
            }

            return moveRequests;
        }

        protected void AddHandler<K>(K handler, Dictionary<Type, List<K>> dict)
        {
            Type handlerType = handler.GetType();
            if (dict.ContainsKey(handlerType) is false)
                dict[handlerType] = new();
            dict[handlerType].Add(handler);
        }
    }
}
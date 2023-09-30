using System;
using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Resources
{
    public class RegisteredResourceRequests<THandler, TRequest>
        where THandler : IHandleResources<TRequest>
    {
        private readonly IResourceCollectionOrder resourceCollectionOrder;
        private readonly Dictionary<Type, List<THandler>> requests = new();

        public RegisteredResourceRequests(IResourceCollectionOrder collectionOrder) =>
            resourceCollectionOrder = collectionOrder;

        public void RegisterHandler(THandler handler) =>
            AddHandler(handler);

        public IEnumerable<TRequest> GetRequests(IPlayer player)
        {
            if (player is null)
                throw new ArgumentNullException("Player cannot be null. Can only get resources for an existing player.");

            IEnumerable<TRequest> result = new TRequest[] { };

            foreach (Type type in resourceCollectionOrder.GetOrderFor<THandler>())
            {
                foreach (THandler handler in GetRequests(type))
                {
                    if (handler.Player is null || handler.Player == player)
                    {
                        result = result.Concat(handler.GetRequests());
                    }
                }
            }

            return result;
        }

        private List<THandler> GetRequests(Type type) =>
            requests.ContainsKey(type) ? requests[type] : new();

        protected void AddHandler(THandler handler)
        {
            Type handlerType = handler.GetType();
            if (requests.ContainsKey(handlerType) is false)
                requests[handlerType] = new();
            requests[handlerType].Add(handler);
        }
    }
}
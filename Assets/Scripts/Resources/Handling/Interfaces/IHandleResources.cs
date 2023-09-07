using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public interface IHandleResources
    {
        public IPlayer Player { get; }
    }

    public interface IHandleResources<T> : IHandleResources
    {
        public IEnumerable<T> GetRequests();
    }
}
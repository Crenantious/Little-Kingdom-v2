using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public interface IHandleResources<T>
    {
        public IPlayer Player { get; }
        public IEnumerable<T> GetRequests();
    }
}
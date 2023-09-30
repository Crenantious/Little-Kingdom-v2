using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public interface IHandleResources
    {

#nullable enable
        public IPlayer? Player { get; }
#nullable disable

    }

    public interface IHandleResources<T> : IHandleResources
    {
        public IEnumerable<T> GetRequests();
    }
}
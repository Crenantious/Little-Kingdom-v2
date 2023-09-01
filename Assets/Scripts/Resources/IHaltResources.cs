using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public interface IHaltResources
    {
        public IPlayer Player { get; }
        public IEnumerable<HaltResourcesRequest> GetHaltRequests();
    }
}
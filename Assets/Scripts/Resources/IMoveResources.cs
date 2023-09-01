using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public interface IMoveResources
    {
        public IPlayer Player { get; }
        public IEnumerable<MoveResourcesRequest> GetMoveRequests();
    }
}
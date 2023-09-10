using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public interface IResourceHandlingUtilities
    {
        /// <summary>
        /// Resources cannot be moved if they would exceed the amount
        /// the <paramref name="moveRequest"/>.From holder has.
        /// Resources are deducted from <paramref name="moveRequest"/> to account for this.
        /// </summary>
        /// 
        public void AccountForAvailableResources(MoveResourcesRequest moveRequest);

        /// <summary>
        /// Resources cannot be moved if they would exceed the amount
        /// the <paramref name="moveRequest"/>.To holder can carry.
        /// Resources are deducted from <paramref name="moveRequest"/> to account for this.
        /// </summary>
        public void AccountForHoldingCapacity(MoveResourcesRequest moveRequest);

        /// <returns>
        /// All requests in <paramref name="haltRequests"/> that attempt to halt the movement of any resources in <paramref name="moveRequest"/>.
        /// </returns>
        public IEnumerable<HaltResourcesRequest> GetMatchingHaltRequests(
            MoveResourcesRequest moveRequest, IEnumerable<HaltResourcesRequest> haltRequests);
    }
}
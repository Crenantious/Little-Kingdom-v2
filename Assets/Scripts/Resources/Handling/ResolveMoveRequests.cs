using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    public class ResolveMoveRequests
    {
        /// <summary>
        /// Move all resources as requested in <paramref name="moveRequests"/>.
        /// </summary>
        public void Resolve(IEnumerable<MoveResourcesRequest> moveRequests)
        {
            foreach (MoveResourcesRequest request in moveRequests)
            {
                if (request.Resources.Total > 0)
                    MoveResources(request);
            }
        }

        private static void MoveResources(MoveResourcesRequest moveRequest)
        {
            moveRequest.From.Resources.Subtract(moveRequest.Resources);
            moveRequest.To.Resources.Add(moveRequest.Resources);
        }
    }
}
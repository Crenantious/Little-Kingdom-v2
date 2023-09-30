using System;
using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Resources
{
    [Serializable]
    public class HaltResourcesRequestsFormulator
    {
        private readonly IGetResourceHolders haltFrom;
        private readonly IGetResourceHolders haltTo;
        private readonly Resources resources;

        public HaltResourcesRequestsFormulator(IGetResourceHolders haltFrom,
            IGetResourceHolders haltTo, Resources resources)
        {
            this.haltFrom = haltFrom;
            this.haltTo = haltTo;
            this.resources = resources;
        }

        public IEnumerable<HaltResourcesRequest> GetRequests() =>
            from IHoldResources fromHolder in haltFrom.Get()
            from IHoldResources toHolder in haltTo.Get()
            select new HaltResourcesRequest(fromHolder, toHolder, resources);
    }
}
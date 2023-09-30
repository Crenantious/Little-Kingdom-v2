using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleKingdom.Resources
{
    public class HaltResources : MonoBehaviour, IHaltResources
    {
        [SerializeField] private List<HaltResourcesRequestsFormulator> formulators = new();

        [field: SerializeField] public IPlayer Player { get; private set; }

        public IEnumerable<HaltResourcesRequest> GetRequests() =>
            from HaltResourcesRequestsFormulator formulator in formulators
            from HaltResourcesRequest request in formulator.GetRequests()
            select request;
    }
}
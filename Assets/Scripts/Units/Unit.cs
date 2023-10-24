using LittleKingdom.Board;
using LittleKingdom.DataStructures;
using LittleKingdom.Resources;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Units
{
    public class Unit : MonoBehaviour, IPlaceableInTile
    {
        [field: SerializeField] public UnitType UnitType { get; private set; }
        [field: SerializeField] public UnitTypeFlags UnitTypeFlags { get; private set; }

        public ITile OriginTile { get; set; }
        public Grid<ITile> Tiles { get; set; }

        [Inject]
        public void Construct(RegisteredMoveResourceRequests moveResourceRequests, RegisteredHaltResourceRequests haltResourcesRequest)
        {
            IMoveResources moveResources = GetComponent<IMoveResources>();
            IHaltResources haltResources = GetComponent<IHaltResources>();

            if (moveResources is not null)
                moveResourceRequests.RegisterHandler(moveResources);

            if (haltResources is not null)
                haltResourcesRequest.RegisterHandler(haltResources);
        }
    }
}
using LittleKingdom.Board;
using LittleKingdom.DataStructures;
using LittleKingdom.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleKingdom.Resources
{
    [Serializable]
    public class GetResourceHoldersFromUnitTypes : IGetResourceHolders
    {
        [SerializeField] private UnitType unitTypes;

        [SerializeField] private IGetTiles getTiles;

        // It is assumed that the unit inherits IHoldResources as the given types are set via an Editor.
        public IEnumerable<IHoldResources> Get() =>
            throw new NotImplementedException();
            //from ITile tile in getTiles.Get()
            //from Unit unit in tile.Units
            //where unitTypes.Contains(unit.UnitType)
            //select (IHoldResources)unit;
    }
}
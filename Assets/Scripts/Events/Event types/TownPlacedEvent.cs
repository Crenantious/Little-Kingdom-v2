using System;
using UnityEngine;

namespace LittleKingdom.Events
{
    public class TownPlacedEvent : Event<TownPlacedEvent.EventData>
    {
        [Serializable]
        public record EventData(ITown Town) : EventDataBase
        {
            [field: SerializeField] public ITown Town { get; private set; } = Town;
        }
    }
}
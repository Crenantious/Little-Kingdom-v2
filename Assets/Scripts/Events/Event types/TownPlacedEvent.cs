using System;
using UnityEngine;

namespace LittleKingdom.Events
{
    public class TownPlacedEvent : Event<TownPlacedEvent.EventData>
    {
        [Serializable]
        public record EventData(Town Town) : EventDataBase
        {
            [field: SerializeField] public Town Town { get; private set; } = Town;
        }
    }
}
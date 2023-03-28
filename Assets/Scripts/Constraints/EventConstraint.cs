using LittleKingdom.Events;
using UnityEngine;
using LittleKingdom.Attributes;

namespace LittleKingdom.Constraints
{
    public class EventConstraint<EventType> : IConstraint
        where EventType : IEvent
    {
        [SerializeReference, AllowDerived] private EventData requirement;

        public EventConstraint(EventData requirement) =>
            this.requirement = requirement;

        public bool Validate() =>
            RecentEventData.Get<EventType>() == requirement;
    }
}
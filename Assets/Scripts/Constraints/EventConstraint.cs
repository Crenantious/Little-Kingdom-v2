using LittleKingdom.Events;
using UnityEngine;
using LittleKingdom.Attributes;

namespace LittleKingdom.Constraints
{
    public class EventConstraint<EventType, Testa, Testb> : Constraint
        where EventType : IEvent
    {
        [SerializeReference, AllowDerived] private EventDataBase requirement;

        public EventConstraint(EventDataBase requirement) =>
            this.requirement = requirement;

        public override bool Validate() =>
            RecentEventData.Get<EventType>() == requirement;
    }
}
using LittleKingdom.Events;

namespace LittleKingdom.Constraints
{
    public class EventConstraint<EventType> : IConstraint
        where EventType : IEvent
    {
        private readonly EventData requirement;

        public EventConstraint(EventData requirement) =>
            this.requirement = requirement;

        public bool Validate() =>
            RecentEventData.Get<EventType>() == requirement;
    }
}
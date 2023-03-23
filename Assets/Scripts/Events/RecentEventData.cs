using System;
using System.Collections.Generic;

namespace LittleKingdom.Events
{
    public static class RecentEventData
    {
        private static Dictionary<Type, EventData> eventsData = new();

        public static EventData Get<EventType>() where EventType : IEvent =>
            eventsData[typeof(EventType)];

        internal static void Update<EventType>(EventType eventType, EventData eventData) where EventType : IEvent =>
            eventsData[typeof(EventType)] = eventData;
    }
}
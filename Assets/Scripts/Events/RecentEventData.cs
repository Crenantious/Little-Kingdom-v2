using System;
using System.Collections.Generic;

namespace LittleKingdom.Events
{
    public static class RecentEventData
    {
        private static Dictionary<Type, EventDataBase> eventsData = new();

        public static EventDataBase Get<EventType>() where EventType : IEvent =>
            eventsData[typeof(EventType)];

        internal static void Update<EventType>(EventType eventType, EventDataBase eventData) where EventType : IEvent =>
            eventsData[typeof(EventType)] = eventData;
    }
}
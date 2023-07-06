using LittleKingdom.Events;

namespace LittleKingdom.Tests
{
    internal class TestEvent : Event<TestEvent.TestEventData>
    {
        public record TestEventData(string TestValue) : EventDataBase;
    }
}
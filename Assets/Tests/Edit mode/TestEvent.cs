using LittleKingdom.Events;

namespace EventTests
{
    internal class TestEvent : Event<TestEvent.TestEventData>
    {
        public record TestEventData(string TestValue) : EventDataBase;
    }
}
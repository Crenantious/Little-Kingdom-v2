using LittleKingdom.Constraints;
using System.Collections.Generic;
using System.Linq;

namespace LittleKingdom.Events
{
    public abstract class Event<EventDataType> : IEvent
        where EventDataType : EventDataBase
    {
        private readonly List<(Callback callback, IConstraint[] constraints)> subscribers = new();

        public delegate void Callback(EventDataType eventData);

        public void Invoke(EventDataType eventData)
        {
            RecentEventData.Update(this, eventData);

            foreach (var (callback, constraints) in subscribers)
            {
                if (ValidateCallback(constraints))
                {
                    callback.Invoke(eventData);
                }
            }
        }

        private static bool ValidateCallback(IEnumerable<IConstraint> constraints)
        {
            foreach (IConstraint constraint in constraints)
            {
                if (constraint.Validate() is false) { return false; }
            }
            return true;
        }

        public void Subscribe(Callback callback, params IConstraint[] constraints) =>
            subscribers.Add((callback, constraints));

        public void Unsubscribe(Callback callback, params IConstraint[] constraints) =>
            subscribers.RemoveAll(s =>
                s.callback == callback &&
                Enumerable.SequenceEqual(s.constraints, constraints));

        public void Unsubscribe(Callback callback) =>
             subscribers.RemoveAll(s =>
             s.callback == callback);
    }
}
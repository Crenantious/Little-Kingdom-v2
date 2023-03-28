using LittleKingdom.Events;
using UnityEngine;
using LittleKingdom.Attributes;

namespace LittleKingdom.Constraints
{
    public class EventConstrainth : IConstraint
    {
        [SerializeReference, AllowDerived] private EventData requirement;

        public EventConstrainth(EventData requirement) =>
            this.requirement = requirement;

        public bool Validate() =>
            true;
    }
}
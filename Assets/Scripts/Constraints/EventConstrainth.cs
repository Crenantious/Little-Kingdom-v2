using LittleKingdom.Events;
using UnityEngine;
using LittleKingdom.Attributes;

namespace LittleKingdom.Constraints
{
    public class EventConstrainth : Constraint
    {
        [SerializeReference, AllowDerived] private EventDataBase requirement;

        public EventConstrainth(EventDataBase requirement) =>
            this.requirement = requirement;

        public override bool Validate() =>
            true;
    }
}
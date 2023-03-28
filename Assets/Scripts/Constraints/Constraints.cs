using LittleKingdom.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Constraints
{
    [System.Serializable]
    public class Constraints
    {
        [SerializeField] private IEnumerable<IConstraint> constraints;
        //[SerializeField] private IConstraint constraint;
        [SerializeReference, AllowDerived] private IConstraint constraint1;
        [SerializeReference, AllowDerived] private TestConstraint constraint2;
        [SerializeField] private int testingint = 11;
        public Constraints(IEnumerable<IConstraint> constraints) =>
            this.constraints = constraints;

        public Constraints() { }

        public bool Validate()
        {
            foreach (IConstraint constraint in constraints)
            {
                if (constraint.Validate() is false) { return false; }
            }
            return true;
        }
    }
}
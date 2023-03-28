using LittleKingdom.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Constraints
{
    [System.Serializable]
    public class Constraints
    {
        [SerializeReference, AllowDerived] private IConstraint[] constraints;

        public Constraints(params IConstraint[] constraints) =>
            this.constraints = constraints;

        public bool Validate()
        {
            foreach (IConstraint constraint in constraints)
            {
                if (constraint.Validate() is false)
                    return false;
            }
            return true;
        }
    }
}
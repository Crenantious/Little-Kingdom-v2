using System.Collections.Generic;

namespace LittleKingdom.Constraints
{
    public class Constraints
    {
        private readonly IEnumerable<IConstraint> constraints;

        public Constraints(IEnumerable<IConstraint> constraints) =>
            this.constraints = constraints;

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
using System.Collections.Generic;

namespace LittleKingdom.Constraints
{
    public class Constraints
    {
        private IEnumerable<IConstraint> constraints;

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
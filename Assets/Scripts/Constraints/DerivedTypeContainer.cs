using UnityEngine;

namespace LittleKingdom.Constraints
{
    [System.Serializable]
    public class DerivedTypeContainer
    {
        [SerializeField] public int value4;
        public int value5 = 5;
        [SerializeField] public Constraint1 value6;
        [SerializeField] public Constraint value7;
        //[SerializeField] public TestConstraint1<TestConstraint3> tc1;
        [SerializeField] public Constraint constraint = new TestConstraint1<TestConstraint2>();

        public void OnTypeConfigured(Constraint constraint)
        {
            this.constraint = constraint;
        }
    }


    [System.Serializable]
    public class Constraint1
    {
    }
}
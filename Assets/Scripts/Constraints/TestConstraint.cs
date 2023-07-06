using LittleKingdom.Events;
using UnityEngine;
using System;
using LittleKingdom.Attributes;

namespace LittleKingdom.Constraints
{
    [Serializable]
    public class TestConstraint1<A> : Constraint
        where A : TestConstraint2
    {
        public int T1 = 1;

        public override bool Validate()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class TestConstraint2
    {
        public int T2 = 2;
    }

    [Serializable]
    public class TestConstraint3<B> : TestConstraint2
        where B : TestConstraint2
    {
        public int T3 = 3;
    }

    [Serializable]
    public class TestConstraint4<C, D> : TestConstraint2
    where C : TestConstraint2
    where D : TestConstraint2
    {
        public int T4 = 4;
        public int T5 = 5;
    }
}
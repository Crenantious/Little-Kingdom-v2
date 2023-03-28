using LittleKingdom.Events;
using UnityEngine;
using System;
using LittleKingdom.Attributes;

namespace LittleKingdom.Constraints
{
    [Serializable]
    public class TestConstraint
    {
        public int haha = 98;
    }

    [Serializable]
    public class TestConstraint2 : TestConstraint
    {
        public int aa = 98;
        [SerializeReference, AllowDerived] private TestConstraint constraint2;
        [SerializeReference, AllowDerived] private EventData requirement;
    }

    [Serializable]
    public class TestConstraint1 : TestConstraint
    {
        public int bb = 98;
    }
}
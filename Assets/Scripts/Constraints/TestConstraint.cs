using LittleKingdom.Events;
using UnityEngine;
using System;

namespace LittleKingdom.Constraints
{
    [Serializable]
    public class TestConstraint// : MonoBehaviour// : IConstraint
    {
        public int haha = 98;

        //public bool Validate() =>
        //    true;
    }

    [Serializable]
    public class TestConstraint2 : TestConstraint// : MonoBehaviour// : IConstraint
    {
        public int aa = 98;

        //public bool Validate() =>
        //    true;
    }

    [Serializable]
    public class TestConstraint1 : TestConstraint// : MonoBehaviour// : IConstraint
    {
        public int bb = 98;

        //public bool Validate() =>
        //    true;
    }
}
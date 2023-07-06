using LittleKingdom.Attributes;
using System;
using UnityEngine;

namespace LittleKingdom.Constraints
{
    [System.Serializable]
    public class Constraints
    {
        //[SerializeReference, AllowDerived] private IConstraint[] constraints;
        [SerializeReference, AllowDerived] public DerivedTypeContainer container = new();
        [SerializeField] public int value;
        //[SerializeField] public IConstraint value;

        //public Constraints(params IConstraint[] constraints) =>
        //    this.constraints = constraints;

        //public bool Validate()
        //{
        //    foreach (IConstraint constraint in constraints)
        //    {
        //        if (constraint.Validate() is false)
        //            return false;
        //    }
        //    return true;
        //}

    }
}
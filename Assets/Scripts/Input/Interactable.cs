using System.Collections.Generic;
using UnityEngine;
using LittleKingdom.Constraints;
using LittleKingdom.Events;

namespace LittleKingdom
{
    public class Interactable : MonoBehaviour
    {
        //[SerializeField] private List<Interaction> interaction;
        [SerializeField] private Constraints.Constraints constraints;
        //[SerializeField] private IConstraint constraint = new TestConstraint();
        [SerializeReference] private TestConstraint constraint11 = new TestConstraint1();


        private void OnMouseDown()
        {
            
        }
    }
}
using UnityEngine;
using System;
using UnityEngine.Events;

namespace LittleKingdom
{
    [Serializable]
    public class Interaction
    {
        [SerializeField] public InteractionType Type;
        [SerializeField] public Constraints.Constraints Constraints = new();
        [SerializeField] public UnityEvent Event;

        [SerializeField]
        public enum InteractionType
        {
            //MouseTap,
            MouseDrag,
            MouseDown,
            MouseEnter,
            MouseExit,
            MouseOver,
            MouseUp
        }

        public virtual void OnInteraction()
        {
            if (Constraints.Validate())
            {
                Event?.Invoke();
            }
        }
    }
}
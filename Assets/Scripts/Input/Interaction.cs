using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using LittleKingdom.Constraints;

namespace LittleKingdom
{
    [Serializable]
    public class Interaction
    {
        [SerializeField] private InteractionType interactionType;
        [SerializeField] private UnityEvent unityEvent;
        [SerializeField] private IConstraint constraint;

        [SerializeField]
        public enum InteractionType
        {
            MouseTap,
            MouseDrag
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace LittleKingdom.Interactions
{
    [Serializable]
    [AddComponentMenu("LittleKingdom/Interaction")]
    public abstract class Interaction : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The current game state must be one of these to invoke the event. " +
                 "If it is both allowed and prohibited, then the event will not be invoked.")]
        private GameState allowedStates;

        [SerializeField]
        [Tooltip("The current game state must not be one of these to invoke the event. " +
                 "If it is both allowed and prohibited, then the event will not be invoked.")]
        private GameState prohibitedStates;

        [SerializeField]
        [Tooltip("This will be invoked when the interaction occurs and if it is valid (see allowed/prohibited states).")]
        private UnityEvent Event;

        private InteractionUtilities interactionUtilities;

        public Interaction() { }

        [Inject]
        public void ConstructBase(InteractionUtilities interactionUtilities) =>
            this.interactionUtilities = interactionUtilities;

        protected virtual void OnInteraction()
        {
            if (interactionUtilities.AreValidStates(allowedStates, prohibitedStates) is false)
                return;

            Event?.Invoke();
        }
    }
}
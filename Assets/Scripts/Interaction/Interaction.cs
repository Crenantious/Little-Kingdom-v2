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
        private GameState AllowedStates;

        [SerializeField]
        [Tooltip("The current game state must not be one of these to invoke the event. " +
                 "If it is both allowed and prohibited, then the event will not be invoked.")]
        private GameState ProhibitedStates;

        [SerializeField]
        [Tooltip("This will be invoked when the interaction occurs and if it is valid (see allowed/prohibited states).")]
        private UnityEvent Event;

        private IReferences references;

        public Interaction() { }

        [Inject]
        public void ConstructBase(IReferences references) =>
            this.references = references;

        protected virtual void OnInteraction()
        {
            if (IsGameStateAllowed() is false || IsGameStateProhibited())
                return;

            Event?.Invoke();
        }

        private bool IsGameStateAllowed() =>
            (references.GameState & AllowedStates) == references.GameState;

        private bool IsGameStateProhibited() =>
            (references.GameState & ProhibitedStates) == references.GameState;
    }
}
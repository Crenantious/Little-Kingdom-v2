using System;
using UnityEngine;
using UnityEngine.Events;

namespace LittleKingdom.Interaction
{
    [Serializable]
    public abstract class Interaction
    {
        [SerializeField] private GameState RequiredStates;
        [SerializeField] private GameState ProhibitedStates;
        [SerializeField] private UnityEvent Event;

        private readonly IReferences references;

        public Interaction(IReferences references) =>
            this.references = references;

        public virtual void OnInteraction()
        {
            if (IsGameStateRequired() is false || IsGameStateProhibited())
                return;

            Event?.Invoke();
        }

        private bool IsGameStateRequired() =>
            (references.GameState & RequiredStates) == references.GameState;

        private bool IsGameStateProhibited() =>
            (references.GameState & ProhibitedStates) == references.GameState;
    }
}
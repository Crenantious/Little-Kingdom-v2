using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace LittleKingdom.Interactions
{
    public class InteractionUtilities
    {
        private readonly IReferences references;

        public InteractionUtilities(IReferences references) =>
            this.references = references;

        public bool AreValidStates(GameState allowedStates, GameState prohibitedStates) =>
            IsGameStateAllowed(allowedStates) && IsGameStateProhibited(prohibitedStates) is false;

        private bool IsGameStateAllowed(GameState allowedStates) =>
            (references.GameState & allowedStates) == references.GameState;

        private bool IsGameStateProhibited(GameState prohibitedStates) =>
            (references.GameState & prohibitedStates) == references.GameState;
    }
}
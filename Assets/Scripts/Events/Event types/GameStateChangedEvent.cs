using System;
using UnityEngine;

namespace LittleKingdom.Events
{
    public class GameStateChangedEvent : Event<GameStateChangedEvent.EventData>
    {
        [Serializable]
        public record EventData(GameState ChangedTo, GameState ChangedFrom) : EventDataBase
        {
            [field: SerializeField] public GameState ChangedTo { get; private set; } = ChangedTo;
            [field: SerializeField] public GameState ChangedFrom { get; private set; } = ChangedFrom;
        }
    }
}
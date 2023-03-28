using System;
using UnityEngine;

namespace LittleKingdom.Events
{
    [Serializable]
    public record EventData { }

    [Serializable]
    public record DrawerTestingEventData(string DR) : EventData {
        [field: SerializeField] public string DR { get; private set; } = DR;
    }
}
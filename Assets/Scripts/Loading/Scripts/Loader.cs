using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [Serializable]
    // TODO: JR - convert to an interface
    public class Loader : MonoBehaviour
    {
        public virtual List<Loader> Dependencies { get; protected set; } = new();
        public virtual void Load() { }
        public virtual void Unload() { }
    }
}
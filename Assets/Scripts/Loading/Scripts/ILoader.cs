using System;
using System.Collections.Generic;

namespace LittleKingdom.Loading
{
    public interface ILoader
    {
        public List<Type> Dependencies { get; }
        public void Load() { }
        public void Unload() { }
    }
}
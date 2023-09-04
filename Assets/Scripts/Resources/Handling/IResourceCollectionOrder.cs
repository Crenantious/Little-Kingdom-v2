using System;
using System.Collections.Generic;

namespace LittleKingdom.Resources
{
    /// <summary>
    /// Configures the order resource collection should occur.
    /// </summary>
    public interface IResourceCollectionOrder
    {
        public IReadOnlyList<Type> Producers { get; }
        public IReadOnlyList<Type> Halters { get; }
        public IReadOnlyList<Type> Movers { get; }
    }
}
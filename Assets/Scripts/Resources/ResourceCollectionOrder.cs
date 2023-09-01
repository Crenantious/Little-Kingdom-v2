using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Resources
{
    public class ResourceCollectionOrder : ScriptableObject
    {
        [SerializeField] private List<Type> producers = new();
        [SerializeField] private List<Type> movers = new();
        [SerializeField] private List<Type> halters = new();

        public IReadOnlyList<Type> Producers { get; private set; }
        public IReadOnlyList<Type> Movers { get; private set; }
        public IReadOnlyList<Type> Halters { get; private set; }

        private void Awake()
        {
            Producers = producers.AsReadOnly();
            Movers = movers.AsReadOnly();
            Halters = halters.AsReadOnly();
        }
    }
}
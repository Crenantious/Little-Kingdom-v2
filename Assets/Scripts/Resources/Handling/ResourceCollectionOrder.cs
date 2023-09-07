using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleKingdom.Resources
{
    [CreateAssetMenu(menuName = "Game/Resources/Resource collection order", fileName = "Resource collection order")]
    public class ResourceCollectionOrder : ScriptableObject, IResourceCollectionOrder, ISerializationCallbackReceiver
    {
        [SerializeField] private List<string> producers = new();
        [SerializeField] private List<string> halters = new();
        [SerializeField] private List<string> movers = new();

        public IReadOnlyList<Type> Producers { get; private set; }
        public IReadOnlyList<Type> Halters { get; private set; }
        public IReadOnlyList<Type> Movers { get; private set; }

        public IReadOnlyList<Type> GetOrderFor<T>() where T : IHandleResources =>
            typeof(T) switch
            {
                IProduceResources => Producers,
                IHaltResources => Halters,
                IMoveResources => Movers,
                _ => throw new NotImplementedException()
            };

        public void OnAfterDeserialize()
        {
            Producers = producers.Select(h => Type.GetType(h)).ToList().AsReadOnly();
            Halters = halters.Select(h => Type.GetType(h)).ToList().AsReadOnly();
            Movers = movers.Select(h => Type.GetType(h)).ToList().AsReadOnly();
        }

        public void OnBeforeSerialize() { }
    }
}
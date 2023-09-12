using System;
using System.Collections.Generic;
using Zenject;

namespace LittleKingdom.Loading
{
    [Serializable]
    public abstract class Loader<TConfig> : ILoader
        where TConfig : LoaderConfig
    {
        private TConfig config;

        public List<Type> Dependencies { get; protected set; } = new();

        [Inject]
        public void Construct(TConfig config) =>
            this.config = config;

        public void Load() => Load(config);

        public abstract void Load(TConfig config);

        public void AddDependency<T>() where T : ILoader =>
            Dependencies.Add(typeof(T));
    }
}
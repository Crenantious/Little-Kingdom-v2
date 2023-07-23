using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Loading
{
    public interface ILoader
    {
        public List<ILoader> Dependencies { get; }
        public void Load();
        public void Unload();
    }

    public abstract class Loader<TConfig> : ILoader
        where TConfig : LoaderConfig
    {
        [SerializeField] private List<ILoader> dependencies;
        public List<ILoader> Dependencies => dependencies;

        public void Load() =>
            Load(LoaderProfiles.Current.GetConfig<TConfig>());

        public abstract void Load(TConfig config);

        public abstract void Unload();
    }
}
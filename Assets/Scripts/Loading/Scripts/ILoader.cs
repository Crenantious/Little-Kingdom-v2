using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [Serializable]
    public abstract class Loader : MonoBehaviour
    {
        public abstract List<Loader> Dependencies { get; }
        public abstract void Load();
        public abstract void Unload();
    }

    [Serializable]
    public abstract class Loader<TConfig> : Loader
        where TConfig : LoaderConfig
    {
        public override void Load() =>
            Load(LoaderProfiles.Current.GetConfig<TConfig>());

        public abstract void Load(TConfig config);
    }
}
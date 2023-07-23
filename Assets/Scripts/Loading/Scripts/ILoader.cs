using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [Serializable]
    public class Loader : MonoBehaviour
    {
        public virtual List<Loader> Dependencies { get; }
        public virtual void Load() { }
        public virtual void Unload() { }
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
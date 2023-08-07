using System;
using Zenject;

namespace LittleKingdom.Loading
{
    [Serializable]
    public abstract class Loader<TConfig> : Loader
        where TConfig : LoaderConfig
    {
        private TConfig config;

        [Inject]
        public void Construct(TConfig config) => this.config = config;

        public override void Load() => Load(config);

        public abstract void Load(TConfig config);
    }
}
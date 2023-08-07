using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [Serializable]
    public class LoaderProfile : ScriptableObject, ISerializationCallbackReceiver
    {
        // TODO: JR - make this populated from the LoaderProfilesEditor via reflection.
        // If so, should probably add config categories such that only certain ones need to be loaded at a time.
        [SerializeField] private List<LoaderConfigTypeAndInstance> configs = new();
        private Dictionary<Type, LoaderConfig> typeToConfig = new();

        public LoaderConfig GetConfig(Type TConfig) =>
            typeToConfig.ContainsKey(TConfig) ?
            typeToConfig[TConfig] :
            (LoaderConfig)CreateInstance(TConfig.Name);

        public TConfig GetConfig<TConfig>()
            where TConfig : LoaderConfig =>
            (TConfig)GetConfig(typeof(TConfig));

        // typeToConfigs does not change so nothing needs to be done here.
        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() =>
            UpdateConfigDict();

        public void SetConfigs(List<LoaderConfigTypeAndInstance> configs)
        {
            this.configs = configs;
            UpdateConfigDict();
        }

        private void UpdateConfigDict() =>
            typeToConfig = configs.ToDictionary(c => c.ConfigType, c => c.config);
    }
}
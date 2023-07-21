using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [Serializable]
    public class LoaderProfile : ScriptableObject
    {
        // TODO: JR - make this private and have it populated from the LoaderProfilesEditor via reflection.
        // If so, should probably add config categories such that only certain ones need to be loaded at a time.
        public List<LoaderConfigTypeAndInstance> configs = new();

        private Dictionary<Type, LoaderConfig> typeToConfig = new();

        private void Awake()
        {
#if !UNITY_EDITOR
            foreach (LoaderConfigTypeAndInstance configTypeAndInstance in configs)
            {
                typeToConfig.Add(configTypeAndInstance.ConfigType, configTypeAndInstance.config);
            }
#endif
        }

        public TConfig GetConfig<TConfig>() where TConfig : LoaderConfig =>
            (TConfig)GetConfig(typeof(TConfig));

        public LoaderConfig GetConfig(Type TConfig) =>
            typeToConfig.ContainsKey(TConfig) ?
            typeToConfig[TConfig] :
            (LoaderConfig)CreateInstance(TConfig.Name);

        // typeToConfigs does not change so nothing needs to be done here.
        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() =>
            typeToConfig = configs.ToDictionary(c => c.ConfigType, c => c.config);
    }
}
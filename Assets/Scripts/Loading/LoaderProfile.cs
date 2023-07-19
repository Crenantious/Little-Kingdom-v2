using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [Serializable]
    public class LoaderProfile : ScriptableObject
    {
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
            (TConfig)typeToConfig[typeof(TConfig)];

        public LoaderConfig GetConfig(Type TLoader) =>
            typeToConfig[TLoader];

        public void OnBeforeSerialize()
        {
            // typeToConfigs does not change so nothing needs to be done here.
        }

        public void OnAfterDeserialize() =>
            typeToConfig = configs.ToDictionary(c => c.ConfigType, c => c.config);
    }
}
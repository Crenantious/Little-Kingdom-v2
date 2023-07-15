using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [Serializable]
    public class LoaderProfile : ScriptableObject
    {
        private readonly Dictionary<Type, LoaderConfig> typeToConfig = new();

        [SerializeField] private List<LoaderConfigTypeAndInstance> configTypesAndInstances = new();

        private void Awake()
        {
            foreach (LoaderConfigTypeAndInstance configTypeAndInstance in configTypesAndInstances)
            {
                typeToConfig.Add(configTypeAndInstance.Type, configTypeAndInstance.Config);
            }
        }

        public TConfig GetConfig<TConfig>() where TConfig : LoaderConfig =>
            (TConfig)typeToConfig[typeof(TConfig)];

        public LoaderConfig GetConfig(Type TLoader) =>
            typeToConfig[TLoader];


        //public void OnBeforeSerialize()
        //{
        //    for (int i = 0; i < loaderAndConfigs.Count; i++)
        //    {
        //        loaderTypeToConfig.Add(loaderAndConfigs[i], configs[i]);
        //    }
        //}

        //public void OnAfterDeserialize()
        //{
        //    // typeToConfigs does not change so nothing needs to be done here.
        //}

        //public void AddConfig<TLoader>(LoaderConfig config) where TLoader : ILoader
        //{
        //    loaderAndConfigs.Add(typeof(TLoader));
        //    configs.Add(config);
        //}
    }
}
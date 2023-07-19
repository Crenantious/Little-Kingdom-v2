using System;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [Serializable]
    public class LoaderConfigTypeAndInstance : ISerializationCallbackReceiver
    {
        private const string nullTypeName = "null";

        [SerializeField] public string configTypeName = nullTypeName;
        [SerializeField] public LoaderConfig config;

        public Type ConfigType { get; set; } = null;

        public void OnBeforeSerialize() =>
            configTypeName = ConfigType is null ?
                nullTypeName :
                ConfigType.Name;

        public void OnAfterDeserialize() =>
            ConfigType = configTypeName is nullTypeName ?
                null :
                Type.GetType(configTypeName);
    }
}
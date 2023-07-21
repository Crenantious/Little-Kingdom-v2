using System;
using UnityEngine;

namespace LittleKingdom.Loading
{
    [Serializable]
    public class LoaderConfigTypeAndInstance : ISerializationCallbackReceiver
    {
        private const string nullTypeName = "null";
        private const string configTypeAssemblyQualifiedName = "LittleKingdom.Loading.{0}, Scripts";

        [SerializeField] public string configTypeName = nullTypeName;
        [SerializeField] public LoaderConfig config;

        public Type ConfigType { get; set; } = null;

        public LoaderConfigTypeAndInstance(Type configType, LoaderConfig config = null)
        {
            ConfigType = configType;
            configTypeName = configType.Name;
            this.config = config;
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            ConfigType = configTypeName is nullTypeName ?
                null :
                Type.GetType(configTypeAssemblyQualifiedName.FormatConst(configTypeName));
        }
    }
}
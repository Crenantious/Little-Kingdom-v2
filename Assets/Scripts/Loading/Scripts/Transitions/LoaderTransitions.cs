using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Loading
{
    public class LoaderTransitions : MonoBehaviour, ISerializationCallbackReceiver
    {
        private static Loading loading;

        private readonly Dictionary<Type, Action> loaderToTransition = new()
        {
            { typeof(GameSetupLoader), GameSetupToBoard },
            { typeof(BoardLoader), BoardToTown },
        };

        [SerializeField] private string initialLoaderTypeName;
        private Type initialLoaderType;

        [Inject]
        public void Construct(Loading loading)
        {
            loading.TopLevelLoaded += OnLoaded;
            LoaderTransitions.loading = loading;
        }

        private void Start()
        {
            if (initialLoaderType is null)
                Debug.LogError("Must set the initial loader.", this);
            else
                loading.Load(initialLoaderType);
        }

        private static void GameSetupToBoard() =>
            loading.Load<BoardLoader>();

        private static void BoardToTown() =>
            loading.Load<TownLoader>();

        private void OnLoaded(ILoader loader)
        {
            if (loaderToTransition.ContainsKey(loader.GetType()))
                loaderToTransition[loader.GetType()].Invoke();
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            Type loaderType = Type.GetType(Utilities.GetAssemblyQualifiedName("LittleKingdom.Loading", initialLoaderTypeName));
            if (loaderType is null)
                Debug.LogError("Invalid initial loader type set.", this);
            initialLoaderType = loaderType;
        }
    }
}
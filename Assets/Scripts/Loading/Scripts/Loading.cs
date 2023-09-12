using LittleKingdom.Factories;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LittleKingdom.Loading
{
    // TODO: JR - implement unloading.
    public class Loading
    {
        private readonly Dictionary<Type, ILoader> loaded = new();

        private LoaderFactory loaderFactory;

        /// <summary>
        /// Invoked when a top level (not a dependency) loader is loaded. It's the one passed into <see cref="Load(Type)"/>.
        /// </summary>
        public event SimpleEventHandler<ILoader> TopLevelLoaded;

        /// <summary>
        /// Invoked when a loader from <see cref="ILoader.Dependencies"/> is loaded.
        /// </summary>
        public event SimpleEventHandler<ILoader> DependencyLoaded;

        [Inject]
        public void Construct(LoaderFactory loaderFactory) =>
            this.loaderFactory = loaderFactory;

        /// <summary>
        /// Creates and loads the loader. Also loads all of its dependencies.<br/>
        /// </summary>
        public void Load<T>() where T : ILoader =>
            Load(typeof(T));

        /// <summary>
        /// Creates and loads the loader. Also loads all of its dependencies.<br/>
        /// </summary>
        /// <param name="loaderType">Must inherit <see cref="ILoader"/></param>
        public void Load(Type loaderType) =>
            Load(loaderType, TopLevelLoaded);

        private void LoadDependency(Type loaderType) =>
            Load(loaderType, DependencyLoaded);

        private void Load(Type loaderType, SimpleEventHandler<ILoader> e)
        {
            if (loaded.ContainsKey(loaderType))
            {
                Debug.LogWarning($"A loader with type {loaderType} is already loaded.");
                return;
            }

            ILoader loader = loaderFactory.Create(loaderType);

            LoadDependencies(loader);

            loader.Load();
            loaded.Add(loaderType, loader);

            e?.Invoke(loader);
        }

        private void LoadDependencies(ILoader loader)
        {
            foreach (Type dependency in loader.Dependencies)
            {
                if (loaded.ContainsKey(dependency) is false)
                    LoadDependency(dependency);
            }
        }
    }
}
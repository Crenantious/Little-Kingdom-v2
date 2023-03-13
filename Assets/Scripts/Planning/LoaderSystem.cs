//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace LoaderSystem
//{
//    public static class LoaderManager
//    {
//        private static readonly Dictionary<Type, ILoader> loaders;
//        private static readonly Dictionary<Type, List<Type>> loaderDependants;

//        public static void Load<LoaderType>() where LoaderType : ILoader
//        {
//            Load(LoaderFactory.CreateInstance<LoaderType>());
//        }
//        private static void Load<LoaderType>(LoaderType loader) where LoaderType : ILoader
//        {
//            if (!IsLoaded<LoaderType>())
//            {
//                LoadDependencies(loader);
//                loaders.Add(typeof(LoaderType), loader);
//            }
//        }

//        private static bool IsLoaded<LoaderType>() where LoaderType : ILoader
//        {
//            return loaders.ContainsKey(typeof(LoaderType));
//        }

//        public static void Unload<LoaderType>() where LoaderType : ILoader
//        {
//            loaders.Remove(typeof(LoaderType));
//        }

//        public static void Set<LoaderType>() where LoaderType : ILoader
//        {
//            // unload each first
//            loaders.Clear();
//            // loaders.Add(LoaderFactory.GetInstance<ILoader>());
//        }

//        private static void Load(ILoader loader)
//        {
//            LoadDependencies(loader);
//            loader.Load();
//        }

//        private static void LoadDependencies(ILoader loader)
//        {
//            foreach (ILoader dependency in loader.GetDependencies())
//            {
//                if (loaders.ContainsKey(dependency.GetType()) is false)
//                {
//                    Load(dependency);
//                }
//            }
//        }
//    }

//    public static class LoaderFactory
//    {
//        public static LoaderType CreateInstance<LoaderType>() =>
//            (LoaderType)Activator.CreateInstance(typeof(LoaderType));
//    }
//    public interface ILoader

//    {
//        public IEnumerable<ILoader> GetDependencies();
//        public bool Load();
//        public bool UnLoad();

//    }

//    public class TestLoader : ILoader
//    {
//    }

//    public enum GameStateType
//    {
//        Trading,
//    }

//    public struct GameState<TLoader> where TLoader : LoaderSystem.ILoader
//    {
//        public static readonly GameState<LoaderSystem.TestLoader> TRADING = new(GameStateType.Trading);

//        public GameStateType Type { get; private set; }
//        public Type LoaderType { get; private set; }

//        private GameState(GameStateType type)
//        {
//            Type = type;
//            LoaderType = typeof(TLoader);
//        }

//    }

//    public interface IGameState
//    {
//        public ILoader loader { get; }
//    }

//    public class TradingGameState : IGameState
//    {
//        public ILoader loader => throw new NotImplementedException();
//    }
//}




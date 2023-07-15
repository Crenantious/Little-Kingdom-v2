using System.Collections.Generic;

namespace LittleKingdom.Loading
{
    public class Loading
    {
        private static readonly HashSet<ILoader> loaded = new();

        public static void Load(ILoader loader)
        {
            if (loaded.Contains(loader))
                return;

            LoadDependencies(loader);

            loader.Load();
            loaded.Add(loader);
        }

        private static void LoadDependencies(ILoader loader)
        {
            foreach (ILoader dependency in loader.Dependencies)
            {
                Load(dependency);
            }
        }
    }
}
using System.Collections.Generic;

namespace LittleKingdom.Loading
{
    public class Loading
    {
        private static readonly HashSet<Loader> loaded = new();

        public static void Load(Loader loader)
        {
            if (loaded.Contains(loader))
                return;

            LoadDependencies(loader);

            loader.Load();
            loaded.Add(loader);
        }

        private static void LoadDependencies(Loader loader)
        {
            foreach (Loader dependency in loader.Dependencies)
            {
                Load(dependency);
            }
        }
    }
}
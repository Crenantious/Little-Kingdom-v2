namespace LittleKingdom.Loading
{
    public class Loading
    {
        public static void Load(ILoader loader)
        {
            LoadDependencies(loader);
            loader.Load();
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
using LittleKingdom.Loading;
using System;
using Zenject;

namespace LittleKingdom.Factories
{
    public class LoaderFactory : PlaceholderFactory<Type, ILoader>
    {
        private readonly DiContainer container;

        public LoaderFactory(DiContainer container) =>
            this.container = container;

        public override ILoader Create(Type loaderType)
        {
            ILoader loader = (ILoader)Activator.CreateInstance(loaderType);
            container.Inject(loader);
            return loader;
        }
    }
}
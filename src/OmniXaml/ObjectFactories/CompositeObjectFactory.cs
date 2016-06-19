namespace OmniXaml.ObjectFactories
{
    using System;
    using System.Linq;

    public class CompositeObjectFactory : IObjectFactory
    {
        private readonly IObjectFactory[] factories;

        public CompositeObjectFactory(params IObjectFactory[] factories)
        {
            this.factories = factories;
        }

        public object Create(Type type, params InjectableValue[] injectableValues)
        {
            return factories
                .Select(factory => factory.Create(type, injectableValues))
                .FirstOrDefault(o => o != null);
        }
    }
}
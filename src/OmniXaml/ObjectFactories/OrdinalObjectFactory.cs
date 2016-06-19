namespace OmniXaml.Testing.Common
{
    using System;
    using System.Linq;
    using ObjectFactories;

    public class OrdinalObjectFactory : IObjectFactory
    {
        public object Create(Type type, params InjectableValue[] injectableValues)
        {
            var parameters = injectableValues.Select(i => i.Value).ToArray();
            try
            {
                return Activator.CreateInstance(type, parameters);
            }
            catch (MissingMethodException)
            {
                return null;
            }
        }
    }
}
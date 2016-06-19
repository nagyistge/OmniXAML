namespace OmniXaml.ObjectFactories
{
    using System;

    public interface IObjectFactory
    {
        object Create(Type type, params InjectableValue[] injectableValues);
    }
}
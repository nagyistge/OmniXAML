namespace OmniXaml
{
    using System;

    public interface ITypeFactory
    {

        object Create(Type type, params object[] args);
        bool CanCreate(Type type, params object[] args);
    }
}
namespace OmniXaml.Services
{
    using System;
    using ObjectFactories;

    public class TypeFactoryRegistration
    {
        public IObjectFactory Factory { get; }
        public Func<Type, bool> IsApplicable { get; }

        public TypeFactoryRegistration(IObjectFactory factory, Func<Type, bool> isApplicable)
        {
            this.Factory = factory;
            this.IsApplicable = isApplicable;
        }
    }
}
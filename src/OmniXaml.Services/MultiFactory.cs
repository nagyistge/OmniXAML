namespace OmniXaml.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ObjectFactories;

    public class MultiFactory: IObjectFactory
    {
        private readonly IEnumerable<TypeFactoryRegistration> factoryRegistrations;

        public MultiFactory(IEnumerable<TypeFactoryRegistration> factoryRegistrations)
        {
            this.factoryRegistrations = factoryRegistrations;                
        }

        public object Create(Type type, params InjectableValue[] args)
        {
            return GetFactory(type).Create(type, args);
        }

        private IObjectFactory GetFactory(Type type, params object[] args)
        {
            var factory = (from reg in factoryRegistrations
                where reg.IsApplicable(type)
                select reg.Factory).FirstOrDefault();

            return factory;
        }

        public bool CanCreate(Type type, params object[] args)
        {
            return GetFactory(type, args) != null;
        }
    }
}
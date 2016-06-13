namespace OmniXaml.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MultiFactory: ITypeFactory
    {
        private readonly IEnumerable<TypeFactoryRegistration> factoryRegistrations;

        public MultiFactory(IEnumerable<TypeFactoryRegistration> factoryRegistrations)
        {
            this.factoryRegistrations = factoryRegistrations;                
        }

        public object Create(Type type, params object[] args)
        {
            return GetFactory(type).Create(type, args);
        }

        private ITypeFactory GetFactory(Type type, params object[] args)
        {
            var factory = (from reg in factoryRegistrations
                where reg.IsApplicable(type) && reg.Factory.CanCreate(type, args)
                select reg.Factory).FirstOrDefault();

            return factory;
        }

        public bool CanCreate(Type type, params object[] args)
        {
            return GetFactory(type, args) != null;
        }
    }
}
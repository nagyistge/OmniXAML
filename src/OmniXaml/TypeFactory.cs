namespace OmniXaml
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class TypeFactory : ITypeFactory
    {
        public object Create(Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args);
        }

        public bool CanCreate(Type type, params object[] args)
        {
            var typesOfEachCtor = type
                .GetTypeInfo()
                .DeclaredConstructors
                .Select(info => info.GetParameters().Select(parameterInfo => parameterInfo.ParameterType));
                

            var argsTypes = args
                .Select(o => o.GetType());

            var zips = typesOfEachCtor
                .Select(
                types => argsTypes
                    .DefaultIfEmpty()
                    .Zip(types, (type1, type2) => new {type1, type2}));

            return zips.Any(enumerable => enumerable.All(arg => arg.type1 == arg.type2));
        }
    }
}
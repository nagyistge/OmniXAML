namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class TypeFactory : ITypeFactory
    {
        public object Create(Type type, params object[] args)
        {
            try
            {
                return Activator.CreateInstance(type, args);
            }
            catch (MissingMethodException)
            {
                return CreateFitToArgs(type, args);                
            }            
        }

        private object CreateFitToArgs(Type type, object[] args)
        {
            var ctorsWithSameNumberOfArgs = type
                .GetTypeInfo()
                .DeclaredConstructors
                .Where(info => info.GetParameters().Length == args.Length);

            var results = ctorsWithSameNumberOfArgs.Select(info => Fit(info, args));
            return results.FirstOrDefault(o => o != null);
        }

        private static object Fit(ConstructorInfo info, object[] args)
        {
            var type = info.DeclaringType;
            
            try
            {
                var arguments = GetSortedArguments(info, args).ToArray();
                return Activator.CreateInstance(type, arguments);
            }
            catch
            {
                return null;
            }            
        }

        private static IEnumerable<object> GetSortedArguments(MethodBase ctor, object[] args)
        {
            return ctor.GetParameters()
                .Select(
                    parameterInfo => args
                        .Single(o => parameterInfo.ParameterType.GetTypeInfo().IsAssignableFrom(o.GetType())));
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
namespace OmniXaml.ObjectFactories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class NameMatchingObjectFactory : IObjectFactory
    {
        public object Create(Type type, params InjectableValue[] injectableValues)
        {
            var ctorsWithSameArgCount = type
                .GetTypeInfo()
                .DeclaredConstructors
                .Where(info => info.GetParameters().Length == injectableValues.Length);

            var results = ctorsWithSameArgCount.Select(ctor => Create(ctor, injectableValues));
            return results.FirstOrDefault(o => o != null);
        }

        private static object Create(MethodBase creationMethod, InjectableValue[] args)
        {
            var type = creationMethod.DeclaringType;

            try
            {
                var sortedParameters = SortInjectables(creationMethod, args)
                    .Select(i => i.Value)
                    .ToArray();

                return Activator.CreateInstance(type, sortedParameters);
            }
            catch
            {
                return null;
            }
        }

        private static IEnumerable<InjectableValue> SortInjectables(MethodBase ctor, InjectableValue[] args)
        {
            return ctor.GetParameters()
                .Select(
                    parameterInfo => args
                        .Single(injectableValue => SameName(parameterInfo, injectableValue)));
        }

        private static bool SameName(ParameterInfo parameterInfo, InjectableValue o)
        {
            return string.Equals(parameterInfo.Name, o.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
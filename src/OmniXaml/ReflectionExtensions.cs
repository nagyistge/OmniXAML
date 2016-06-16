namespace OmniXaml
{
    using System;
    using System.Collections;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static IEnumerable AsImmutable(this IEnumerable collection, Type elementType)
        {
            var immutableListTypeInfo = typeof(ImmutableList<>).MakeGenericType(elementType).GetTypeInfo();

            var addRangeMethod = immutableListTypeInfo.GetMethod("AddRange");
            var typedCollection = ToTyped(collection, elementType);

            var emptyImmutableList = immutableListTypeInfo.GetField("Empty").GetValue(null);
            emptyImmutableList = addRangeMethod.Invoke(emptyImmutableList, new[] { typedCollection });
            return (IEnumerable)emptyImmutableList;
        }

        private static object ToTyped(IEnumerable original, Type type)
        {
            var method = typeof(Enumerable).GetTypeInfo().GetMethod("Cast", BindingFlags.Public | BindingFlags.Static).MakeGenericMethod(type);
            return method.Invoke(original, new object[] { original });
        }
    }
}
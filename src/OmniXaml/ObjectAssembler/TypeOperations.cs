namespace OmniXaml.ObjectAssembler
{
    using System.Collections;
    using ObjectFactories;

    public class TypeOperations
    {
        private readonly IObjectFactory objectFactory;

        public TypeOperations(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public static void AddToCollection(ICollection collection, object instance)
        {
            ((IList)collection).Add(instance);
        }

        public static void AddToDictionary(IDictionary collection, object key, object value)
        {
            collection.Add(key, value);
        }
    }
}
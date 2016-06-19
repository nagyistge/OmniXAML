namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Builder;
    using Glass.Core;
    using ObjectFactories;
    using TypeConversion;
    using Typing;

    public class RuntimeTypeSource : IRuntimeTypeSource
    {
        private ITypeFeatureProvider featureProvider;
        public INamespaceRegistry NamespaceRegistry { get; }

        public RuntimeTypeSource(ITypeRepository typeRepository, INamespaceRegistry nsRegistry)
        {
            TypeRepository = typeRepository;
            NamespaceRegistry = nsRegistry;
        }

        public Namespace GetNamespace(string name)
        {
            return NamespaceRegistry.GetNamespace(name);
        }

        public Namespace GetNamespaceByPrefix(string prefix)
        {
            return NamespaceRegistry.GetNamespaceByPrefix(prefix);
        }

        public void RegisterPrefix(PrefixRegistration prefixRegistration)
        {
            NamespaceRegistry.RegisterPrefix(prefixRegistration);
        }

        public void AddNamespace(XamlNamespace xamlNamespace)
        {
            NamespaceRegistry.AddNamespace(xamlNamespace);
        }

        public XamlType GetByType(Type type)
        {
            return TypeRepository.GetByType(type);
        }

        public XamlType GetByQualifiedName(string qualifiedName)
        {

            return TypeRepository.GetByQualifiedName(qualifiedName);
        }

        public XamlType GetByPrefix(string prefix, string typeName)
        {
            return TypeRepository.GetByPrefix(prefix, typeName);
        }

        public XamlType GetByFullAddress(XamlTypeName xamlTypeName)
        {
            return TypeRepository.GetByFullAddress(xamlTypeName);
        }

        public Member GetMember(PropertyInfo propertyInfo)
        {
            return TypeRepository.GetMember(propertyInfo);
        }

        public AttachableMember GetAttachableMember(string name, MethodInfo getter, MethodInfo setter)
        {
            return TypeRepository.GetAttachableMember(name, getter, setter);
        }      

        public IEnumerable<PrefixRegistration> RegisteredPrefixes => NamespaceRegistry.RegisteredPrefixes;

        public ITypeRepository TypeRepository { get; }

        public static IRuntimeTypeSource FromAttributes(IEnumerable<Assembly> assemblies)
        {
            var allExportedTypes = assemblies.AllExportedTypes();

            var typeFactory = new CompositeObjectFactory();

            var xamlNamespaceRegistry = new NamespaceRegistry();
            xamlNamespaceRegistry.FillFromAttributes(assemblies);

            var typeFeatureProvider = new TypeFeatureProvider(new TypeConverterProvider());
            typeFeatureProvider.FillFromAttributes(allExportedTypes);
                
            var xamlTypeRepo = new TypeRepository(xamlNamespaceRegistry, typeFactory, typeFeatureProvider);

            return new RuntimeTypeSource(xamlTypeRepo, xamlNamespaceRegistry);
        }

        public ITypeConverter GetTypeConverter(Type type)
        {
            return featureProvider.GetTypeConverter(type);
        }

        public string GetContentPropertyName(Type type)
        {
            return featureProvider.GetContentPropertyName(type);
        }

        public IEnumerable<TypeConverterRegistration> TypeConverters => featureProvider.TypeConverters;
        public Metadata GetMetadata(Type type)
        {
            return featureProvider.GetMetadata(type);
        }

        public void RegisterMetadata(Type type, Metadata metadata)
        {
            featureProvider.RegisterMetadata(type, metadata);
        }
    }
}
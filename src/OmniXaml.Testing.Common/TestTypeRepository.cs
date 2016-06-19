namespace OmniXaml.Testing.Common
{
    using System;
    using System.Collections.Generic;
    using ObjectFactories;
    using Typing;

    internal class TestTypeRepository : TypeRepository
    {
        readonly ISet<Type> nameScopes = new HashSet<Type>();

        public TestTypeRepository(INamespaceRegistry namespaceRegistry, IObjectFactory objectObjectFactory, ITypeFeatureProvider featureProvider)
            : base(namespaceRegistry, objectObjectFactory, featureProvider)
        {
        }

        public override XamlType GetByType(Type type)
        {
            var isNameScope = nameScopes.Contains(type);
            return new XamlTypeMock(type, this, ObjectFactory, FeatureProvider) { IsNameScope = isNameScope };
        }

        public void ClearNameScopes()
        {           
            nameScopes.Clear();
        }

        public void EnableNameScope(Type type)
        {
            nameScopes.Add(type);
        }
    }
}
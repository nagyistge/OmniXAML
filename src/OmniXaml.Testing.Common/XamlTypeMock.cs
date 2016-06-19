namespace OmniXaml.Testing.Common
{
    using System;
    using ObjectFactories;
    using Typing;

    internal class XamlTypeMock : XamlType
    {
        public XamlTypeMock(Type type, ITypeRepository typeRepository, IObjectFactory objectObjectFactory, ITypeFeatureProvider featureProvider)
            : base(type, typeRepository, objectObjectFactory, featureProvider)
        {
        }

        public bool IsNameScope { get; set; }


        public override INameScope GetNamescope(object instance)
        {
            if (IsNameScope)
            {
                return instance as INameScope;
            }

            return null;
        }
    }
}
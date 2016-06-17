namespace OmniXaml.Pure
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using Glass.Core;
    using TypeConversion;
    using Typing;

    internal class Workshop
    {
        private readonly IValueContext valueContext;
        private readonly Action<object> setResult;
        private readonly StackingLinkedList<Workbench> workbenches = new StackingLinkedList<Workbench>();

        private Workbench Current => workbenches.CurrentValue;
        public Workbench Previous => workbenches.PreviousValue;

        public Workshop(IValueContext valueContext, Action<object> setResult)
        {
            this.valueContext = valueContext;
            this.setResult = setResult;
        }

        public void CreateInstanceIfNotYetCreated()
        {
            if (Current.Instance == null)
            {
                Current.Instance = Current.XamlType.CreateInstance(null);
            }
        }

        public void StartObjectOftype(XamlType xamlType)
        {
            var workbench = new Workbench(valueContext) { XamlType = xamlType };
            workbenches.Push(workbench);
        }

        public void StartDirective(Directive directive)
        {
            workbenches.Push(new Workbench(valueContext));
            Current.Member = directive;
        }

        public void StartMember(MemberBase member)
        {
            CreateInstanceIfNotYetCreated();
            Current.Member = member;
        }

        public void SetValue(object value)
        {
            var item = new Workbench(valueContext) { Instance = value };
            workbenches.Push(item);
        }

        public void EndObject()
        {
            CreateInstanceIfNotYetCreated();

            if (workbenches.Previous != null)
            {
                if (Equals(Previous.Member, CoreTypes.Items))
                {
                    Previous.BufferedChildren.Add(Current.Instance);
                    workbenches.Pop();
                }
            }

            setResult(Current.Instance);
        }

        public void EndMember()
        {
            if (!Current.Flag)
            {
                object instanceToAssign;
                if (Equals(Current.Member, CoreTypes.Items))
                {
                    instanceToAssign = CreateCollection();
                }
                else
                {
                    instanceToAssign = Current.Instance;
                }

                if (Equals(Current.Member, CoreTypes.Items))
                {
                    Previous.Flag = true;
                }

                workbenches.Pop();

                AssignCompatibleValue(instanceToAssign);
            }
        }

        private void AssignCompatibleValue(object instanceToAssign)
        {
            object converted;
            if (CommonValueConversion.TryConvert(instanceToAssign, Current.Member.XamlType, valueContext, out converted))
            {
                Current.SetMemberValue(converted);
            }
            else
            {
                Current.SetMemberValue(instanceToAssign);
            }
        }

        private IList CreateCollection()
        {
            var collectionType = Previous.Member.XamlType;

            if (!IsImmutable(collectionType))
            {
                var collection = (IList)collectionType.CreateInstance();

                foreach (var bufferedChild in Current.BufferedChildren)
                {
                    collection.Add(bufferedChild);
                }

                return collection;
            }

            var underlyingType = Previous.Member.XamlType.UnderlyingType.GetTypeInfo().GetGenericArguments().First();
            return (IList)Current.BufferedChildren.AsImmutable(underlyingType);
        }

        private static bool IsImmutable(XamlType collectionType)
        {
            var underlyingType = collectionType.UnderlyingType;
            if (underlyingType == null)
                return false;

            var ti = underlyingType.GetTypeInfo();
            if (!ti.IsGenericType || ti.Assembly.GetName().Name != "System.Collections.Immutable")
                return false;

            var typeDef = ti.GetGenericTypeDefinition();
            var name = typeDef.FullName;
            if (!name.StartsWith("System.Collections.Immutable.Immutable", StringComparison.Ordinal))
                return false;

            return name.EndsWith("`1", StringComparison.Ordinal) || name.EndsWith("`2", StringComparison.Ordinal);
        }
    }
}
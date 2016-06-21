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
        private readonly Action<object> setResult;
        private readonly IValueContext valueContext;
        private readonly StackingLinkedList<Workbench> workbenches = new StackingLinkedList<Workbench>();

        public Workshop(IValueContext valueContext, Action<object> setResult)
        {
            this.valueContext = valueContext;
            this.setResult = setResult;
        }

        private Workbench Current => workbenches.CurrentValue;
        private Workbench Previous => workbenches.PreviousValue;

        public void StartObjectOftype(XamlType xamlType)
        {
            var workbench = new Workbench(valueContext) { XamlType = xamlType };
            workbenches.Push(workbench);
        }

        public void EndObject()
        {
            CreateInstanceIfNotYetCreated();

            if (IsParentCollectingChildren)
            {
                AddChildAndCollapse();
            }

            setResult(Current.Instance);
        }

        private void AddChildAndCollapse()
        {
            Previous.BufferedChildren.Add(Current.Instance);
            Collapse();
        }

        public bool IsParentCollectingChildren => workbenches.Previous != null && Equals(Previous?.Member, CoreTypes.Items);

        public void StartMember(MemberBase member)
        {
            Current.Member = member;
        }

        public void EndMember()
        {
            if (IsDirective)
            {
                EndDirective();
            }
            else
            {
                RegularEndMember();
            }
        }

        public bool IsDirective => Current.Member != null && Current.Member.IsDirective && !Current.IsDirectiveProcessed;

        private void EndDirective()
        {
            if (Current.Member.Equals(CoreTypes.Items))
            {
                var collection = CreateCollection(Previous.Member.XamlType, Current.BufferedChildren);
                Current.Instance = collection;
                Current.IsDirectiveProcessed = true;
            }
        }

        private void Collapse()
        {
            workbenches.Pop();
        }

        private void RegularEndMember()
        {
            StoreAssignment();
            workbenches.Pop();
        }

        public void StartDirective(Directive directive)
        {
            workbenches.Push(new Workbench(valueContext));
            Current.Member = directive;
        }

        public void SetValue(object value)
        {
            var item = new Workbench(valueContext) { Instance = value };
            workbenches.Push(item);
        }

        private void CreateInstanceIfNotYetCreated()
        {
            if (Current.Instance == null)
            {
                CreateInstance();
            }
        }

        private void CreateInstance()
        {
            var directAssignments = Current.MemberAssignments.Where(pair => pair.Key.IsWritable);
            var instance = Current.XamlType.CreateInstance();
            foreach (var directAssignment in directAssignments)
            {
                var member = directAssignment.Key;
                var value = directAssignment.Value;
                member.SetValue(instance, value, valueContext);
            }

            Current.Instance = instance;
        }

        private void StoreAssignment()
        {
            var memberToBeAssigned = Previous.Member;
            var instanceToAssign = Current.Instance;

            object converted;
            var finalValue = CommonValueConversion.TryConvert(instanceToAssign, memberToBeAssigned.XamlType, valueContext, out converted)
                ? converted
                : instanceToAssign;

            Previous.MemberAssignments.Add((MutableMember)memberToBeAssigned, finalValue);
        }

        private IList CreateCollection(XamlType collectionType, ICollection children)
        {
            if (!IsImmutable(collectionType))
            {
                var collection = (IList)collectionType.CreateInstance();

                foreach (var bufferedChild in children)
                {
                    collection.Add(bufferedChild);
                }

                return collection;
            }

            var underlyingType = Previous.Member.XamlType.UnderlyingType.GetTypeInfo().GetGenericArguments().First();
            return (IList)children.AsImmutable(underlyingType);
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
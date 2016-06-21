namespace OmniXaml.Pure
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using Glass.Core;
    using TypeConversion;
    using Typing;

    internal class Workshop : IWorkshop
    {
        private readonly Action<object> setResult;
        private readonly IValueContext valueContext;
        public readonly StackingLinkedList<Workbench> workbenches = new StackingLinkedList<Workbench>();

        public Workshop(IValueContext valueContext, Action<object> setResult)
        {
            this.valueContext = valueContext;
            this.setResult = setResult;
        }

        public Workbench Current => workbenches.CurrentValue;
        public Workbench Previous => workbenches.PreviousValue;



        public bool IsDirective => Current.Member != null && Current.Member.IsDirective && !Current.IsDirectiveProcessed;

        public bool IsRegularMemberValue => !Current.Member.Equals(CoreTypes.Initialization);

        public StackingLinkedList<Workbench> Workbenches => workbenches;

        public void StartObject(XamlType xamlType)
        {
            var workbench = new Workbench(valueContext) { XamlType = xamlType };
            workbenches.Push(workbench);
        }

        public void EndObject()
        {
            CreateInstanceIfNotYetCreated();

            setResult(Current.Instance);
        }

        public void StartMember(MemberBase member)
        {
            Current.Member = member;
        }

        public void EndMember()
        {
            if (Current.IsDirectiveProcessed)
            {
                return;
            }

            StoreAssignment();
            Collapse();

            Current.IsDirectiveProcessed = true;
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

        public void Collapse()
        {
            workbenches.Pop();
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
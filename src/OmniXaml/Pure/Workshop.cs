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
        private readonly StackingLinkedList<Workbench> workbenches = new StackingLinkedList<Workbench>();

        public Workshop(IValueContext valueContext, Action<object> setResult)
        {
            this.valueContext = valueContext;
            this.setResult = setResult;
        }

        public Workbench Current => workbenches.CurrentValue;
        public Workbench Previous => workbenches.PreviousValue;

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
            AssignCurrentInstanceToPreviousMember();
            Collapse();
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
            CreateInstance();
        }

        private void CreateInstance()
        {
            var groupedAssignments = Current.MemberAssignments.GroupBy(pair => pair.Key.IsWritable).ToList();
            var writable = groupedAssignments.Where(pairs => pairs.Key).SelectMany(p => p);
            var nonWritable = groupedAssignments.Where(pairs => !pairs.Key).SelectMany(p => p);

            var instance = Current.XamlType.CreateInstance(nonWritable.Select(pair => new InjectableValue(pair.Key.Name, pair.Value)).ToArray());
            foreach (var directAssignment in writable)
            {
                var member = directAssignment.Key;
                var value = directAssignment.Value;
                member.SetValue(instance, value, valueContext);
            }

            Current.Instance = instance;
        }

        private void AssignCurrentInstanceToPreviousMember()
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

        public void Bump()
        {
            workbenches.Push(new Workbench(valueContext));
        }
    }
}
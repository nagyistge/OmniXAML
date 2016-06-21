namespace OmniXaml.Pure
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using Glass.Core;
    using Typing;

    internal class WorkshopProxy : IWorkshop
    {
        private readonly Workshop workshop;

        private StackingLinkedList<Workbench> Workbenches => workshop.Workbenches;

        public Workbench Previous => Workbenches.PreviousValue;
        public Workbench Current => Workbenches.CurrentValue;

        public bool CheckIsDirective(Directive directive)
        {
            return Workbenches.Previous != null && Previous.Member != null && Previous.Member.Equals(directive);
        }

        public void Collapse()
        {
            workshop.Collapse();
        }

        public WorkshopProxy(Workshop workshop)
        {
            this.workshop = workshop;
        }

        public void StartObject(XamlType xamlType)
        {
            StartObjectDirective();
            workshop.StartObject(xamlType);
        }

        public void SetValue(object value)
        {
            SetValueDirective();
            workshop.SetValue(value);
        }

        public void EndObject()
        {
            EndBeginEndObject();
            workshop.EndObject();
            OnFinishEndObject();
        }

        private void OnFinishEndObject()
        {
            if (CheckIsDirective(CoreTypes.Items))
            {
                var instance = Current.Instance;
                Collapse();
                Previous.BufferedChildren.Add(instance);
            }
        }

        public void EndMember()
        {
            BeginEndMember();
            workshop.EndMember();
        }

        public void StartMember(MemberBase member)
        {
            BeginStartMember();
            workshop.StartMember(member);
        }

        public void StartDirective(Directive directive)
        {
            workshop.StartDirective(directive);
        }

        private void BeginStartMember()
        {
        }

        private void StartObjectDirective()
        {
        }

        private void EndBeginEndObject()
        {
         
        }

        private void SetValueDirective()
        {
        }

        private void BeginEndMember()
        {
            if (AreTherePendingChildren())
            { 
                var children = CreateCollection(Previous.Member.XamlType, Previous.BufferedChildren);
                Current.Instance = children;
            }
        }

        private bool AreTherePendingChildren()
        {
            return Workbenches.Previous!= null && Previous.BufferedChildren.Count > 0;
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
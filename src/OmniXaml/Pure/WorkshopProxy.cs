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

        public WorkshopProxy(Workshop workshop)
        {
            this.workshop = workshop;
        }

        private StackingLinkedList<Workbench> Workbenches => workshop.Workbenches;
        private readonly StackingLinkedList<Directive> directives = new StackingLinkedList<Directive>();

        public Workbench Previous => Workbenches.PreviousValue;
        public Workbench Current => Workbenches.CurrentValue;

        public void StartObject(XamlType xamlType)
        {
            BeginStartObject();
            workshop.StartObject(xamlType);
            EndStartObject();
        }

        public void SetValue(object value)
        {
            BeginSetValue();
            workshop.SetValue(value);
            EndSetValue();
        }

        public void EndObject()
        {
            EndBeginEndObject();
            workshop.EndObject();
            EndEndObject();
        }

        public void EndMember()
        {
            BeginEndMember();

            if (!IsUnderDirective)
            {
                workshop.EndMember();
            }

            ConsumeDirective();
            
            EndEndMember();
        }

        private void ConsumeDirective()
        {
            directives.Pop();
        }

        public bool IsUnderDirective => directives.Current != null && directives.CurrentValue != null;

        public void StartMember(MemberBase member)
        {
            BeginStartMember();

            if (member.IsDirective)
            {
                workshop.Bump();
                PushDirective((Directive) member);
            }
            else
            {
                workshop.StartMember(member);
                PushNullDirective();
            }

            EndStartMember();
        }

        private void PushNullDirective()
        {
            directives.Push(null);
        }

        private void PushDirective(Directive directive)
        {
            directives.Push(directive);
        }

        private void EndStartMember()
        {
        }

        public bool CheckPreviousIsDirective(Directive directive)
        {
            return directives.Current != null && directive.Equals(directives.CurrentValue);
        }

        public void Collapse()
        {
            workshop.Collapse();
        }

        private void EndStartObject()
        {
        }

        private void EndSetValue()
        {
        }

        private void EndEndObject()
        {
            if (CheckPreviousIsDirective(CoreTypes.Items))
            {
                var instance = Current.Instance;
                Collapse();
                Previous.BufferedChildren.Add(instance);
            }
        }

        private void EndEndMember()
        {
        }

        private void BeginStartMember()
        {
        }

        private void BeginStartObject()
        {
        }

        private void EndBeginEndObject()
        {
        }

        private void BeginSetValue()
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
            return Workbenches.Previous != null && Previous.BufferedChildren.Count > 0;
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

    internal class NoDirective
    {
    }
}
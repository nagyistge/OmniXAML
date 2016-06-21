namespace OmniXaml.Pure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Glass.Core;
    using TypeConversion;
    using Typing;

    internal class WorkshopOld
    {
        private readonly IValueContext valueContext;
        private readonly Action<object> setResult;
        private readonly StackingLinkedList<Workbench> workbenches = new StackingLinkedList<Workbench>();

        private Workbench Current => workbenches.CurrentValue;
        public Workbench Previous => workbenches.PreviousValue;

        public WorkshopOld(IValueContext valueContext, Action<object> setResult)
        {
            this.valueContext = valueContext;
            this.setResult = setResult;
        }

        public void CreateInstanceIfNotYetCreated()
        {
            if (Current.Instance == null)
            {
                object instance;
                if (!Current.InitializationValues.Any())
                {
                    var groupedAssignments = Current.MemberAssignments.GroupBy(pair => pair.Key.IsWritable).ToList();

                    instance = CreateInstance(groupedAssignments.Where(pairs => !pairs.Key).SelectMany(pairs => pairs));
                    SetAssignments(instance, groupedAssignments.Where(pairs => pairs.Key).SelectMany(pairs => pairs));
                }
                else
                {
                    var owner = Previous.XamlType;
                    if (IsPrimitiveType(owner) && Current.InitializationValues.Count == 1)
                    {
                        instance = ConvertValue(owner.UnderlyingType, Current.InitializationValues.First());
                    }
                    else
                    {
                        var finalParams = PromoteStringValuesToCtorParameters(owner.UnderlyingType, Current.InitializationValues);
                        instance = Current.XamlType.CreateInstance(finalParams.Select(o => new InjectableValue(o)).ToArray());
                    }
                }

                Current.Instance = instance;
            }
        }

        private bool IsPrimitiveType(XamlType owner)
        {
            return owner.UnderlyingType == typeof(int);
        }

        private IEnumerable<object> PromoteStringValuesToCtorParameters(Type underlyingType, IEnumerable<object> initializationValues)
        {
            var method = underlyingType.GetTypeInfo().DeclaredConstructors.FirstOrDefault(ctor => ctor.GetParameters().Length == initializationValues.Count());
            return method
                .GetParameters()
                .Zip(initializationValues, (info, o) => ConvertValue(info.ParameterType, o));
        }

        private object ConvertValue(Type parameterType, object o)
        {
            var xamlType = valueContext.TypeRepository.GetByType(parameterType);
            object value;
            return CommonValueConversion.TryConvert(o, xamlType, valueContext, out value) ? value : o;
        }

        private object CreateInstance(IEnumerable<KeyValuePair<MutableMember, object>> ctorAssignments)
        {
            var parameters = ctorAssignments.Select(pair => new InjectableValue(pair.Key.Name, pair.Value)).ToArray();
            return Current.XamlType.CreateInstance(parameters);
        }

        private void SetAssignments(object instance, IEnumerable<KeyValuePair<MutableMember, object>> writableAssignments)
        {
            foreach (var assigment in writableAssignments)
            {
                var mutable = assigment.Key;
                mutable.SetValue(instance, assigment.Value, valueContext);
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
                object valueToAssign;
                if (Equals(Current.Member, CoreTypes.Items))
                {
                    valueToAssign = CreateCollection();
                }
                else
                {
                    valueToAssign = Current.Instance;
                }

                if (Current.Member?.IsDirective == true)
                {
                    Previous.Flag = true;
                }

                workbenches.Pop();

                if (Equals(Current.Member, CoreTypes.Initialization))
                {
                    StoreInitializationValue(valueToAssign);
                }
                else
                {
                    StoreAssignment(valueToAssign);
                }                    
            }
        }

        private void StoreInitializationValue(object instanceToAssign)
        {
            object converted;
            var finalValue = CommonValueConversion.TryConvert(instanceToAssign, Current.Member.XamlType, valueContext, out converted) ? converted : instanceToAssign;

            Current.InitializationValues.Add(finalValue);
        }

        private void StoreAssignment(object instanceToAssign)
        {
            object converted;
            var finalValue = CommonValueConversion.TryConvert(instanceToAssign, Current.Member.XamlType, valueContext, out converted) ? converted : instanceToAssign;

            Current.MemberAssignments.Add((MutableMember) Current.Member, finalValue);
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
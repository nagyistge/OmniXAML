namespace OmniXaml.Typing
{
    using System.Reflection;
    using TypeConversion;

    public class MemberValuePlugin : IMemberValuePlugin
    {
        private readonly MutableMember member;

        public MemberValuePlugin(MutableMember member)
        {
            this.member = member;
        }

        public virtual object GetValue(object instance)
        {
            if (ValueGetter.IsStatic)
            {
                return member.Getter.Invoke(null, new[] { instance });
            }
            else
            {
                return member.Getter.Invoke(instance, null);
            }
        }

        public virtual void SetValue(object instance, object value, IValueContext valueContext)
        {
            if (ValueSetter.IsStatic)
            {
                ValueSetter.Invoke(null, new[] { instance, value });
            }
            else
            {
                member.Setter.Invoke(instance, new[] { value });
            }
        }

        private MethodInfo ValueSetter
        {
            get
            {
                if (member.IsAttachable)
                {
                    var underlyingType = member.DeclaringType.UnderlyingType;
                    return underlyingType.GetTypeInfo().GetDeclaredMethod("Set" + member.Name);
                }
                else
                {
                    return member.DeclaringType.UnderlyingType.GetRuntimeProperty(member.Name).SetMethod;
                }
            }
        }

        private MethodInfo ValueGetter
        {
            get
            {
                if (member.IsAttachable)
                {
                    var underlyingType = member.DeclaringType.UnderlyingType;
                    return underlyingType.GetTypeInfo().GetDeclaredMethod("Get" + member.Name);
                }
                else
                {
                    return member.DeclaringType.UnderlyingType.GetRuntimeProperty(member.Name).GetMethod;
                }
            }
        }
    }
}
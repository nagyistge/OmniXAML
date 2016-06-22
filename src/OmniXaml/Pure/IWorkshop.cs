namespace OmniXaml.Pure
{
    using Typing;

    internal interface IWorkshop
    {
        void StartObject(XamlType xamlType);
        void EndObject();
        void StartMember(MemberBase member);
        void EndMember();
        void SetValue(object value);
    }
}
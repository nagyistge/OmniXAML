namespace OmniXaml
{
    public class InjectableValue
    {
        public InjectableValue(object value)
        {            
            Value = value;
        }

        public InjectableValue(string name, object value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; }
        public object Value { get; }
    }
}
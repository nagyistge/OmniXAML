namespace OmniXaml
{
    using ObjectAssembler;

    public interface IObjectAssemblerFactory
    {
        IObjectAssembler CreateAssembler(Settings settings);        
    }
}
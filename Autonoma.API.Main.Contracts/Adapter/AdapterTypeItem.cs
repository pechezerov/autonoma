namespace Autonoma.API.Main.Contracts.Adapter
{
    public class AdapterTypeItem
    {
        public string? AssemblyQualifiedConfigurationTypeName { get; set; }

        public string? AssemblyQualifiedAdapterTypeName { get; set; }

        public string? Configuration { get; set; }

        public string Name { get; set; } = "";
    }
}
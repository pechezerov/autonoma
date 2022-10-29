namespace Autonoma.API.Main.Contracts.ModelElement
{
    public class ModelElementConfigurationItem
    {
        public int? DataPointId { get; set; }

        public string Name { get; set; }

        public int? ParentElementId { get; set; }

        public int TemplateId { get; set; }
    }
}

using System.Collections.Generic;

namespace k8.Template.Engine.TemplatingConfigsModels
{
    public class ResourceSpec : ResourceTemplate
    {
        public string Id { get; set; }

        public List<ResourceTemplate> Transforms { get; set; } = new List<ResourceTemplate>();
    }
}
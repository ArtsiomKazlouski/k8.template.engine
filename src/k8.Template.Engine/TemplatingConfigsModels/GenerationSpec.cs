using System.Collections.Generic;

namespace k8.Template.Engine.TemplatingConfigsModels
{
    public class GenerationSpec
    {
        public List<ResourceSpec> Resources { get; set; }

        public List<Parameter> Parameters { get; set; }
    }
}
using System.Collections.Generic;

namespace k8.Template.Engine.TemplatingConfigsModels
{
    public class ResourceTemplate
    {
        public string Template { get; set; }
        public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
    }
}
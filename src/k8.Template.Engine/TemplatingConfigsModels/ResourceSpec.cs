using System.Collections.Generic;

namespace k8.Template.Engine.TemplatingConfigsModels
{
    public class ResourceSpec
    {
        public string Id { get; set; }

        public string Template { get; set; }

        public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();

        public List<Transform> Transforms { get; set; } = new List<Transform>();
    }
}
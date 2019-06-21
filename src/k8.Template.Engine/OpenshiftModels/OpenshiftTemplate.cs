using System.Collections.Generic;

namespace k8.Template.Engine.OpenshiftModels
{
    public class OpenshiftTemplate
    {
        public string apiVersion { get; set; } = "v1";

        public string kind { get; set; } = "Template";

        public Metadata metadata { get; set; } = new Metadata();

        public List<object> objects { get; set; } = new List<object>();

        public List<object> parameters { get; set; } = new List<object>();

    }
}
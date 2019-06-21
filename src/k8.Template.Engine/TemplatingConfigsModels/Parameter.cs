using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace k8.Template.Engine.TemplatingConfigsModels
{
    public class Parameter
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
        [YamlMember(ScalarStyle = ScalarStyle.SingleQuoted)]
        public string value { get; set; }
    }
}

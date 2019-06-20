namespace k8.Template.Engine
{
    public class BuilderConfig
    {
        public string GenerationRootFolder { get; set; }
        public string EnvironmentsOverridesFolder { get; set; }
        public string TemplateFileName { get; set; }
        public bool ValidateTemplateParameters { get; set; }
    }
}
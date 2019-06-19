using System;
using System.IO;
using Xunit;

namespace k8.Template.Engine.Test
{
    public class YamlBuilderTest
    {
        private DirectoryInfo _transformSourceDirectory;

        public YamlBuilderTest()
        {
            _transformSourceDirectory = new DirectoryInfo("D:\\Work\\GitRepositories\\signing\\openshift");
        }

        [Fact]
        public void ShouldBuild()
        {
            var dir = "../../../../../openshift";

            var builderConfig = new BuilderConfig()
            {
                GenerationRootFolder = dir,
                TemplateFileName = "base-eds-template.yaml",
                EnvironmentsOverridesFolder = "environment-transformations"
            };

            var builder = new OpenshiftTemplateBuilder(builderConfig);
            var results = builder.Build();
            
            Assert.NotNull(results);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using k8.Template.Engine.OpenshiftModels;
using k8.Template.Engine.TemplatingConfigsModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using YamlDotNet.Serialization;

namespace k8.Template.Engine
{
    public class OpenshiftTemplateBuilder
    {
        private readonly DirectoryInfo _generationRootFolder;
        private readonly FileInfo _templateFile;
        private readonly DirectoryInfo _environmentsOverridesFolder;

        public OpenshiftTemplateBuilder(BuilderConfig config)
        {
            _generationRootFolder = new DirectoryInfo(config.GenerationRootFolder);
            _environmentsOverridesFolder = new DirectoryInfo(Path.Combine(_generationRootFolder.FullName, config.EnvironmentsOverridesFolder));
            var templateFile = Path.Combine(_generationRootFolder.FullName, config.TemplateFileName);
            _templateFile = new FileInfo(templateFile);
        }

        public List<GenerationResult> Build()
        {
            var templateFile = ReadFileAsJson(_templateFile.FullName);

            var environmentsTemplates = new Dictionary<string,string>();

            foreach (var file in _environmentsOverridesFolder.EnumerateFiles())
            {
                var templateTransform = ReadFileAsJson(file.FullName);


                try
                {
                    var envTemplate = templateFile.ApplyTransform(templateTransform);
                    environmentsTemplates.Add(file.Name, envTemplate);
                }
                catch (Exception e)
                {
                    throw new Exception($"Can't apply transform, from file: {file.FullName}", e);
                }                
            }

            var result = new List<GenerationResult>();

            foreach (var environmentsTemplate in environmentsTemplates)
            {
                var config = JsonConvert.DeserializeObject<GenerationSpec>(environmentsTemplate.Value);

                var resources = new List<string>();

                foreach (var resource in config.Resources)
                {
                    var template = PrepareTemplate(resource);

                    foreach (var transform in resource.Transforms)
                    {
                        try
                        {
                            template = template.ApplyTransform(PrepareTransform(transform));
                        }
                        catch (Exception e)
                        {
                            throw new Exception($"Can't apply transform, from file: {transform.Template}", e);
                        }
                    }

                    resources.Add(template);
                }

                var generatedTemplate = BuildTemplate(resources, config.Parameters);
                result.Add(new GenerationResult()
                {
                    EnvName = Path.GetFileNameWithoutExtension(environmentsTemplate.Key),
                    Template = generatedTemplate
                });
            }

            //validate templates

            return result;
        }

        public string PrepareTransform(Transform transform)
        {
            var segments = new List<string>()
            {
                _generationRootFolder.FullName,
                "resourceTemplates",
            };

            var subSegments = transform.Template.Split('/', StringSplitOptions.RemoveEmptyEntries);
            segments.AddRange(subSegments.Take(subSegments.Length - 1));
            segments.Add(subSegments.Last() + ".yaml");

            var transformPath = Path.Combine(segments.ToArray());
            

            var transformContent = ReadFileAsJson(transformPath);

            return SubstituteParams(transformContent, transform.Params);
        }

        public string PrepareTemplate(ResourceSpec resource)
        {
            var segments = new List<string>()
            {
                _generationRootFolder.FullName,
                "resourceTemplates",
            };

            var subSegments = resource.Template.Split('/', StringSplitOptions.RemoveEmptyEntries);
            segments.AddRange(subSegments.Take(subSegments.Length - 1));
            segments.Add(subSegments.Last()+ ".yaml");

            var templatePath = Path.Combine(segments.ToArray());

            var template = ReadFileAsJson(templatePath);

            return SubstituteParams(template, resource.Params);
        }

        private string ReadFileAsJson(string filePath)
        {
            var fileContent = File.ReadAllText(filePath);

            if (filePath.EndsWith(".yaml"))
            {
                fileContent = fileContent.FromYamlToJson();
            }

            return fileContent;
        }

        private string SubstituteParams(string json, Dictionary<string, string> replacements)
        {
            var result = json;
            foreach (var replacement in replacements)
            {
                result = result.Replace($"%{{{replacement.Key.ToUpper()}}}", replacement.Value);
            }

            return result;

            //TODO test for not replaced parameters
        }

        private string BuildTemplate(List<string> resources, List<Parameter> parameters)
        {
            var template = new OpenshiftTemplate();
            template.objects = new List<object>(GetResources(resources));
            template.parameters = new List<object>(parameters);

            var serializer = new SerializerBuilder().Build();
            return serializer.Serialize(template);
        }

        private IEnumerable<object> GetResources(List<string> resource)
        {
            foreach (var res in resource)
            {
                var expConverter = new ExpandoObjectConverter();
                yield return JsonConvert.DeserializeObject<ExpandoObject>(res, expConverter);
            }
        }

    }

    public class BuilderConfig
    {
        public string GenerationRootFolder { get; set; }
        public string EnvironmentsOverridesFolder { get; set; }
        public string TemplateFileName { get; set; }
    }
}

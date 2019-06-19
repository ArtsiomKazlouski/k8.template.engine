using System.IO;
using Microsoft.VisualStudio.Jdt;
using YamlDotNet.Serialization;

namespace k8.Template.Engine
{
    internal static class Extensions
    {
        public static string FromYamlToJson(this string yaml)
        {
            var deserializer = new DeserializerBuilder().Build();

            var yamlObject = deserializer.Deserialize(new StringReader(yaml));

            var serializer = new SerializerBuilder()
                .JsonCompatible()
                .Build();

            return serializer.Serialize(yamlObject);
        }

        public static string ApplyTransform(this string source, string transform)
        {
            using (var transformStream = transform.AsStream())
            using (var sourceStream = source.AsStream())
            {
                JsonTransformation transformation = new JsonTransformation(transformStream, null);
                var result = transformation.Apply(sourceStream);
                return result.ReadAsString();
            }
        }

        public static string ReadAsString(this Stream s)
        {
            using (var reader = new StreamReader(s))
            {
                return reader.ReadToEnd();
            }
        }

        public static Stream AsStream(this string s)
        {
            MemoryStream stringStream = new MemoryStream();
            StreamWriter stringWriter = new StreamWriter(stringStream);
            stringWriter.Write(s);
            stringWriter.Flush();
            stringStream.Position = 0;

            return stringStream;
        }
    }
}

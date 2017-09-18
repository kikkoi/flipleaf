using System.IO;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FlipLeaf.Core.Text
{
    public class YamlConfigParser
    {
        public SiteConfiguration ParseConfig(string path)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            using (var file = File.OpenRead(path))
            using (var reader = new StreamReader(file))
            {
                var parser = new Parser(reader);
                parser.Expect<StreamStart>();

                return deserializer.Deserialize<SiteConfiguration>(parser);
            }
        }
    }
}
